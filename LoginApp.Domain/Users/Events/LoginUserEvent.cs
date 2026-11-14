namespace LoginApp.Domain.Users.Events;

public sealed record LoginUserEvent(Guid UserId, string Username, string Email);
