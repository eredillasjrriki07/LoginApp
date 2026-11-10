using LoginApp.Domain.Common;

namespace LoginApp.Domain.Users;

public sealed class User : Entity, IDomainEventCarrier
{
    private readonly List<object> _events = [];

    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyList<object> DomainEvents => _events;

    public User()
    {

    }

    public void ClearDomainEvents()
    {
        throw new NotImplementedException();
    }
}
