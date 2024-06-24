using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Lealthy_Hospital_Application_System.Controllers;

public class PasswordController : Controller
{
    private readonly IStaffSession _staffSession;
    private readonly IPasswordChange _passwordChange;
    private readonly IEmail _email;
    public PasswordController(IStaffSession _staffSession, IPasswordChange _passwordChange,
        IEmail _email)
    {
        this._staffSession = _staffSession;
        this._passwordChange = _passwordChange;
        this._email = _email;
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

    public IActionResult ResetPassword()
    {
        return View();
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

    [HttpPost]
    public IActionResult SendResetPasswordLink(ResetPasswordModel resetPasswordModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                StaffModel staffModel = _passwordChange.GetStaffByLoginAndEmail(resetPasswordModel.Login, resetPasswordModel.Email);
            
            if (staffModel != null)
            {
                string NewPassword = _passwordChange.GeneratePassword();

                string message = $"You have requested a new password.\n If you did not request considering change " +
                                 $"your current password for safety \n" +
                                 $"Your new Password is: {NewPassword}";

                bool SendMessage = _email.SendEmailLink(staffModel.Email, message, "LHAS - Lealthy Hospital Management System");

                if (SendMessage)
                {
                    staffModel.Password = BCrypt.Net.BCrypt.HashPassword(NewPassword);
                    _passwordChange.ResetPassword(staffModel);
                    return RedirectToAction("ResetPassword");
                }
            }
            }

            return RedirectToAction("ResetPassword");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
  
}