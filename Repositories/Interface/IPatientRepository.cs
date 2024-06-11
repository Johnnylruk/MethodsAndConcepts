using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Repositories.Interfaces;

public interface IPatientRepository
{
    List<PatientModel> GetAllPatients();
    PatientModel GetPatientById(int patientId);
    PatientModel RegisterPatient(PatientModel patientModel);
    PatientModel UpdatePatient(PatientModel patientModel);
    bool DeletePatient(int patient);
}