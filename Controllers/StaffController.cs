using Lealthy_Hospital_Application_System.Filters;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

[OnlyAdministratorPage]

public class StaffController : Controller
{
    private readonly IStaffRepository _staffRepository;

    public StaffController(IStaffRepository _staffRepository)
    {
        this._staffRepository = _staffRepository;
    }
    
    public IActionResult Index()
    {
        List<StaffModel> ListAllStaff = _staffRepository.GetAllStaff();
        return View(ListAllStaff);    
    }

    public IActionResult CreateStaff()
    {
        return View();
    }

    public IActionResult UpdateStaff(int id)
    {
        StaffModel staffId = _staffRepository.GetStaffById(id);
        var StaffWithoutPassword = new StaffWithoutPwdModel()
        {
            StaffId = staffId.StaffId,
            Name = staffId.Name,
            Email = staffId.Email,
            Mobile = staffId.Mobile,
            Address = staffId.Address,
            DateOfBirth = staffId.DateOfBirth,
            Access = staffId.Access,
            Login = staffId.Login
        };
        return View(StaffWithoutPassword);
    }
    public IActionResult DeleteStaff(int id)
    {
        StaffModel staffId = _staffRepository.GetStaffById(id);
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
                TempData["SuccessMessage"] = "Staff has been createted.";
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