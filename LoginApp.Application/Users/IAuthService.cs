using LoginApp.Application.Users.Dtos;
using LoginApp.Application.Users.RequestDtos;

namespace LoginApp.Application.Users;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(CreateUserRequestDto request, CancellationToken ct = default);
    Task<string> LoginAsync(LoginRequestDto request, CancellationToken ct = default);
}
