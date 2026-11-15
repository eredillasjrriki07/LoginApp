using System.Security.Claims;

namespace LoginApp.Api.Extensions;

public static class ClaimsPrincipalExtension
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue("sub");

        return Guid.TryParse(value, out var id)
            ? id
            : throw new UnauthorizedAccessException("Invalid user id claim");
    }
}
