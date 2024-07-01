using Lealthy_Hospital_Application_System.Enum;
using Lealthy_Hospital_Application_System.Filters;
using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

[ReceptionistPage]
[UserLoggedPage]
public class AppointmentController : Controller
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IStaffSession _staffSession;

    public AppointmentController(IAppointmentRepository _appointmentRepository, IStaffRepository _staffRepository,
                                 IPatientRepository _patientRepository, IStaffSession _staffSession)
    {
        this._appointmentRepository = _appointmentRepository;
        this._staffRepository = _staffRepository;
        this._patientRepository = _patientRepository;
        this._staffSession = _staffSession;
    }
    
    
    public IActionResult Index()
    {
        List<AppointmentModel> ListAppointments = _appointmentRepository.GetAllAppointments();
        var Staff = _staffSession.GetLoginSession();
        ViewBag.Staff = Staff.Name;
        ViewBag.Access = Staff.StaffType;
        return View(ListAppointments);
    }

    public IActionResult CreateAppointment()
    {
        List<StaffModel> GetDoctors = _staffRepository.GetAllStaff()
            .Where(x => x.StaffType == RoleAccessEnum.Doctor).ToList();
        List<PatientModel> GetPatients = _patientRepository.GetAllPatients();
        
        var Staff = _staffSession.GetLoginSession();
        ViewBag.Staff = Staff.Name;
        ViewBag.Access = Staff.StaffType;
        ViewBag.Doctors = GetDoctors;
        ViewBag.Patients = GetPatients;

        return View();
    }

        public IActionResult UpdateAppointment(int id)
        {
            AppointmentModel getAppointmentModel = _appointmentRepository.GetAppointmentById(id);
            List<StaffModel> GetDoctors = _staffRepository.GetAllStaff()
                .Where(x => x.StaffType == RoleAccessEnum.Doctor).ToList();
            List<PatientModel> GetPatients = _patientRepository.GetAllPatients();
            var Staff = _staffSession.GetLoginSession();
            ViewBag.Staff = Staff.Name;
            ViewBag.Access = Staff.StaffType;
            
            ViewBag.Doctors = GetDoctors;
            ViewBag.Patients = GetPatients;
            return View(getAppointmentModel);
        }

    public IActionResult DeleteAppointment(int id)
    {
        AppointmentModel appointmentModel = _appointmentRepository.GetAppointmentById(id);
        var Staff = _staffSession.GetLoginSession();
        ViewBag.Staff = Staff.Name;
        ViewBag.Access = Staff.StaffType;
        return View(appointmentModel);
    }
    
    [HttpPost]
    public IActionResult CreateAppointment(AppointmentModel appointmentModel)
    {
        try
        {
            var StaffId = _staffRepository.GetStaffById(appointmentModel.StaffId);
            var PatientId = _patientRepository.GetPatientById(appointmentModel.PatientId);
            appointmentModel.StaffName = StaffId.Name;
            appointmentModel.PatientName = PatientId.Name;
            
            bool appointmentCreated =  _appointmentRepository.CreateAppointment(appointmentModel);
            
            if (!appointmentCreated)
            {
                TempData["ErrorMessage"] = $"{appointmentModel.StaffName} already have an appointment for this time.";
                return RedirectToAction("Index");
            }
            
            TempData["SuccessMessage"] = "Appointment has been successful created.";
            return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, problem when trying to create appointment. Error {error.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult UpdateAppointment(AppointmentModel appointmentModel)
    {
        try
        {
            StaffModel StaffId = _staffRepository.GetStaffById(appointmentModel.StaffId);
            PatientModel PatientId = _patientRepository.GetPatientById(appointmentModel.PatientId);
            appointmentModel.StaffName = StaffId.Name;
            appointmentModel.PatientName = PatientId.Name;
            
            if (appointmentModel != null)
            {
                bool appointmentUpdated =  _appointmentRepository.UpdateAppointment(appointmentModel);
                if (!appointmentUpdated)
                {
                    TempData["ErrorMessage"] = $"{appointmentModel.StaffName} already have an appointment for this time.";
                    return RedirectToAction("Index");
                }
 
                TempData["SuccessMessage"] = "Appointment has been successful updated.";
                return RedirectToAction("Index");
            }

            return View(appointmentModel);
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, problem when trying to update appointment. Error {error.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult DeleteAppointment(AppointmentModel appointmentModel)
    {
        try
        {
            bool deleted = _appointmentRepository.DeleteAppointment(appointmentModel.AppointmentId);
            if (deleted)
            {
                TempData["SuccessMessage"] = "Appointment has been successful deleted.";
                return RedirectToAction("Index");
            }
            return View(appointmentModel);
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, problem when trying to delete appointment. Error {error.Message}";
            return RedirectToAction("Index");
        }
       

    }
    
}