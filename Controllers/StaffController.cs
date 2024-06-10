using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

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
        return View(staffId);
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
            if (ModelState.IsValid)
            {
                _staffRepository.RegisterStaff(staffModel);
                return RedirectToAction("Index");
            }

            return View(staffModel);
        }
        catch (Exception e)
        {
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult UpdateStaff(StaffModel staffModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _staffRepository.UpdateStaff(staffModel);
                return RedirectToAction("Index");
            }

            return View(staffModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
                return RedirectToAction("Index");
            }
        }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        return RedirectToAction("Index");
    }
}