using Lealthy_Hospital_Application_System.Enum;
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
    
    public IActionResult Privacy()
    {
        var Staff = _staffSession.GetLoginSession();
        if (Staff != null)
        {
            ViewBag.Staff = Staff.Name;
            ViewBag.Access = Staff.Access;    
        }
        return View();
    }
    
    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        try
        {
                List<StaffModel> DBStaff = _staffRepository.GetAllStaff();
                if (DBStaff.IsNullOrEmpty())
                {
                    var TestAdmin = new StaffModel()
                    {
                        Name = "TestAdmin",
                        Email = "TestAdmin@gmail.com",
                        Mobile = "784578965",
                        Address = "Some Address",
                        DateOfBirth = DateTime.Today,
                        Access = RoleAccessEnum.Administrator,
                        Login = "TestAdmin",
                        Password = "TestAdmin@123"
                    };
                    _staffRepository.RegisterStaff(TestAdmin);
                    loginModel.Login = TestAdmin.Login;
                }
                
            StaffModel staff = _staffRepository.GetByLogin(loginModel.Login);
            
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginModel.Password, staff.Password);

            if (isValidPassword)
            {
                _staffSession.CreateLoginSession(staff);
                return RedirectToAction("Index", "Home");
            }
            
            TempData["ErrorMessage"] = "Login and/or password invalid";
            return View("Index");
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, could not login staff. Error: {error.Message}";
            return RedirectToAction("Index");
        }
    }
}