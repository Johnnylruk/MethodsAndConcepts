using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Helper;

public interface IStaffSession
{
    void CreateLoginSession(StaffModel staffModel);
    void RemoveLoginSession();
    StaffModel GetLoginSession();
    
}