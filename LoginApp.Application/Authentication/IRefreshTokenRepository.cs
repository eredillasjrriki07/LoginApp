using LoginApp.Domain.Authentication;

namespace LoginApp.Application.Authentication;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByHashAsync(string Hash, CancellationToken ct = default);

    void Add(RefreshToken token);

    Task RevokeAllForUserAsync(Guid userId, DateTime now, CancellationToken ct);
}
