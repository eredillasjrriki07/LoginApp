using LoginApp.Domain.Users;

namespace LoginApp.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken ct = default);

    bool EmailExists(string email);

    void Add(User user);

    void ExpectVersion(User user, uint version);
}
