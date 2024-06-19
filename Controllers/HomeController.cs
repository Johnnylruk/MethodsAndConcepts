using System.Diagnostics;
using Lealthy_Hospital_Application_System.Filters;
using Lealthy_Hospital_Application_System.Helper;
using Microsoft.AspNetCore.Mvc;
using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Controllers;

[UserLoggedPage]
public class HomeController : Controller
{
    private readonly IStaffSession _staffSession;

    public HomeController(IStaffSession _staffSession)
    {
        this._staffSession = _staffSession;
    }

    public IActionResult Index()
    {
        var Staff = _staffSession.GetLoginSession();
        ViewBag.Staff = Staff.Name;
        ViewBag.Access = Staff.Access;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}