using LoginApp.Application.Security;
using LoginApp.Domain.Security;
using Microsoft.EntityFrameworkCore;

namespace LoginApp.Infrastructure.Persistence.Security;

internal sealed class RoleRepository(LoginAppDbContext _db) : IRoleRepository
{
    public async Task<IReadOnlyCollection<string>> GetPermissionsAsync(Guid userId, CancellationToken ct = default) =>
        await _db.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Join(_db.RolePermissions, ur => ur.RoleId, rp => rp.RoleId, (ur, rp) => rp.PermissionId)
            .Join(_db.Permissions, pid => pid, p => p.Id, (pid, p) => p.Name)
            .Distinct()
            .ToListAsync();

    public void AssignRole(Guid userId, int roleId) =>
        _db.UserRoles.Add(new UserRole(userId, roleId));

}
