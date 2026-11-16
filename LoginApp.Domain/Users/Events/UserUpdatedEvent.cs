namespace LoginApp.Domain.Users.Events;

public sealed record UserUpdatedEvent(Guid UserId, string Username, IReadOnlyList<UserFieldChanged> Changes);

public sealed record UserFieldChanged(string Field, string? From, string? To);
