using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Repositories.Interfaces;

public interface IAppointmentRepository
{
    List<AppointmentModel> GetAllAppointments();
    List<int> GetAllBookedStaff();
    AppointmentModel GetAppointmentById(int id);
    bool CreateAppointment(AppointmentModel appointmentModel);
    AppointmentModel UpdateAppointment(AppointmentModel appointmentModel);
    bool DeleteAppointment(int appointmentId);
}