using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Repositories.Interfaces;

public interface IStaffRepository
{
    List<StaffModel> GetAllStaff();
    StaffModel RegisterStaff(StaffModel staffModel);
    StaffModel GetByLogin(string login);
    StaffModel GetStaffById(int id);
    StaffModel UpdateStaff(StaffWithoutPwdModel staffModel);
    bool DeleteStaff(int staff);
}