using LoginApp.Domain.Common;
using LoginApp.Domain.Users.Events;

namespace LoginApp.Domain.Users;

public sealed class User : Entity, IDomainEventCarrier
{
    private readonly List<object> _events = [];

    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public IReadOnlyList<object> DomainEvents => _events;

    public User() { }

    public User(string username, string email, string passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }

    public static User Create(Username username, Email email, string passwordHash)
    {
        var user = new User
        {
            Id = Guid.CreateVersion7(),
        };
        user.SetDetails(username, email, passwordHash);
        user._events.Add(new UserCreatedEvent(user.Id, user.Username, user.Email));
        return user;
    }

    // Making the update method simple for demo purposes.
    // In real world app, only changed field should be in
    // the request body. :)
    public void Update(Username username, Email email, string passwordHash)
    {
        EnsureUserActive();

        var changes = new List<UserFieldChanged>(2);
        Track(changes, "username", Username, username.Value);
        Track(changes, "email", Email, email.Value);
        Track(changes, "password_hash", PasswordHash, passwordHash);
        if (changes.Count == 0)
            return;

        SetDetails(username, email, passwordHash);
        _events.Add(new UserUpdatedEvent(Id, Username, changes));

    }

    private void SetDetails(Username username, Email email, string passwordHash)
    {
        Username = username.Value;
        Email = email.Value;
        PasswordHash = passwordHash;
    }

    private void EnsureUserActive() => Rule.Require(IsActive, UserRules.Inactive);

    private static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    public void ClearDomainEvents() => _events.Clear();

    private static void Track(List<UserFieldChanged> changes, string field, string? from, string? to)
    {
        if (!string.Equals(from, to, StringComparison.Ordinal))
            changes.Add(new UserFieldChanged(field, from, to));
    }

}

public static class UserRules
{
    public const string Inactive = "This user is inactive.";
}