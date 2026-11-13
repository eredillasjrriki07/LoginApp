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

    private void SetDetails(Username username, Email email, string passwordHash)
    {
        Username = username.Value;
        Email = email.Value;
        PasswordHash = passwordHash;
    }

    private void EnsureUserActive() => Rule.Require(IsActive, UserRules.Inactive);

    public void ClearDomainEvents() => _events.Clear();

}

public static class UserRules
{
    public const string Inactive = "This user is inactive.";
}