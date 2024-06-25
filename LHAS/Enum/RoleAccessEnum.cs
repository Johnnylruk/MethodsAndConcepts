using System.ComponentModel.DataAnnotations;

namespace Lealthy_Hospital_Application_System.Enum;

public enum RoleAccessEnum
{
    [Display(Name ="Administrator")]
    Administrator = 1,
    [Display(Name ="Receptionist")]
    Receptionist = 2,
    [Display(Name ="Doctor")]
    Doctor = 3, 
    [Display(Name ="Nurse")]
    Nurse = 4
}