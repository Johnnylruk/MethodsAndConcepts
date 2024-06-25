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
    public string Name { get; set; }
    [EmailAddress]
    public string Email { get; set;}
    [Phone(ErrorMessage = "Contact number it's not valid.")]
    public string Mobile { get; set;}
    public string Address { get; set;}
    public DateTime DateOfBirth { get; set;}

    public ICollection<AppointmentModel> Appointments { get; set; } 
    public ICollection<DiagnosisModel> Diagnosis { get; set; } 
    public ICollection<LabTestsModel> LabTests { get; set; } 
}