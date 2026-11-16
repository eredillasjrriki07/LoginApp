using Microsoft.AspNetCore.Authorization;

namespace LoginApp.Api.Security;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) : base(policy: permission) { }
}
