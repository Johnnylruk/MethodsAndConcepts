using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lealthy_Hospital_Application_System.Models;

public class AppointmentModel
{
   [Key]
    public int AppointmentId { get; set;}
    public string Name { get; set;}
    public DateTime Date { get; set;}
    public string StaffName { get; set; }
    public int StaffId { get; set; }

    public virtual StaffModel Staff { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; }
    public virtual PatientModel Patient { get; set; }
}