namespace LoginApp.Application.Users.Dtos;

public sealed record UserDto
{
    public Guid Id { get; set; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public uint Version { get; init; }
}
