using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceDay.MVC.Filters;

public class SessionAuthorizeAttribute : ActionFilterAttribute
{
    public string Roles { get; set; } = "";
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var role = context.HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role))
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }
        if (!string.IsNullOrEmpty(Roles) && !Roles.Split(',').Contains(role))
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
    }
}