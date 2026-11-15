using LoginApp.Application.Authentication;
using LoginApp.Domain.Authentication;
using LoginApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoginApp.Infrastructure.Authentication;

internal sealed class RefreshTokenRepository(LoginAppDbContext _db) : IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByHashAsync(string hash, CancellationToken ct = default) =>
        _db.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash, ct);

    public void Add(RefreshToken token) => _db.RefreshTokens.Add(token);

    public Task RevokeAllForUserAsync(Guid userId, DateTime now, CancellationToken ct) =>
        _db.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, now), ct);
}
