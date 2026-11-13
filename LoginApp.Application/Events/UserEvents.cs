using LoginApp.Domain.Users.Events;

namespace LoginApp.Application.Events;

public static class UserEvents
{
    public static (string EventType, object Payload) For(object domainEvent) => domainEvent switch
    {
        UserCreatedEvent e => (UserEventTypes.UserCreated, UserCreatedPayload(e)),
        _ => throw new InvalidOperationException($"{domainEvent.GetType().Name} has no user event mapping; add one before raising it.")
    };

    public static string EntityId(Guid id) => id.ToString("D");

    public static object UserCreatedPayload(UserCreatedEvent e) => new
    {
        summary = $"Created user with id {e.UserId}",
    };
}

public static class UserEventTypes
{
    public const string UserEntityType = "User";
    public const string UserCreated = "loginapp.user.created";
}
