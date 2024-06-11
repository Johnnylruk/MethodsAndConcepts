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
}