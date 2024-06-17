using Lealthy_Hospital_Application_System.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

[UserLoggedPage]
public class RestrictController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }
}