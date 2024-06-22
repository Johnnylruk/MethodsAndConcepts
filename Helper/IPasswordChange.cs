using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Helper;

public interface IPasswordChange
{
    StaffModel ChangePassword(ChangePasswordModel changePasswordModel);
}