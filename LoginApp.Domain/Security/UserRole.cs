namespace LoginApp.Domain.Security;

public sealed class UserRole
{
    public Guid UserId { get; private set; }
    public int RoleId { get; private set; }

    public UserRole() { }
    public UserRole(Guid userId, int roleId) { UserId = userId; RoleId = roleId; }
}
