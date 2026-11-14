using FluentValidation;
using LoginApp.Application.Common;
using LoginApp.Application.Common.Exceptions;
using LoginApp.Application.Events;
using LoginApp.Application.Users.Dtos;
using LoginApp.Application.Users.RequestDtos;
using LoginApp.Domain.Common;
using LoginApp.Domain.Repositories;
using LoginApp.Domain.Users;
using Microsoft.Extensions.Logging;

namespace LoginApp.Application.Users;

public sealed class AuthService(
    ILogger<AuthService> _logger,
    IUserUnitOfWork _uow,
    IUserRepository _users,
    IPasswordHasher _passwordHasher,
    IUserQueries _queries,
    IDomainEventRecorder _recorder,
    ITokenProvider _tokenProvider,
    IClock _clock,
    IValidator<CreateUserRequestDto> _createValidator,
    IValidator<LoginRequestDto> _loginValidator
) : IAuthService
{
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

    public async Task<string> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
    {
        await _loginValidator.ValidateAndThrowDomainAsync(request);
        var email = new Email(request.Email);

        var user = await _uow.ExecuteAsync(_ =>
        {
            User? user = _users.GetUserByEmail(email.Value);

            if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
                throw new InvalidCredentialsException();

            RecordEvent(user);
            return Task.FromResult(user);
        });

        _logger.LogInformation("User has logged in with user id {UserId} and {Email}", user.Id, user.Email);
        return _tokenProvider.Create(user);
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
