using Lealthy_Hospital_Application_System.Data;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;

namespace Lealthy_Hospital_Application_System.Repositories;

public class DiagnosisRepository : IDiagnosisRepository
{
    private readonly LHASDB _lhasdb;

    public DiagnosisRepository(LHASDB _lhasdb)
    {
        this._lhasdb = _lhasdb;
    }
    public List<DiagnosisModel> GetAllDiagnosis()
    {
        return _lhasdb.Diagnosis.ToList();
    }

    public DiagnosisModel GetDiagnosisById(int DiagnosisId)
    {
        return _lhasdb.Diagnosis.FirstOrDefault(x => x.DiagnosisId == DiagnosisId);
    }

    public DiagnosisModel CreateDiagnosis(DiagnosisModel diagnosisModel)
    {
        _lhasdb.Diagnosis.Add(diagnosisModel);
        _lhasdb.SaveChanges();
        return diagnosisModel;
    }

    public DiagnosisModel UpdateDiagnosis(DiagnosisModel diagnosisModel)
    {
        DiagnosisModel DiagnosisDB = GetDiagnosisById(diagnosisModel.DiagnosisId);
        if (DiagnosisDB == null) throw new Exception("Could not update Diagnosis");

        DiagnosisDB.TypeOfDiagnosis = diagnosisModel.TypeOfDiagnosis;
        DiagnosisDB.Treatment = diagnosisModel.Treatment;
        DiagnosisDB.StaffId = diagnosisModel.StaffId;
        DiagnosisDB.StaffName = diagnosisModel.StaffName;

        _lhasdb.Diagnosis.Update(DiagnosisDB);
        _lhasdb.SaveChanges();
        return DiagnosisDB;
    }

    public bool DeleteDiagnosis(int diagnosisId)
    {
        DiagnosisModel DiagnosisDB = GetDiagnosisById(diagnosisId);
        if (DiagnosisDB == null) throw new Exception("Could not delete Diagnosis.");
        _lhasdb.Diagnosis.Remove(DiagnosisDB);
        _lhasdb.SaveChanges();
        return true;
    }
}