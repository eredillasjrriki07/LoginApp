namespace LoginApp.Domain.Authentication;

public sealed class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }

    public RefreshToken() { }

    public RefreshToken(Guid userId, string tokenHash, DateTime now, DateTime expiresAt)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TokenHash = tokenHash;
        CreatedAt = now;
        ExpiresAt = expiresAt;
    }

    public void Revoke(DateTime now, string? replacedByHash = null)
    {
        RevokedAt = now;
        ReplacedByTokenHash = replacedByHash;
    }
}
