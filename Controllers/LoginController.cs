using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Lealthy_Hospital_Application_System.Controllers;

public class LoginController : Controller
{
    private readonly IStaffRepository _staffRepository;

    public LoginController(IStaffRepository _staffRepository)
    {
        this._staffRepository = _staffRepository;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
            StaffModel Staff = _staffRepository.GetByLogin(loginModel.Login);

            if (Staff != null)
            {
                if (Staff.ValidPassword(loginModel.Password))
                {
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