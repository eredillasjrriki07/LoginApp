using LoginApp.Application.Authentication;

namespace LoginApp.Infrastructure.Persistence.Authentication;

internal sealed class BCryptPasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string Hash(string password)
        => BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor);

    public bool Verify(string password, string passwordHash)
        => BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash  );
}
