using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Marka.Api.Services;
using System.Security.Claims;

namespace Marka.Api.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _permissionCode;

    public RequirePermissionAttribute(string permissionCode)
    {
        _permissionCode = permissionCode;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Check if user is authenticated
        if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Get user ID from claims
        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Get permission service from DI
        var permissionService = context.HttpContext.RequestServices
            .GetService<IPermissionService>();

        if (permissionService == null)
        {
            context.Result = new StatusCodeResult(500);
            return;
        }

        // Check if user has the required permission
        var hasPermission = await permissionService.UserHasPermissionAsync(userId, _permissionCode);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}
