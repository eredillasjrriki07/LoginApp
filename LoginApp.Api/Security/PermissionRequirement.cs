using Microsoft.AspNetCore.Authorization;

namespace LoginApp.Api.Security;

public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; } = string.Empty;
    public PermissionRequirement(string permission) => Permission = permission;
}
