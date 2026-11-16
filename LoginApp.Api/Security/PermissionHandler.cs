using LoginApp.Infrastructure.Persistence.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace LoginApp.Api.Security;

public sealed class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.HasClaim(CustomClaims.Permission, requirement.Permission))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
