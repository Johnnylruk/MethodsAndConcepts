using Lealthy_Hospital_Application_System.Data;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;

namespace Lealthy_Hospital_Application_System.Repositories;

public class LabTestsRepository : ILabTestsRepository
{
    private readonly LHASDB _lhasdb;

    public LabTestsRepository(LHASDB _lhasdb)
    {
        this._lhasdb = _lhasdb;
    }
    
    public List<LabTestsModel> GetAllLabTests()
    {
        return _lhasdb.LabTests.ToList();
    }

    public LabTestsModel GetLabTestById(int id)
    {
        return _lhasdb.LabTests.FirstOrDefault(x => x.LabTestId == id);
    }

    public LabTestsModel RegisterLabTest(LabTestsModel labTestsModel)
    {
        _lhasdb.LabTests.Add(labTestsModel);
        _lhasdb.SaveChanges();
        return labTestsModel;
    }

    public LabTestsModel UpdateLabTest(LabTestsModel labTestsModel)
    {
        LabTestsModel LabTestDB = GetLabTestById(labTestsModel.LabTestId);
        if (LabTestDB == null) throw new Exception("Could not update Lab Test");

        LabTestDB.TypeOfTests = labTestsModel.TypeOfTests;
        LabTestDB.Description = labTestsModel.Description;
        LabTestDB.Date = labTestsModel.Date;
        LabTestDB.Result = labTestsModel.Result;
        
        _lhasdb.LabTests.Update(LabTestDB);
        _lhasdb.SaveChanges();
        return LabTestDB;

    }

    public bool RemoveLabTest(int labTestsModel)
    {
        LabTestsModel LabTestDB = GetLabTestById(labTestsModel);
        if (LabTestDB == null) throw new Exception("Could not delete Lab Test");
        _lhasdb.LabTests.Remove(LabTestDB);
        _lhasdb.SaveChanges();
        return true;

    }
}