using LoginApp.Application.Common;
using System.Security.Principal;

namespace LoginApp.Infrastructure.Persistence;

public sealed class UserUnitOfWork(LoginAppDbContext _db) : IUserUnitOfWork
{
    private bool _executing = false;

    public Task ExecuteAsync(Func<CancellationToken, Task> work, CancellationToken ct = default) =>
        ExecuteAsync<bool>(async token => { await work(token); return true; }, ct);

    public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> work, CancellationToken ct = default)
    {
        if (_executing)
            throw new InvalidOperationException("ExecuteAsync is not re-entrant");
        _executing = true;

        try
        {
            await using var transaction = await _db.Database.BeginTransactionAsync(ct);

            var result = await work(ct);

            await _db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return result;
        }
        finally
        {
            _executing = false;
        }
    }
}
