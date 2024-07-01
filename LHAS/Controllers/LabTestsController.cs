using Lealthy_Hospital_Application_System.Enum;
using Lealthy_Hospital_Application_System.Filters;
using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

[DoctorPage]
[UserLoggedPage]
public class LabTestsController : Controller
{
    private readonly ILabTestsRepository _labTestsRepository;
    private readonly IStaffSession _staffSession;
    private readonly IPatientRepository _patientRepository;

    public LabTestsController(ILabTestsRepository _labTestsRepository, IStaffSession _staffSession,
        IPatientRepository _patientRepository)
    {
        this._labTestsRepository = _labTestsRepository;
        this._patientRepository = _patientRepository;
        this._staffSession = _staffSession;
    }

    public IActionResult Index()
    {
        var Staff = _staffSession.GetLoginSession();
        if (Staff.StaffType == RoleAccessEnum.Doctor)
        {
            var labTests = _labTestsRepository.GetAllLabTests()
                .Where(ap => ap.StaffId == Staff.StaffId)
                .ToList();

            if (labTests.Count != 0)
            {
                ViewBag.Doctor = "Doctor";
                ViewBag.Staff = Staff.Name;
                ViewBag.Access = Staff.StaffType;
                return View(labTests);
            }
            TempData["ErrorMessage"] = "You do not have any Laboratory Test";
            return RedirectToAction("Index", "Home");
        }

        List<LabTestsModel> ListAllLabTests = _labTestsRepository.GetAllLabTests();
        ViewBag.Staff = Staff.Name;
        ViewBag.Access = Staff.StaffType;
        return View(ListAllLabTests);
    }

    public IActionResult CreateLabTest(int id)
    {
        var staff = _staffSession.GetLoginSession();
        var patient = _patientRepository.GetPatientById(id);
        ViewBag.Staff = staff.Name;
        ViewBag.Access = staff.StaffType;

        var labTest = new LabTestsModel()
        {
            StaffId = staff.StaffId,
            StaffName = staff.Name,
            PatientId = patient.PatientId,
            PatientName = patient.Name,
            Date = DateTime.Now
        };

        ViewBag.PatientName = labTest.PatientName;
        ViewBag.StaffName = labTest.StaffName;

        return View(labTest);
    }

    public IActionResult UpdateLabTest(int id)
    {

        try
        {
            LabTestsModel labTestsModel = _labTestsRepository.GetLabTestById(id);
            var Staff = _staffSession.GetLoginSession();

            if (labTestsModel != null)
            {
                ViewBag.Staff = Staff.Name;
                ViewBag.Access = Staff.StaffType;
                return View(labTestsModel);
            }
            ViewBag.Staff = Staff.Name;
            ViewBag.Access = Staff.StaffType;
            return RedirectToAction("Index", "Home");
        }
        catch (Exception error)
        {
            return RedirectToAction("Index", "Restrict");
        }
    }

    public IActionResult DeleteLabTest(int id)
    {
        try
        {
            LabTestsModel labTestsModel = _labTestsRepository.GetLabTestById(id);
            var Staff = _staffSession.GetLoginSession();

            if (labTestsModel != null)
            {
                ViewBag.Staff = Staff.Name;
                ViewBag.Access = Staff.StaffType;
                return View(labTestsModel);
            }
            ViewBag.Staff = Staff.Name;
            ViewBag.Access = Staff.StaffType;
            return RedirectToAction("Index", "Home");
        }
        catch (Exception error)
        {
            return RedirectToAction("Index", "Restrict");
        }
    }

    [HttpPost]
    public IActionResult CreateLabTest(LabTestsModel labTestsModel)
    {
        try
        {
            _labTestsRepository.RegisterLabTest(labTestsModel);
            TempData["SuccessMessage"] = "Laboratory Test has been requested.";
            return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Laboratory Test could not been requested.{error.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult UpdateLabTest(LabTestsModel labTestsModel)
    {
        try
        {
            PatientModel getPatientModel = _patientRepository.GetPatientById(labTestsModel.PatientId);
            labTestsModel.Patient = getPatientModel;
            StaffModel getStaffModel = _staffSession.GetLoginSession();
            labTestsModel.Staff = getStaffModel;
            
            _labTestsRepository.UpdateLabTest(labTestsModel);
            TempData["SuccessMessage"] = "Laboratory Test has been successful updated.";
            return RedirectToAction("Index");    

        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, cannot update Laboratory Test. Error{error.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult DeleteLabTest(LabTestsModel labTestsModel)
    {
        try
        {
            bool deleted = _labTestsRepository.RemoveLabTest(labTestsModel.LabTestId);
            if (deleted)
            {
                TempData["SuccessMessage"] = "Laboratory Test has been successful deleted.";
                return RedirectToAction("Index");
            }

            return View(labTestsModel);
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, cannot delete Laboratory Test. Error{error.Message}";
            return RedirectToAction("Index");
        }
    }
}