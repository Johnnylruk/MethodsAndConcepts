using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Repositories.Interfaces;

public interface IDiagnosisRepository
{
    List<DiagnosisModel> GetAllDiagnosis();
    DiagnosisModel GetDiagnosisById(int DiagnosisId);
    DiagnosisModel CreateDiagnosis(DiagnosisModel diagnosisModel);
    DiagnosisModel UpdateDiagnosis(DiagnosisModel diagnosisModel);
    bool DeleteDiagnosis(int id);
}