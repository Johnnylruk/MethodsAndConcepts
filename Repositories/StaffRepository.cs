using Lealthy_Hospital_Application_System.Data;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;

namespace Lealthy_Hospital_Application_System.Repositories;

public class StaffRepository : IStaffRepository
{
    private readonly LHASDB _lhasdb;

    public StaffRepository(LHASDB _lhasdb)
    {
        this._lhasdb = _lhasdb;
    }
    
    public List<StaffModel> GetAllStaff()
    {
        return _lhasdb.Staffs.ToList();
    }
    public StaffModel GetStaffById(int id)
    {
        return _lhasdb.Staffs.FirstOrDefault(x => x.StaffId == id);
    }
    public StaffModel GetByLogin(string login)
    {
        return _lhasdb.Staffs.FirstOrDefault(x => x.Login.ToUpper() == login.ToUpper());
    }
    
    public StaffModel RegisterStaff(StaffModel staffModel)
    {
        _lhasdb.Staffs.Add(staffModel);
        _lhasdb.SaveChanges();
        return staffModel;
    }
    public StaffModel UpdateStaff(StaffModel staffModel)
    {
        StaffModel StaffDb = GetStaffById(staffModel.StaffId);
        if (staffModel == null) throw new Exception("Error when updating candidate.");

        StaffDb.Name = staffModel.Name;
        StaffDb.Email = staffModel.Email;
        StaffDb.Mobile = staffModel.Mobile;
        StaffDb.Address = staffModel.Address;
        StaffDb.DateOfBirth = staffModel.DateOfBirth;
        StaffDb.Access = staffModel.Access;
        StaffDb.Login = staffModel.Login;
        
        _lhasdb.Staffs.Update(StaffDb);
        _lhasdb.SaveChanges();
        return StaffDb;
    }

    public bool DeleteStaff(int staffModel)
    {
        StaffModel StaffDB = GetStaffById(staffModel);
        if (StaffDB == null) throw new Exception("Error when deleting candidate.");

        _lhasdb.Staffs.Remove(StaffDB);
        _lhasdb.SaveChanges();
        return true;
    }
}