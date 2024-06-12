using Lealthy_Hospital_Application_System.Enum;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

public class AppointmentController : Controller
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IPatientRepository _patientRepository;

    public AppointmentController(IAppointmentRepository _appointmentRepository, IStaffRepository _staffRepository,
                                 IPatientRepository _patientRepository)
    {
        this._appointmentRepository = _appointmentRepository;
        this._staffRepository = _staffRepository;
        this._patientRepository = _patientRepository;
    }
    
    
    public IActionResult Index()
    {
        List<AppointmentModel> ListAppointments = _appointmentRepository.GetAllAppointments();
       
        return View(ListAppointments);
    }

    public IActionResult CreateAppointment()
    {
        List<StaffModel> GetDoctors = _staffRepository.GetAllStaff()
            .Where(x => x.Access == RoleAccessEnum.Doctor).ToList();
        List<PatientModel> GetPatients = _patientRepository.GetAllPatients();
        
        ViewBag.Doctors = GetDoctors;
        ViewBag.Patients = GetPatients;

        return View();
    }

    public IActionResult UpdateAppointment(int id)
    {
        AppointmentModel getAppointmentModel = _appointmentRepository.GetAppointmentById(id);
        List<StaffModel> GetDoctors = _staffRepository.GetAllStaff()
            .Where(x => x.Access == RoleAccessEnum.Doctor).ToList();
        List<PatientModel> GetPatients = _patientRepository.GetAllPatients();
        
        ViewBag.Doctors = GetDoctors;
        ViewBag.Patients = GetPatients;
        return View(getAppointmentModel);
    }

    [HttpPost]
    public IActionResult CreateAppointment(AppointmentModel appointmentModel)
    {
        try
        {
            var StaffId = _staffRepository.GetStaffById(appointmentModel.StaffId);
            string StaffName = StaffId.Name;
            appointmentModel.StaffName = StaffName;
            var PatientId = _patientRepository.GetPatientById(appointmentModel.PatientId);
            string PatientName = PatientId.Name;
            appointmentModel.PatientName = PatientName;
            
            bool appointmentCreated =  _appointmentRepository.CreateAppointment(appointmentModel);
            
            if (!appointmentCreated)
            {
                TempData["ErrorMessage"] = "Already exist an appointment for this time.";
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
                _appointmentRepository.UpdateAppointment(appointmentModel);
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
    
}