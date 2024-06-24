using Lealthy_Hospital_Application_System.Data;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;

namespace Lealthy_Hospital_Application_System.Helper;

public class PasswordChange : IPasswordChange
{
    private readonly LHASDB _lhasdb;
    private readonly IStaffRepository _staffRepository;

    public PasswordChange(LHASDB _lhasdb, IStaffRepository _staffRepository)
    {
        this._lhasdb = _lhasdb;
        this._staffRepository = _staffRepository;
    }
    public StaffModel ChangePassword(ChangePasswordModel changePasswordModel)
    {
        StaffModel StaffDB = _staffRepository.GetStaffById(changePasswordModel.StaffId);

        if (StaffDB == null) throw new Exception("Error when trying to update password: User not found");
        
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(changePasswordModel.OldPassword, StaffDB.Password);
        if (!isValidPassword) 
            throw new Exception("Old Password is incorrect");

        bool isNewPassword = BCrypt.Net.BCrypt.Verify(changePasswordModel.NewPassword, StaffDB.Password);

        if (isNewPassword)
            throw new Exception("Password cannot be equal to old password, please insert a different password");

        StaffDB.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordModel.NewPassword);
        _lhasdb.Staffs.Update(StaffDB);
        _lhasdb.SaveChanges();
        return StaffDB;
    }

    public StaffModel ResetPassword(StaffModel staffModel)
    {
        StaffModel StaffDB = _staffRepository.GetStaffById(staffModel.StaffId);

        if (StaffDB == null) throw new Exception("Error trying to send password send password");
        StaffDB.Password = staffModel.Password;
        _lhasdb.Staffs.Update(StaffDB);
        _lhasdb.SaveChanges();
        return StaffDB;
    }

    public StaffModel GetStaffByLoginAndEmail(string login, string email)
    {
        return _lhasdb.Staffs.FirstOrDefault(
            x => x.Email.ToUpper() == email.ToUpper() && x.Login.ToUpper() == login.ToUpper());
    }

    public string GeneratePassword()
    {
            return Guid.NewGuid().ToString().Substring(0, 8);
    }
}