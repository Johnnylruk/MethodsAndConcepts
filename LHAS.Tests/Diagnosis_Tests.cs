using FizzWare.NBuilder;
using FluentAssertions;
using Lealthy_Hospital_Application_System.Controllers;
using Lealthy_Hospital_Application_System.Enum;
using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace LHAS.Tests;

public class Diagnosis_Tests
{
    private readonly Mock<IDiagnosisRepository> _diagnosisRepository;
    private readonly Mock<IPatientRepository> _patientRepository;
    private readonly Mock<IStaffSession> _staffSession;
    private DiagnosisController _diagnosisController;

    public Diagnosis_Tests()
    {
        _diagnosisRepository = new Mock<IDiagnosisRepository>();
        _staffSession = new Mock<IStaffSession>();
        _patientRepository = new Mock<IPatientRepository>();
        _diagnosisController = new DiagnosisController(_diagnosisRepository.Object, _staffSession.Object, _patientRepository.Object);
    }

    private List<DiagnosisModel> CreateDiagnosisList()
    {
        var createDiagnosisList = Builder<DiagnosisModel>.CreateListOfSize(5).Build().ToList();
        return createDiagnosisList;
    }

    private DiagnosisModel CreateDiagnosis()
    {
        var createDiagnosis = Builder<DiagnosisModel>.CreateNew().Build();
        return createDiagnosis;
    }
    private PatientModel CreatePatient()
        {
            var createPatient = Builder<PatientModel>.CreateNew().Build();
            return createPatient;
        }
    private StaffModel CreateStaff()
        {
            var createStaff = Builder<StaffModel>.CreateNew().Build();
            return createStaff;
        }

    [Fact]
    public void Index_ShouldReturn_View()
    {
        //Arrange
        var staff = CreateStaff();
        var diagnosisList = CreateDiagnosisList();

        _diagnosisRepository.Setup(x => x.GetAllDiagnosis()).Returns(diagnosisList);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _diagnosisController.Index() as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }
    [Fact]
    public void Index_ShouldReturn_DoctorPrivateView()
    {
        //Arrange
        var staff = CreateStaff();
        var diagnosisList = CreateDiagnosisList();
        staff.Access = RoleAccessEnum.Doctor;
        _diagnosisRepository.Setup(x => x.GetAllDiagnosis()).Returns(diagnosisList);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _diagnosisController.Index() as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }
    [Fact]
    public void Index_ShouldReturn_ErrorMessageWhenNoDiagnosisForDoctorView()
    {
        //Arrange
        var httpContext = new DefaultHttpContext();
        var staff = CreateStaff();
        var diagnosisList = CreateDiagnosisList();
        staff.Access = RoleAccessEnum.Doctor;
        staff.StaffId = 10;
        _diagnosisRepository.Setup(x => x.GetAllDiagnosis()).Returns(diagnosisList);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);

        _diagnosisController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["ErrorMessage"] = "You do not have any Diagnosis"
        };
        //Act
        var result = _diagnosisController.Index() as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        result.ControllerName.Should().Be("Home");
        _diagnosisController.TempData["ErrorMessage"].Should().Be("You do not have any Diagnosis.");
    }

    [Fact]
    public void CreateDiagnosis_ShouldReturn_View()
    {
        //Arrange
        var staff = CreateStaff();
        var patient = CreatePatient();
        var diagnosis = CreateDiagnosis();
        diagnosis.PatientId = patient.PatientId;
        diagnosis.StaffId = staff.StaffId;
        diagnosis.PatientName = patient.Name;
        diagnosis.StaffName = staff.Name;
        diagnosis.Date = DateTime.Now;

        _patientRepository.Setup(x => x.GetPatientById(patient.PatientId)).Returns(patient);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _diagnosisController.CreateDiagnosis(patient.PatientId) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void CreateDiagnosis_ShouldReturn_CreatedWhenSuccessful()
    {
        //Arrange
        var diagnosis = CreateDiagnosis();
        var httpContext = new DefaultHttpContext();

        _diagnosisRepository.Setup(x => x.CreateDiagnosis(diagnosis)).Verifiable();
        _diagnosisController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Diagnosis has been successful created."
        };
        
        //Act
        var result = _diagnosisController.CreateDiagnosis(diagnosis) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _diagnosisController.TempData["SuccessMessage"].Should().Be("Diagnosis has been successful created.");
    }

    [Fact]
    public void UpdateDiagnosis_ShouldReturn_View()
    {
        //Arrange
        var diagnosis = CreateDiagnosis();
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _diagnosisRepository.Setup(x => x.GetDiagnosisById(diagnosis.DiagnosisId)).Returns(diagnosis);
        
        //Act
        var result = _diagnosisController.UpdateDiagnosis(diagnosis.DiagnosisId) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void UpdateDiagnosis_ShouldReturn_UpdatedWhenSuccessful()
    {
        //Arrange
        var patient = CreatePatient();
        var staff = CreateStaff();
        var diagnosis = CreateDiagnosis();
        var httpContext = new DefaultHttpContext();

        diagnosis.Patient = patient;
        diagnosis.Staff = staff;
        diagnosis.Date = DateTime.Now;

        _patientRepository.Setup(x => x.GetPatientById(patient.PatientId)).Returns(patient);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _diagnosisRepository.Setup(x => x.UpdateDiagnosis(diagnosis)).Verifiable();
        _diagnosisController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Diagnosis has been successful updated."
        };
        //Act
        var result = _diagnosisController.UpdateDiagnosis(diagnosis) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _diagnosisController.TempData["SuccessMessage"].Should().Be("Diagnosis has been successful updated.");
    }

    [Fact]
    public void DeleteDiagnosis_ShouldReturn_View()
    {
        //Arrange
        var diagnosis = CreateDiagnosis();
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _diagnosisRepository.Setup(x => x.GetDiagnosisById(diagnosis.DiagnosisId)).Returns(diagnosis);
        
        //Act
        var result = _diagnosisController.DeleteDiagnosis(diagnosis.DiagnosisId) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void DeleteDiagnosis_ShouldReturn_DeletedWhenSuccessful()
    {
        //Arrange
        var diagnosis = CreateDiagnosis();
        var httpContext = new DefaultHttpContext();

        _diagnosisRepository.Setup(x => x.GetDiagnosisById(diagnosis.DiagnosisId)).Returns(diagnosis);
        _diagnosisRepository.Setup(x => x.DeleteDiagnosis(diagnosis.DiagnosisId)).Returns(true);
        
        _diagnosisController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Diagnosis has been successful deleted."
        };
        
        //Act
        var result = _diagnosisController.DeleteDiagnosis(diagnosis) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _diagnosisController.TempData["SuccessMessage"].Should().Be("Diagnosis has been successful deleted.");
    }
}