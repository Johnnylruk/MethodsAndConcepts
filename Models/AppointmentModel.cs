using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lealthy_Hospital_Application_System.Models;

public class AppointmentModel
{
    [Key]
    public int AppointmentId { get; set;}
    [Required]
    public string Name { get; set;}
    [Required]
    public DateTime Date { get; set;}
    public int StaffId { get; set; }
    public virtual StaffModel Staffs { get; set; }
    public int PatientId { get; set; }
    public virtual PatientModel Patient { get; set; }
}