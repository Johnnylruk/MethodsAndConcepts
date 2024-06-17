using Lealthy_Hospital_Application_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Lealthy_Hospital_Application_System.Filters;

public class UserLoggedPage : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        string userSession = context.HttpContext.Session.GetString("staffLoggedSession");

        if (string.IsNullOrEmpty(userSession))
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary{{"controller", "Login"}, {"action", "Index"}});
        }
        else
        {
            StaffModel staff = JsonConvert.DeserializeObject<StaffModel>(userSession);

            if (staff == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary{{"controller", "Login"}, {"action", "Index"}});
            }
        }
        base.OnActionExecuted(context);
    }
}