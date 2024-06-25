using Lealthy_Hospital_Application_System.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lealthy_Hospital_Application_System.Views.Shared.Components.Menu;

public class Menu : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        string userSession = HttpContext.Session.GetString("staffLoggedSession");
        
        if (string.IsNullOrEmpty(userSession)) return null;

        StaffModel user = JsonConvert.DeserializeObject<StaffModel>(userSession);
        return View(user);
    }
}