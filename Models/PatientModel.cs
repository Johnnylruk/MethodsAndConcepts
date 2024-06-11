using System.ComponentModel.DataAnnotations;

namespace Lealthy_Hospital_Application_System.Models;

public class PatientModel
{
    public PatientModel()
    {
        Appointments = new List<AppointmentModel>();
        Diagnosis = new List<DiagnosisModel>();
        LabTests = new List<LabTestsModel>();
    }
    [Key]
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

    public ICollection<AppointmentModel> Appointments { get; set; } 
    public ICollection<DiagnosisModel> Diagnosis { get; set; } 
    public ICollection<LabTestsModel> LabTests { get; set; } 
}