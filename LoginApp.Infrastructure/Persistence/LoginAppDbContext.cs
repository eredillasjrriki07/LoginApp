using LoginApp.Domain.Users;
using LoginApp.Infrastructure.Persistence.Configuration;
using LoginApp.Infrastructure.Persistence.Events;
using Microsoft.EntityFrameworkCore;

namespace LoginApp.Infrastructure.Persistence;

public class LoginAppDbContext(DbContextOptions<LoginAppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<DomainEventRow> DomainEvents => Set<DomainEventRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoginAppDbContext).Assembly);

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        StampRoots();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        StampRoots();
        return base.SaveChanges();
    }

    private void StampRoots()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Metadata.FindProperty(AuditColumn.CreatedAt) is null)
                continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(AuditColumn.CreatedAt).CurrentValue = now;
                    entry.Property(RowVersion.Name).CurrentValue = 1u;
                    break;

                case EntityState.Modified:
                    entry.Property(AuditColumn.UpdatedAt).CurrentValue = now;
                    entry.Property(AuditColumn.CreatedAt).IsModified = false;

                    var version = entry.Property(RowVersion.Name);
                    version.CurrentValue = (uint)version.OriginalValue + 1;
                    break;
            }

        }
    }
}
