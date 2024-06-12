using System.ComponentModel.DataAnnotations;
using Lealthy_Hospital_Application_System.Enum;

namespace Lealthy_Hospital_Application_System.Models;

public class DiagnosisModel
{
    [Key]
    public int DiagnosisId { get; set; }
    public IllnessesList TypeOfDiagnosis { get; set;}
    public string? Treatment { get; set; }
    public DateTime Date { get; set; }
    public string StaffName { get; set; }
    public string PatientName { get; set; }
    public int StaffId { get; set; }
    public virtual StaffModel Staff { get; set; }
    public int PatientId { get; set; }
    
    public virtual PatientModel Patient { get; set; }
}