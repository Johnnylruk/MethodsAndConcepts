using Lealthy_Hospital_Application_System.Filters;
using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

[UserLoggedPage]
[DoctorPage]
public class DiagnosisController : Controller
{
    private readonly IDiagnosisRepository _diagnosisRepository;
    private readonly IStaffSession _staffSession;
    private readonly IPatientRepository _patientRepository;

    public DiagnosisController(IDiagnosisRepository _diagnosisRepository, IAppointmentRepository _appointmentRepository, 
                                IStaffSession _staffSession, IPatientRepository _patientRepository)
    {
        this._diagnosisRepository = _diagnosisRepository;
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

    public IActionResult UpdateDiagnosis(int id)
    {
        DiagnosisModel diagnosisModel = _diagnosisRepository.GetDiagnosisById(id);
        return View(diagnosisModel);
    }
    
    public IActionResult DeleteDiagnosis(int id)
    {
        DiagnosisModel diagnosisModel = _diagnosisRepository.GetDiagnosisById(id);
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

    [HttpPost]
    public IActionResult UpdateDiagnosis(DiagnosisModel diagnosisModel)
    {
        try
        {
            PatientModel getPatientModel = _patientRepository.GetPatientById(diagnosisModel.PatientId);
            diagnosisModel.Patient = getPatientModel;
            StaffModel getStaffModel = _staffSession.GetLoginSession();
            diagnosisModel.Staff = getStaffModel;
            diagnosisModel.Date = DateTime.Now;
            
            _diagnosisRepository.UpdateDiagnosis(diagnosisModel);
            TempData["SuccessMessage"] = "Diagnosis has been successful updated";
            return RedirectToAction("Index");    

        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, cannot update Diagnosis. Error{error.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult DeleteDiagnosis(DiagnosisModel diagnosisModel)
    {
        try
        {
            bool deleted = _diagnosisRepository.DeleteDiagnosis(diagnosisModel.DiagnosisId);
            if (deleted)
            {
                TempData["SuccessMessage"] = "Diagnosis has been successful deleted";
                return RedirectToAction("Index");  
            }
            return View(diagnosisModel);
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, cannot delete Diagnosis. Error{error.Message}";
            return RedirectToAction("Index");
        }
    }
}