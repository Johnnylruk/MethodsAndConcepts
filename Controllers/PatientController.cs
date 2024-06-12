using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lealthy_Hospital_Application_System.Controllers;

public class PatientController : Controller
{
    private IPatientRepository _patientRepository;

    public PatientController(IPatientRepository _patientRepository)
    {
        this._patientRepository = _patientRepository;
    }
    // GET
    public IActionResult Index()
    {
        List<PatientModel> ListPatients = _patientRepository.GetAllPatients(); 
        return View(ListPatients);
    }

    public IActionResult CreatePatient()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreatePatient(PatientModel patientModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _patientRepository.RegisterPatient(patientModel);
                TempData["SuccessMessage"] = "Patient has been successful created.";
                return RedirectToAction("Index");
            }

            return View(patientModel);
        }
        catch (Exception error)
        {
            TempData["ErrorMessage"] = $"Ops, problem when trying to create patient. Error {error.Message}";
            return RedirectToAction("Index");
        }
    }
}