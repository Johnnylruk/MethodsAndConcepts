using Lealthy_Hospital_Application_System.Filters;
using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

[OnlyAdministratorPage]
[UserLoggedPage]

public class StaffController : Controller
{
    private readonly IStaffRepository _staffRepository;
    private readonly IStaffSession _staffSession;

    public StaffController(IStaffRepository _staffRepository, IStaffSession _staffSession)
    {
        this._staffRepository = _staffRepository;
        this._staffSession = _staffSession;
    }
    
    public IActionResult Index()
    {
        var Staff = _staffSession.GetLoginSession();
        if (Staff != null)
        {
            List<StaffModel> ListAllStaff = _staffRepository.GetAllStaff();
            ViewBag.Staff = Staff.Name;
            ViewBag.Access = Staff.StaffType;
            return View(ListAllStaff);   
        }

        return RedirectToAction("Index", "Login");
    }

    public IActionResult CreateStaff()
    {
        var Staff = _staffSession.GetLoginSession();
        ViewBag.Staff = Staff.Name;
        ViewBag.Access = Staff.StaffType;
        return View();
    }

    public IActionResult UpdateStaff(int id)
    {
         var Staff = _staffSession.GetLoginSession();
                    ViewBag.Staff = Staff.Name;
                    ViewBag.Access = Staff.StaffType;
                    
        StaffModel staffId = _staffRepository.GetStaffById(id);
        var StaffWithoutPassword = new StaffWithoutPwdModel()
        {
            StaffId = staffId.StaffId,
            Name = staffId.Name,
            Email = staffId.Email,
            Mobile = staffId.Mobile,
            Address = staffId.Address,
            DateOfBirth = staffId.DateOfBirth,
            Access = staffId.StaffType,
            Login = staffId.Login
        };
        return View(StaffWithoutPassword);
    }
    public IActionResult DeleteStaff(int id)
    {
        StaffModel staffId = _staffRepository.GetStaffById(id);
        var Staff = _staffSession.GetLoginSession();
        ViewBag.Staff = Staff.Name;
        ViewBag.Access = Staff.StaffType;
        return View(staffId);
    }
    
    [HttpPost]
    public IActionResult CreateStaff(StaffModel staffModel)
    {
        try
        {
            StaffModel staffDB = _staffRepository.GetAllStaff()
                .FirstOrDefault(
                    x => x.Login == staffModel.Login ||
                         x.Mobile == staffModel.Mobile ||
                         x.Email == staffModel.Email
                );

            if (staffDB != null)
            {
                if (staffDB.Login == staffModel.Login)
                {
                    TempData["ErrorMessage"] = "Login already registered.";
                    return View("CreateStaff");
                }
                if (staffDB.Mobile == staffModel.Mobile)
                {
                    TempData["ErrorMessage"] = "Mobile already registered.";
                    return View("CreateStaff");
                }
                if (staffDB.Email == staffModel.Email)
                {
                    TempData["ErrorMessage"] = "Email already registered.";
                    return View("CreateStaff");
                }
                
            }
                _staffRepository.RegisterStaff(staffModel);
                TempData["SuccessMessage"] = "Staff has been created.";
                return RedirectToAction("Index");

        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, could not create staff. Error: {error.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult UpdateStaff(StaffWithoutPwdModel staffModel)
    {
        try
        {
            var Staff = _staffSession.GetLoginSession();
            ViewBag.Staff = Staff.Name;
            ViewBag.Access = Staff.StaffType;
            
            StaffModel duplicateEmailStaff = _staffRepository.GetAllStaff()
                .FirstOrDefault(x => x.Email == staffModel.Email &&
                                     x.StaffId != staffModel.StaffId);
            
            StaffModel duplicateMobileStaff = _staffRepository.GetAllStaff()
                .FirstOrDefault(x => x.Mobile == staffModel.Mobile &&
                                     x.StaffId != staffModel.StaffId); 
            StaffModel duplicateLoginStaff = _staffRepository.GetAllStaff()
                .FirstOrDefault(x => x.Login == staffModel.Login &&
                                     x.StaffId != staffModel.StaffId);

            if (duplicateEmailStaff != null)
            {
                TempData["ErrorMessage"] = "This email already exist.";
                return View();
            }
            if (duplicateMobileStaff != null)
            {
                TempData["ErrorMessage"] = "This mobile already exist.";
                return View();
            } 
            if (duplicateLoginStaff != null)
            {
                TempData["ErrorMessage"] = "This login already exist.";
                return View();
            }
            _staffRepository.UpdateStaff(staffModel);
            TempData["SuccessMessage"] = "Staff has been updated.";
            return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, could not update staff. Error: {error.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult DeleteStaff(StaffModel staffModel)
    {
        try
        {
            bool deleted = _staffRepository.DeleteStaff(staffModel.StaffId);
            if (deleted)
            {
                TempData["SuccessMessage"] = "Staff has been deleted.";
                return RedirectToAction("Index");
            }

            return View(staffModel);
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, could not delete staff. Error: {error.Message}";
            return RedirectToAction("Index");
        }
    }
}