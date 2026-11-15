namespace LoginApp.Application.Authentication;

public interface IRefreshTokenGenerator
{
    (string Raw, string Hash) Generate();
    string Hash(string raw);
}
