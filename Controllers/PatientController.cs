using Lealthy_Hospital_Application_System.Enum;
using Lealthy_Hospital_Application_System.Filters;
using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

[UserLoggedPage]

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
    
            return View(distinctPatients);
        }
        else
        {
            List<PatientModel> ListPatients = _patientRepository.GetAllPatients(); 
            return View(ListPatients);
        }
       
    }

    public IActionResult CreatePatient()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreatePatient(PatientModel patientModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _patientRepository.RegisterPatient(patientModel);
                TempData["SuccessMessage"] = "Patient has been successful created.";
                return RedirectToAction("Index");
            }

            return View(patientModel);
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, problem when trying to create patient. Error {error.Message}";
            return RedirectToAction("Index");
        }
    }
}