namespace LoginApp.Domain.Users.Events;

public sealed record UserCreatedEvent(Guid UserId, string Username, string Email);
