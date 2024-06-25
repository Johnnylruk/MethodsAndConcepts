using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Helper;

public interface IPasswordChange
{
    StaffModel ChangePassword(ChangePasswordModel changePasswordModel);
    StaffModel ResetPassword(StaffModel resetPasswordModel);
    StaffModel GetStaffByLoginAndEmail(string login, string email);
    string GeneratePassword();
}