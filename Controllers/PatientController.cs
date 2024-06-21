using Lealthy_Hospital_Application_System.Enum;
using Lealthy_Hospital_Application_System.Filters;
using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

[UserLoggedPage]
[NotNursePage]

public class PatientController : Controller
{
    private readonly IPatientRepository _patientRepository;
    private readonly IStaffSession _staffSession;
    private readonly IAppointmentRepository _appointmentRepository;

    public PatientController(IPatientRepository _patientRepository, IStaffSession _staffSession,
                            IAppointmentRepository _appointmentRepository)
    {
        this._patientRepository = _patientRepository;
        this._staffSession = _staffSession;
        this._appointmentRepository = _appointmentRepository;
        
    }

    public IActionResult Index()
    {
        var staffSession = _staffSession.GetLoginSession();
        if (staffSession.Access == RoleAccessEnum.Doctor)
        {
            var appointments = _appointmentRepository.GetAllAppointments()
                .Where(ap => ap.StaffId == staffSession.StaffId)
                .ToList(); 
    
            var distinctPatients = appointments
                .Where(ap => ap.Patient != null) 
                .Select(ap => ap.Patient)
                .GroupBy(p => p.PatientId) 
                .Select(g => g.First()) 
                .ToList();
            
            if (distinctPatients.Count != 0)
            {
                ViewBag.Doctor = "Doctor";
                return View(distinctPatients);    
            }
            TempData["ErrorMessage"] = "You do not have any Patient";
            return RedirectToAction("Index", "Home");
        }
        else
        {
            List<PatientModel> ListPatients = _patientRepository.GetAllPatients(); 
            var Staff = _staffSession.GetLoginSession();
            ViewBag.Staff = Staff.Name;
            ViewBag.Access = Staff.Access;
            return View(ListPatients);
        }
       
    }

    public IActionResult CreatePatient()
    {
        var Staff = _staffSession.GetLoginSession();
        ViewBag.Staff = Staff.Name;
        ViewBag.Access = Staff.Access;
        return View();
    }

    public IActionResult UpdatePatient(int PatientId)
    {
        PatientModel patientModel = _patientRepository.GetPatientById(PatientId);
        var Staff = _staffSession.GetLoginSession();
        ViewBag.Staff = Staff.Name;
        ViewBag.Access = Staff.Access;
        return View(patientModel);
    }
    public IActionResult DeletePatient(int PatientId)
    {
        PatientModel patientModel = _patientRepository.GetPatientById(PatientId);
        var Staff = _staffSession.GetLoginSession();
        ViewBag.Staff = Staff.Name;
        ViewBag.Access = Staff.Access;
        return View(patientModel);
    }

    [HttpPost]
    public IActionResult CreatePatient(PatientModel patientModel)
    {
        try
        {

            PatientModel PatientDB = _patientRepository.GetAllPatients()
                .FirstOrDefault(x => x.Email == patientModel.Email ||
                                     x.Mobile == patientModel.Mobile);

            if (PatientDB != null)
            {
                if (PatientDB.Email == patientModel.Email)
                {
                    TempData["ErrorMessage"] = "This email already exist.";
                }
                if (PatientDB.Mobile == patientModel.Mobile)
                {
                    TempData["ErrorMessage"] = "This mobile already exist.";
                }
            }    
                _patientRepository.RegisterPatient(patientModel);
                TempData["SuccessMessage"] = "Patient has been successful created.";
                return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, problem when trying to create patient. Error {error.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult UpdatePatient(PatientModel patientModel)
    {
        try
        {
                _patientRepository.UpdatePatient(patientModel);
                TempData["SuccessMessage"] = "Patient has been successful updated.";
                return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, problem when trying to update patient. Error {error.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]

    public IActionResult DeletePatient(PatientModel patientModel)
    {
        try
        {
                bool PatientDeleted = _patientRepository.DeletePatient(patientModel.PatientId);
           
                if (PatientDeleted)
                {
                    TempData["SuccessMessage"] = "Patient has been successful updated.";
                    return RedirectToAction("Index");
                }    

            return View(patientModel);
        }
        
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, problem when trying to delete patient. Error {error.Message}";
            return RedirectToAction("Index");
        }
    }
}