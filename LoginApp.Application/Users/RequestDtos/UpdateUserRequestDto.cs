namespace LoginApp.Application.Users.RequestDtos;

public sealed class UpdateUserRequestDto
{
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public uint Version { get; set; }
}
