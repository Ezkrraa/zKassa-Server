using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RoleCheck : Attribute, IAuthorizationFilter
{
    private readonly Permission _permissionRequired;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        UserManager<Employee> userManager =
            context.HttpContext.RequestServices.GetService(typeof(UserManager<Employee>))
                as UserManager<Employee>
            ?? throw new ArgumentNullException("Cannot get a Usermanager");

        Employee? employee = userManager
            .GetUserAsync(context.HttpContext.User)
            .GetAwaiter()
            .GetResult();

        if (employee == null)
            context.Result = new UnauthorizedResult();
        else if (!PermissionMethods.IsAtLeast(_permissionRequired, employee.Role))
            context.Result = new ForbidResult();
    }

    public RoleCheck(Permission permissionRequired)
    {
        _permissionRequired = permissionRequired;
    }
}
