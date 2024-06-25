using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Repositories.Interfaces;

public interface ILabTestsRepository
{
    List<LabTestsModel> GetAllLabTests();
    LabTestsModel GetLabTestById(int id);
    LabTestsModel RegisterLabTest(LabTestsModel labTestsModel);
    LabTestsModel UpdateLabTest(LabTestsModel labTestsModel);
    bool RemoveLabTest(int id);
}