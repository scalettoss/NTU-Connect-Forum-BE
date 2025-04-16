using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ForumBE.Helpers;

public class AuthorizeRolesAttribute : Attribute, IAuthorizationFilter
{
    private static readonly Dictionary<string, int> RoleHierarchy = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
    {
        { "User", 1 },
        { "Moderator", 2 },
        { "Admin", 3 }
    };

    private readonly string[] _allowedRoles;

    public AuthorizeRolesAttribute(params string[] roles)
    {
        _allowedRoles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new JsonResult(new
            {
                status = 400,
                message = "You are not login"
            });
            return;
        }

        var roleClaim = user.FindFirst("RoleName");

        if (roleClaim == null)
        {
            context.Result = new JsonResult(new
            {
                status = 400,
                message = "You do not have permission"
            });
            return;
        }

        var userRole = roleClaim.Value;

        if (!RoleHierarchy.TryGetValue(userRole, out var userRoleLevel))
        {
            context.Result = new JsonResult(new
            {
                status = 400,
                message = "Invalid user role"
            });
            return;
        }

        // Lấy cấp độ thấp nhất trong danh sách role được phép truy cập
        var minRequiredRoleLevel = _allowedRoles
            .Where(r => RoleHierarchy.ContainsKey(r))
            .Select(r => RoleHierarchy[r])
            .Min();

        // Nếu cấp độ của user >= cấp độ tối thiểu => pass
        if (userRoleLevel < minRequiredRoleLevel)
        {
            context.Result = new JsonResult(new
            {
                status = 400,
                message = "You do not have permission"
            });
        }
    }
}
