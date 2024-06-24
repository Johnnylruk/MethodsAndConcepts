using Lealthy_Hospital_Application_System.Data;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        return _lhasdb.Appointments
            .Include(x => x.Patient)
            .ToList();
    }

    public List<int> GetAllBookedStaff()
    {
        return _lhasdb.Appointments.Select(x => x.StaffId).ToList();
    }
    
    public AppointmentModel GetAppointmentById(int id)
    {
        return _lhasdb.Appointments.FirstOrDefault(x => x.AppointmentId == id);
    }

    public bool CreateAppointment(AppointmentModel appointmentModel)
    {
        var appointments = _lhasdb.Appointments
            .Where(x => x.Date == appointmentModel.Date && x.StaffName == appointmentModel.StaffName); 
            

        if (appointments.IsNullOrEmpty())
        {
            _lhasdb.Appointments.Add(appointmentModel);
            _lhasdb.SaveChanges();
            return true;
        }

        return false;
    }

    public bool UpdateAppointment(AppointmentModel appointmentModel)
    {
        AppointmentModel appointmentDB = GetAppointmentById(appointmentModel.AppointmentId);
        if (appointmentDB == null) throw new Exception("Could not update appointment");
        
        var appointments = _lhasdb.Appointments
            .Where(x => x.Date == appointmentModel.Date && x.StaffName == appointmentModel.StaffName);
        if (appointments.IsNullOrEmpty())
        {
            appointmentDB.Name = appointmentModel.Name;
            appointmentDB.Date = appointmentModel.Date;
            appointmentDB.StaffId = appointmentModel.StaffId;
            appointmentDB.PatientId = appointmentModel.PatientId;
            appointmentDB.StaffName = appointmentModel.StaffName;
            appointmentDB.PatientName = appointmentModel.PatientName;
        
            _lhasdb.Appointments.Update(appointmentDB);
            _lhasdb.SaveChanges();
            return true;
        }

        return false;
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