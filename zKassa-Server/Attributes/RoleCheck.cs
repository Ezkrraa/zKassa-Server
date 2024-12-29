using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RoleCheck : Attribute
{
    private readonly Permission _permissionRequired;

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        UserManager<Employee> userManager =
            context.HttpContext.RequestServices.GetService(typeof(UserManager<Employee>))
                as UserManager<Employee>
            ?? throw new ArgumentNullException("Cannot get a Usermanager");

        Employee? employee = await userManager.GetUserAsync(context.HttpContext.User);
        if (employee == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        if (!PermissionMethods.IsAtLeast(_permissionRequired, employee.Role))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // TODO: add logic for checking actual perms

        await next();
    }

    public RoleCheck(Permission permissionRequired)
    {
        _permissionRequired = permissionRequired;
    }
}
