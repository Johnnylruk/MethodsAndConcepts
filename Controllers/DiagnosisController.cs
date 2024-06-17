using Lealthy_Hospital_Application_System.Filters;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

[UserLoggedPage]

public class DiagnosisController : Controller
{
    private readonly IDiagnosisRepository _diagnosisRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IPatientRepository _patientRepository;

    public DiagnosisController(IDiagnosisRepository _diagnosisRepository)
    {
        this._diagnosisRepository = _diagnosisRepository;
    }
    
    public IActionResult Index()
    {
        List<DiagnosisModel> ListAll = _diagnosisRepository.GetAllDiagnosis();
        return View(ListAll);
    }

    public IActionResult CreateDiagnosis()
    {
        return View();
    }
    
    
}