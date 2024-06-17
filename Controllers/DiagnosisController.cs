using Lealthy_Hospital_Application_System.Filters;
using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;


public class DiagnosisController : Controller
{
    private readonly IDiagnosisRepository _diagnosisRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IStaffSession _staffSession;
    private readonly IPatientRepository _patientRepository;

    public DiagnosisController(IDiagnosisRepository _diagnosisRepository, IAppointmentRepository _appointmentRepository, 
                                IStaffSession _staffSession, IPatientRepository _patientRepository)
    {
        this._diagnosisRepository = _diagnosisRepository;
        this._appointmentRepository = _appointmentRepository;
        this._staffSession = _staffSession;
        this._patientRepository = _patientRepository;
    }
    
    public IActionResult Index()
    {
        List<DiagnosisModel> ListAll = _diagnosisRepository.GetAllDiagnosis();
        return View(ListAll);
    }

    public IActionResult CreateDiagnosis(int id)
    {
        var staffSession = _staffSession.GetLoginSession();
        var patient = _patientRepository.GetPatientById(id);
        
        var diagnosisModel = new DiagnosisModel()
        {
            PatientId = patient.PatientId,
            StaffId = staffSession.StaffId,
            PatientName = patient.Name,
            StaffName = staffSession.Name,
            Date = DateTime.Now
        };

        ViewBag.PatientName = diagnosisModel.PatientName;
        ViewBag.StaffName = diagnosisModel.StaffName;
        
        return View(diagnosisModel);
    }

    [HttpPost]
    public IActionResult CreateDiagnosis(DiagnosisModel diagnosisModel)
    {
        try
        {
            _diagnosisRepository.CreateDiagnosis(diagnosisModel);
            TempData["SuccessMessage"] = "Diagnosis has been successful created";
            return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, cannot create Diagnosis. Error{error.Message}";
            return RedirectToAction("Index");
        }
    }
    
}