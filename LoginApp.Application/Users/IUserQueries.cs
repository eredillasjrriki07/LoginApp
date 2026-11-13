using LoginApp.Application.Users.Dtos;

namespace LoginApp.Application.Users;

public interface IUserQueries
{
    Task<UserDto?> GetAsync(Guid id, CancellationToken ct = default);
}
