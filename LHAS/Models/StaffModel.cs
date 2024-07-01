using System.ComponentModel.DataAnnotations;
using Lealthy_Hospital_Application_System.Enum;

namespace Lealthy_Hospital_Application_System.Models;

public class StaffModel
{

    public StaffModel()
    {
        Appointments = new List<AppointmentModel>();
        Diagnosis = new List<DiagnosisModel>();
        LabTests = new List<LabTestsModel>();
    }
    
    [Key]
    public int StaffId { get; set;}
    public string Name { get; set; }
    [EmailAddress]
    public string Email { get; set;}
    [Phone(ErrorMessage = "Contact number it's not valid.")]
    public string Mobile { get; set;}
    public string Address { get; set;}
   
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set;}
    
    [Display(Name = "Staff Role")]
    public RoleAccessEnum StaffType { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    public ICollection<AppointmentModel> Appointments { get; set; } = new List<AppointmentModel>();
    public ICollection<DiagnosisModel> Diagnosis { get; set; } = new List<DiagnosisModel>();
    public ICollection<LabTestsModel> LabTests { get; set; } = new List<LabTestsModel>();

    public bool ValidPassword(string password)
    {
        return Password == password;
    }
    
}