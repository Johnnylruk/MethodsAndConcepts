using System.ComponentModel.DataAnnotations;

namespace Lealthy_Hospital_Application_System.Models;

public class ResetPasswordModel
{
    
    [Required(ErrorMessage = "Type your login.")]
    public string Login { get; set;}
    [Required(ErrorMessage = "Type your email.")]
    [EmailAddress]
    public string Email { get; set; }
}