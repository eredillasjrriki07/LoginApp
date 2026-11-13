using LoginApp.Application.Users;
using LoginApp.Application.Users.Dtos;
using LoginApp.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace LoginApp.Infrastructure.Persistence;

internal sealed class UserQueries(LoginAppDbContext _db) : IUserQueries
{
    public Task<UserDto?> GetAsync(Guid id, CancellationToken ct = default) =>
        _db.Users.AsNoTracking()
            .Where(u => u.Id == id && u.IsActive)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CreatedAt = EF.Property<DateTime>(u, AuditColumn.CreatedAt),
                UpdatedAt = EF.Property<DateTime>(u, AuditColumn.UpdatedAt),
                Version = EF.Property<uint>(u, RowVersion.Name),
            })
            .SingleOrDefaultAsync(ct);
}
