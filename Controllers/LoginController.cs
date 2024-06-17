using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Lealthy_Hospital_Application_System.Controllers;

public class LoginController : Controller
{
    private readonly IStaffRepository _staffRepository;
    private readonly IStaffSession _staffSession;

    public LoginController(IStaffRepository _staffRepository, IStaffSession _staffSession)
    {
        this._staffRepository = _staffRepository;
        this._staffSession = _staffSession;
    }
    public IActionResult Index()
    {
        if(_staffSession.GetLoginSession() != null) return RedirectToAction("Index", "Home");
        return View();
    }

    public IActionResult Logout()
    {
        _staffSession.RemoveLoginSession();
        return RedirectToAction("Index", "Login");
    }

    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
            StaffModel staff = _staffRepository.GetByLogin(loginModel.Login);

            if (staff != null)
            {
                if (staff.ValidPassword(loginModel.Password))
                {
                    _staffSession.CreateLoginSession(staff);
                    return RedirectToAction("Index", "Home");
                }
                TempData["ErrorMessage"] = "Login and/or password invalid";
            }
            TempData["ErrorMessage"] = $"Login and/or password invalid";
            
            }
            return View("Index");
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, could not delete staff. Error: {error.Message}";
            return RedirectToAction("Index");
        }
    }
}