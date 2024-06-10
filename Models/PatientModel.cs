using System.ComponentModel.DataAnnotations;

namespace Lealthy_Hospital_Application_System.Models;

public class PatientModel
{
    public int PatientId { get; set;}
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set;}
    [Required]
    public string Mobile { get; set;}
    [Required]
    public string Address { get; set;}
    [Required]
    public DateTime DateOfBirth { get; set;}
}