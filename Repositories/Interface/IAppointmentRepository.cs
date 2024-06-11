using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Repositories.Interfaces;

public interface IAppointmentRepository
{
    List<AppointmentModel> GetAllAppointments();
    AppointmentModel GetAppointmentById(int id);
    AppointmentModel CreateAppointment(AppointmentModel appointmentModel);
    AppointmentModel UpdateAppointment(AppointmentModel appointmentModel);
    bool DeleteAppointment(int appointmentId);
}