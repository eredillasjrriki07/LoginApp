using FluentValidation;
using LoginApp.Application.Authentication.Dtos;
using LoginApp.Application.Authentication.RequestDtos;
using LoginApp.Application.Common;
using LoginApp.Application.Common.Exceptions;
using LoginApp.Application.Events;
using LoginApp.Application.Users;
using LoginApp.Application.Users.Dtos;
using LoginApp.Application.Users.RequestDtos;
using LoginApp.Domain.Authentication;
using LoginApp.Domain.Common;
using LoginApp.Domain.Repositories;
using LoginApp.Domain.Users;
using Microsoft.Extensions.Logging;

namespace LoginApp.Application.Authentication;

public sealed class AuthService(
    ILogger<AuthService> _logger,
    IUserUnitOfWork _uow,
    IUserRepository _users,
    IPasswordHasher _passwordHasher,
    IUserQueries _queries,
    IDomainEventRecorder _recorder,
    ITokenProvider _tokenProvider,
    IRefreshTokenGenerator _refreshTokenGenerator,
    IRefreshTokenRepository _refreshTokens,
    IClock _clock,
    IValidator<CreateUserRequestDto> _createValidator,
    IValidator<LoginRequestDto> _loginValidator
) : IAuthService
{

    private static readonly TimeSpan RefreshLifetime = TimeSpan.FromDays(7);

    public async Task<UserDto> RegisterAsync(CreateUserRequestDto request, CancellationToken ct = default)
    {
        await _createValidator.ValidateAndThrowDomainAsync(request);
        var username = new Username(request.Username);
        var email = new Email(request.Email);

        var user = await _uow.ExecuteAsync(_ =>
        {
            if (_users.EmailExists(email.Value))
                throw new EmailExistsException(email.Value);

            var passwordHash = _passwordHasher.Hash(request.Password);

            var created = User.Create(username, email, passwordHash);
            _users.Add(created);
            RecordEvent(created);
            return Task.FromResult(created);
        }, ct);

        _logger.LogInformation("User {UserId} created.", user.Id);
        return await ReadBackAsync(user.Id, ct);
    }

    public async Task<TokenDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
    {
        await _loginValidator.ValidateAndThrowDomainAsync(request);
        var email = new Email(request.Email);

        User? user = _users.GetUserByEmail(email.Value);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        var tokenDto = await _uow.ExecuteAsync(_ => Task.FromResult(IssueTokens(user, DateTime.UtcNow)), ct);

        _logger.LogInformation("User has logged in with id {Id}.", user.Id);
        return tokenDto;
    }

    public async Task<TokenDto> RefreshAsync(string refreshToken, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var stored = await _refreshTokens.GetByHashAsync(_refreshTokenGenerator.Hash(refreshToken), ct)
            ?? throw new InvalidRefreshTokenException();

        if (stored.RevokedAt is not null)
        {
            // Reuse of a revoked token: assume theft and kill every session.
            await _refreshTokens.RevokeAllForUserAsync(stored.UserId, now, ct);
            throw new InvalidRefreshTokenException();
        }

        if (stored.ExpiresAt <= now)
            throw new InvalidRefreshTokenException();

        var user = await _users.GetUserByIdAsync(stored.UserId, ct)
            ?? throw new InvalidRefreshTokenException();

        var tokenDto = await _uow.ExecuteAsync(_ => Task.FromResult(IssueTokens(user, DateTime.UtcNow, replacing: stored)), ct);

        _logger.LogInformation("Refresh token triggered with user id {Id}.", user.Id);
        return tokenDto;
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken ct)
    {
        var stored = await _refreshTokens.GetByHashAsync(_refreshTokenGenerator.Hash(refreshToken), ct);
        if (stored is null || stored.RevokedAt is not null) return;

        await _uow.ExecuteAsync(_ =>
        {
            stored.Revoke(DateTime.UtcNow);
            return Task.CompletedTask;
        }, ct);

        _logger.LogInformation("User has logged out with id {Id}.", stored.UserId);
    }

    private TokenDto IssueTokens(User user, DateTime now, RefreshToken? replacing = null)
    {
        var (raw, hash) = _refreshTokenGenerator.Generate();

        replacing?.Revoke(now, hash);
        _refreshTokens.Add(new RefreshToken(user.Id, hash, now, now.Add(RefreshLifetime)));

        return new TokenDto { AccessToken = _tokenProvider.Create(user), RefreshToken = raw };
    }

    private void RecordEvent(User user)
    {
        foreach (var domainEvent in user.DomainEvents)
        {
            var (eventType, payload) = UserEvents.For(domainEvent);
            _recorder.Record(eventType, UserEventTypes.UserEntityType, UserEvents.EntityId(user.Id), payload);
        }
        user.ClearDomainEvents();
    }

    private async Task<UserDto> ReadBackAsync(Guid id, CancellationToken ct)
        => await _queries.GetAsync(id, ct)
        ?? throw new InvalidOperationException($"User {id} was saved but could not be read back.");


}
