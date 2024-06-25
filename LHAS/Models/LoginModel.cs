using System.ComponentModel.DataAnnotations;

namespace Lealthy_Hospital_Application_System.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Type login")]
    public string Login { get; set; }
    [Required(ErrorMessage = "Type Password")]
    public string Password { get; set; }
}