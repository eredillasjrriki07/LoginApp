using LoginApp.Application.Authentication.Dtos;
using LoginApp.Application.Authentication.RequestDtos;
using LoginApp.Application.Users.Dtos;
using LoginApp.Application.Users.RequestDtos;

namespace LoginApp.Application.Authentication;

public interface IAuthService
{
    Task<TokenDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default);

    Task<UserDto> RegisterAsync(CreateUserRequestDto request, CancellationToken ct = default);

    Task<UserDto> UpdateAsync(Guid id, UpdateUserRequestDto request, CancellationToken ct = default);

    Task<TokenDto> RefreshAsync(string refreshToken, CancellationToken ct = default);

    Task LogoutAsync(string refreshToken, CancellationToken ct = default);
}
