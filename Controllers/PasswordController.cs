using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

public class PasswordController : Controller
{
    private readonly IStaffSession _staffSession;
    private readonly IPasswordChange _passwordChange;

    public PasswordController(IStaffSession _staffSession, IPasswordChange _passwordChange)
    {
        this._staffSession = _staffSession;
        this._passwordChange = _passwordChange;
    }
    public IActionResult ChangePassword()
    {
        var Staff = _staffSession.GetLoginSession();
        ChangePasswordModel StaffId = new ChangePasswordModel();
        StaffId.StaffId = Staff.StaffId;
        ViewBag.Staff = Staff.Name;
        ViewBag.Access = Staff.Access;
        return View(StaffId);
    }

    [HttpPost]
    public IActionResult ChangeStaffPassword(ChangePasswordModel changePasswordModel)
    {
        try
        {
            StaffModel userLogged = _staffSession.GetLoginSession();
            changePasswordModel.StaffId = userLogged.StaffId;
            if (ModelState.IsValid)
            {
                _passwordChange.ChangePassword(changePasswordModel);
                TempData["SuccessMessage"] = "Password has been successful updated.";
                return RedirectToAction("ChangePassword", changePasswordModel);
            }
            TempData["ErrorMessage"] = "Error when trying to update your password. Contact your administrator.";
            return View("ChangePassword", changePasswordModel);
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Error when trying to update your password.{error.Message}";
            return View("ChangePassword", changePasswordModel);
        }
    }
  
}