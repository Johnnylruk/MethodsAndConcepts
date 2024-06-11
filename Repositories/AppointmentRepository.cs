using Lealthy_Hospital_Application_System.Data;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;

namespace Lealthy_Hospital_Application_System.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly LHASDB _lhasdb;

    public AppointmentRepository(LHASDB _lhasdb)
    {
        this._lhasdb = _lhasdb;
    }
    
    public List<AppointmentModel> GetAllAppointments()
    {
        return _lhasdb.Appointments.ToList();
    }

    public AppointmentModel GetAppointmentById(int id)
    {
        return _lhasdb.Appointments.FirstOrDefault(x => x.AppointmentId == id);
    }

    public AppointmentModel CreateAppointment(AppointmentModel appointmentModel)
    {
        _lhasdb.Appointments.Add(appointmentModel);
        _lhasdb.SaveChanges();
        return (appointmentModel);
    }

    public AppointmentModel UpdateAppointment(AppointmentModel appointmentModel)
    {
        AppointmentModel appointmentDB = GetAppointmentById(appointmentModel.AppointmentId);
        if (appointmentDB == null) throw new Exception("Could not update appointment");

        appointmentDB.Name = appointmentModel.Name;
        appointmentDB.Date = appointmentModel.Date;
        appointmentDB.StaffId = appointmentModel.StaffId;
        appointmentDB.PatientId = appointmentModel.PatientId;

        _lhasdb.Appointments.Update(appointmentDB);
        _lhasdb.SaveChanges();
        return appointmentDB;
    }

    public bool DeleteAppointment(int appointmentId)
    {
        AppointmentModel appointmentDB = GetAppointmentById(appointmentId);
        if (appointmentDB == null) throw new Exception("Could not delete appointment");

        _lhasdb.Appointments.Remove(appointmentDB);
        _lhasdb.SaveChanges();
        return true;
    }
}