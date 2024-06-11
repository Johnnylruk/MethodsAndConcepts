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
    [Required(ErrorMessage = "Type Staff Name")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Type Staff Email")]
    [EmailAddress]
    public string Email { get; set;}
    [Required(ErrorMessage = "Type your Mobile")]
    [Phone(ErrorMessage = "Contact number it's not valid.")]
    public string Mobile { get; set;}
    [Required(ErrorMessage = "Type your Address")]
    public string Address { get; set;}
   
    [Required(ErrorMessage = "Select your date of birth.")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set;}
    
    [Required(ErrorMessage = "Select user role based access")]
    [Display(Name = "Staff Role")]
    public RoleAccessEnum Access { get; set; }

    public ICollection<AppointmentModel> Appointments { get; set; } = new List<AppointmentModel>();
    public ICollection<DiagnosisModel> Diagnosis { get; set; } = new List<DiagnosisModel>();
    public ICollection<LabTestsModel> LabTests { get; set; } = new List<LabTestsModel>();
}