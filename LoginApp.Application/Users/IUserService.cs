using LoginApp.Application.Users.Dtos;
using LoginApp.Application.Users.RequestDtos;

namespace LoginApp.Application.Users;

public interface IUserService
{
    Task<UserDto> CrateAsync(CreateUserRequestDto request, CancellationToken ct = default);
}
