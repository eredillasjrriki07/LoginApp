using LoginApp.Application.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace LoginApp.Infrastructure.Authentication;

internal sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public (string Raw, string Hash) Generate()
    {
        var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        return (raw, Hash(raw));
    }

    public string Hash(string raw) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(raw)));
}
