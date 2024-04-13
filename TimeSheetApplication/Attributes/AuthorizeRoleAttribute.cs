using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class AuthorizeRoleAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly int _requiredRoleId;

    public AuthorizeRoleAttribute(int requiredRoleId)
    {
        _requiredRoleId = requiredRoleId;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        var roleClaim = user.Claims.FirstOrDefault(c => c.Type == "RoleId");
        if (roleClaim == null || !int.TryParse(roleClaim.Value, out var roleId) || roleId != _requiredRoleId)
        {
            context.Result = new ForbidResult();
        }
    }
}
