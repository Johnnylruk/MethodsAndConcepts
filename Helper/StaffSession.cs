using Newtonsoft.Json;

using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Helper;

public class StaffSession : IStaffSession
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StaffSession(IHttpContextAccessor _httpContextAccessor)
    {
        this._httpContextAccessor = _httpContextAccessor;
    }
    public void CreateLoginSession(StaffModel staffModel)
    {
       string staffValue = JsonConvert.SerializeObject(staffModel);
       _httpContextAccessor.HttpContext.Session.SetString("staffLoggedSession", staffValue);
       
    }
    public void RemoveLoginSession()
    {
        _httpContextAccessor.HttpContext.Session.Remove("staffLoggedSession");
    }

    public StaffModel GetLoginSession()
    {
        string userSession = _httpContextAccessor.HttpContext.Session.GetString("staffLoggedSession");
        if (string.IsNullOrEmpty(userSession)) return null;
            
        return JsonConvert.DeserializeObject<StaffModel>(userSession);
    }
}