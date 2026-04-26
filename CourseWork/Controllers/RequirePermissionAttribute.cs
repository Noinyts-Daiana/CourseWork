using System.Security.Claims;
using CourseWork.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CourseWork.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequirePermissionAttribute(string permissionName) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User;

        if (!(user.Identity?.IsAuthenticated ?? false))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var roleIdClaim = user.FindFirstValue("roleId");
        if (roleIdClaim == null || !int.TryParse(roleIdClaim, out var roleId))
        {
            context.Result = new ForbidResult();
            return;
        }

        var permService = context.HttpContext.RequestServices
            .GetRequiredService<IPermissionService>();

        var hasPermission = await permService.RoleHasPermissionAsync(roleId, permissionName);

        if (!hasPermission)
        {
            context.Result = new ObjectResult(
                    new { message = $"Недостатньо прав: '{permissionName}'" })
                { StatusCode = 403 };
            return;
        }

        await next();
    }
}