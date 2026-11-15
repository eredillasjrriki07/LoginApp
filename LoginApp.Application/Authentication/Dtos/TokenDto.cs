namespace LoginApp.Application.Authentication.Dtos;

public sealed class TokenDto
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}
