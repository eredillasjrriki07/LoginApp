using LoginApp.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginApp.Infrastructure.Persistence.Configuration;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(p => p.Username).HasColumnName("username").HasMaxLength(Username.MaxLength).IsRequired();
        builder.Property(p => p.Email).HasColumnName("email").HasMaxLength(Email.MaxLength).IsRequired();
        builder.Property(p => p.PasswordHash).HasColumnName("password_hash").IsRequired();
        builder.Property(p => p.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        builder.Ignore(p => p.DomainEvents);

        builder.HasIndex(p => p.Username, "ix_users_username");

        AuditColumn.Apply(builder);
        RowVersion.Apply(builder);
    }
}
