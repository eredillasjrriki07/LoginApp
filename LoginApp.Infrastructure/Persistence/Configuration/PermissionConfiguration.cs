using LoginApp.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginApp.Infrastructure.Persistence.Configuration;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.HasIndex(p => p.Name).IsUnique();

        builder.HasData(
            new { Id = 1, Name = Permissions.UsersView },
            new { Id = 2, Name = Permissions.UsersCreate },
            new { Id = 3, Name = Permissions.UsersUpdate },
            new { Id = 4, Name = Permissions.UsersDelete }
        );
    }
}
