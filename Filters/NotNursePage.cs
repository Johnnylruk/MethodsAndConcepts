using Lealthy_Hospital_Application_System.Enum;
using Lealthy_Hospital_Application_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Lealthy_Hospital_Application_System.Filters;

public class NotNursePage : ActionFilterAttribute
{

    public override void OnActionExecuted(ActionExecutedContext context)
    {

        string userSession = context.HttpContext.Session.GetString("staffLoggedSession");

        if (string.IsNullOrEmpty(userSession))
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary{{"controller" , "Login"} , {"action" , "Index"}});
        }

        StaffModel staffModel = JsonConvert.DeserializeObject<StaffModel>(userSession);

        if (staffModel == null)
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary{{"controller" , "Login"} , {"action" , "Index"}});
        }

        if (staffModel.Access == RoleAccessEnum.Nurse)
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary{{"controller" , "Restrict"} , {"action" , "Index"}});
        }
        base.OnActionExecuted(context);
    }
    
}