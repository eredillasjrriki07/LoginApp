using LoginApp.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginApp.Infrastructure.Persistence.Configuration;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permission");
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });
        builder.HasOne<Role>().WithMany().HasForeignKey(rp => rp.RoleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Permission>().WithMany().HasForeignKey(rp => rp.PermissionId).OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            // Admin: every permission
            new { RoleId = RoleIds.Admin, PermissionId = 1 },
            new { RoleId = RoleIds.Admin, PermissionId = 2 },
            new { RoleId = RoleIds.Admin, PermissionId = 3 },
            new { RoleId = RoleIds.Admin, PermissionId = 4 },
            // Viewer: read-only
            new { RoleId = RoleIds.Viewer, PermissionId = 1 }
        );
    }
}
