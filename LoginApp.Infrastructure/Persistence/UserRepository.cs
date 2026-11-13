using LoginApp.Domain.Repositories;
using LoginApp.Domain.Users;
using LoginApp.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace LoginApp.Infrastructure.Persistence;

internal sealed class UserRepository(LoginAppDbContext _db) : IUserRepository
{
    public Task<User?> GetAsync(Guid id, CancellationToken ct = default) =>
        _db.Users.SingleOrDefaultAsync(u => u.Id == id, ct);

    public void Add(User user) => _db.Users.Add(user);

    public void ExpectVersion(User user, uint version)
    {
        var loaded = _db.Entry(user).Property<uint>(RowVersion.Name).OriginalValue;
        if (loaded != version)
            throw new DbUpdateConcurrencyException(
                $"User {user.Id} is at version {loaded}; the request was made against version {version}");
    }

}
