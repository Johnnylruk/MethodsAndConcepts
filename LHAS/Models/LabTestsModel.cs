using System.ComponentModel.DataAnnotations;
using Lealthy_Hospital_Application_System.Enum;

namespace Lealthy_Hospital_Application_System.Models;

public class LabTestsModel
{
    //CREATE A LABTEST BEFORE CREATING THE RESULT//
    
    [Key] 
    public int LabTestId { get; set; }
    public LaboratoryTests TypeOfTests { get; set; }
    public string? Description { get; set;}
    public DateTime Date { get; set; }
    public bool? Result { get; set; }
    public string StaffName { get; set; }
    public string PatientName { get; set; }
    public int StaffId { get; set; }
    public virtual StaffModel Staff { get; set; }
    public int PatientId { get; set;}
    public virtual PatientModel Patient { get; set; }

}