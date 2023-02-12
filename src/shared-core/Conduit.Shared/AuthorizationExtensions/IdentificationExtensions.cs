using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Conduit.Shared.AuthorizationExtensions;

public static class IdentificationExtensions
{
    public static Guid GetCurrentUserId(
        this HttpContext httpContext)
    {
        var typedValue = GetCurrentUserIdOptional(httpContext) ??
                         throw new InvalidOperationException(
                             "Empty identification claim");
        return typedValue;
    }

    public static Guid? GetCurrentUserIdOptional(
        this HttpContext httpContext)
    {
        var claim = GetClaim(httpContext);

        var currentUserId = claim?.Value;
        _ = Guid.TryParse(currentUserId, out var typedCurrentUserId);
        return typedCurrentUserId;
    }

    private static Claim? GetClaim(
        HttpContext httpContext)
    {
        return FindFirst(httpContext);
    }

    private static Claim? FindFirst(
        HttpContext httpContext)
    {
        return httpContext.User.Identity?.IsAuthenticated == true
            ? httpContext.User.FindFirst(ClaimTypes.NameIdentifier)
            : null;
    }
}
