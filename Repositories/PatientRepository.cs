using Lealthy_Hospital_Application_System.Data;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;

namespace Lealthy_Hospital_Application_System.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly LHASDB _lhasdb;

    public PatientRepository(LHASDB _lhasdb)
    {
        this._lhasdb = _lhasdb;
    }
    public List<PatientModel> GetAllPatients()
    {
        return _lhasdb.Patients.ToList();
    }

    public PatientModel GetPatientById(int patientId)
    {
        return _lhasdb.Patients.FirstOrDefault(x => x.PatientId == patientId);
    }

    public PatientModel RegisterPatient(PatientModel patientModel)
    {
        _lhasdb.Patients.Add(patientModel);
        _lhasdb.SaveChanges();
        return patientModel;
    }

    public PatientModel UpdatePatient(PatientModel patientModel)
    {
        PatientModel PatientDB = GetPatientById(patientModel.PatientId);
        if (PatientDB == null) throw new Exception("Could not update patient.");

        PatientDB.Name = patientModel.Name;
        PatientDB.Email = patientModel.Email;
        PatientDB.Mobile = patientModel.Mobile;
        PatientDB.Address = patientModel.Address;
        PatientDB.DateOfBirth = patientModel.DateOfBirth;

        _lhasdb.Patients.Update(PatientDB);
        _lhasdb.SaveChanges();
        return PatientDB;

    }

    public bool DeletePatient(int patientId)
    {
        PatientModel PatientDB = GetPatientById(patientId);
        if (PatientDB == null) throw new Exception("Could not update patient.");

        _lhasdb.Patients.Remove(PatientDB);
        _lhasdb.SaveChanges();
        return true;
    }
}