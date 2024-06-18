using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

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
        List<LabTestsModel> ListAllLabTests = _labTestsRepository.GetAllLabTests();
        return View(ListAllLabTests);
    }

    public IActionResult CreateLabTest(int id)
    {
        var staff = _staffSession.GetLoginSession();
        var patient = _patientRepository.GetPatientById(id);

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
}