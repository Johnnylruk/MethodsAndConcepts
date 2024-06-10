using System.ComponentModel.DataAnnotations;

namespace Lealthy_Hospital_Application_System.Models;

public class AppointmentModel
{
    public int AppointmentId { get; set;}
    [Required]
    public string Name { get; set;}
    [Required]
    public DateTime StartDate { get; set;}
    [Required]
    public DateTime EndDate { get; set;}
}