using LoginApp.Domain.Users;

namespace LoginApp.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetAsync(Guid id, CancellationToken ct = default);

    void Add(User user);

    void ExpectVersion(User user, uint version);
}
