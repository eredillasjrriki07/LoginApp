namespace LoginApp.Application.Security;

public interface IRoleRepository
{
    Task<IReadOnlyCollection<string>> GetPermissionsAsync(Guid userId, CancellationToken ct = default);

    void AssignRole(Guid userId, int roleId);
}
