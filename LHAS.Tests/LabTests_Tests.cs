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

public class LabTests_Tests
{
    private readonly Mock<ILabTestsRepository> _labTestsRepository;
    private readonly Mock<IStaffSession> _staffSession;
    private readonly Mock<IPatientRepository> _patientRepository;
    private LabTestsController _labTestsController;

    public LabTests_Tests()
    {
        _labTestsRepository = new Mock<ILabTestsRepository>();
        _staffSession = new Mock<IStaffSession>();
        _patientRepository = new Mock<IPatientRepository>();
        _labTestsController = new LabTestsController(_labTestsRepository.Object, _staffSession.Object, _patientRepository.Object);
    }

     private List<LabTestsModel> CreateLabTestsList()
    {
        var createLabTestsList = Builder<LabTestsModel>.CreateListOfSize(5).Build().ToList();
        return createLabTestsList;
    }

    private LabTestsModel CreateLabTests()
    {
        var createLabTests = Builder<LabTestsModel>.CreateNew().Build();
        return createLabTests;
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
        var labTestsList = CreateLabTestsList();

        _labTestsRepository.Setup(x => x.GetAllLabTests()).Returns(labTestsList);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _labTestsController.Index() as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }
    [Fact]
    public void Index_ShouldReturn_DoctorPrivateView()
    {
        //Arrange
        var staff = CreateStaff();
        var labTestsList = CreateLabTestsList();
        staff.StaffType = RoleAccessEnum.Doctor;
        _labTestsRepository.Setup(x => x.GetAllLabTests()).Returns(labTestsList);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _labTestsController.Index() as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }
    [Fact]
    public void Index_ShouldReturn_ErrorMessageWhenNoLabTestsForDoctorView()
    {
        //Arrange
        var httpContext = new DefaultHttpContext();
        var staff = CreateStaff();
        var labTestsList = CreateLabTestsList();
        staff.StaffType = RoleAccessEnum.Doctor;
        staff.StaffId = 10;
        _labTestsRepository.Setup(x => x.GetAllLabTests()).Returns(labTestsList);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);

        _labTestsController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["ErrorMessage"] = "You do not have any Laboratory Test"
        };
        //Act
        var result = _labTestsController.Index() as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        result.ControllerName.Should().Be("Home");
        _labTestsController.TempData["ErrorMessage"].Should().Be("You do not have any Laboratory Test");
    }

    [Fact]
    public void CreateLabTests_ShouldReturn_View()
    {
        //Arrange
        var staff = CreateStaff();
        var patient = CreatePatient();
        var labTests = CreateLabTests();
        labTests.PatientId = patient.PatientId;
        labTests.StaffId = staff.StaffId;
        labTests.PatientName = patient.Name;
        labTests.StaffName = staff.Name;
        labTests.Date = DateTime.Now;

        _patientRepository.Setup(x => x.GetPatientById(patient.PatientId)).Returns(patient);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _labTestsController.CreateLabTest(patient.PatientId) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void CreateLabTests_ShouldReturn_CreatedWhenSuccessful()
    {
        //Arrange
        var labTests = CreateLabTests();
        var httpContext = new DefaultHttpContext();

        _labTestsRepository.Setup(x => x.RegisterLabTest(labTests)).Verifiable();
        _labTestsController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Laboratory Test has been requested."
        };
        
        //Act
        var result = _labTestsController.CreateLabTest(labTests) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _labTestsController.TempData["SuccessMessage"].Should().Be("Laboratory Test has been requested.");
    }

    [Fact]
    public void UpdateLabTests_ShouldReturn_View()
    {
        //Arrange
        var labTests = CreateLabTests();
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _labTestsRepository.Setup(x => x.GetLabTestById(labTests.LabTestId)).Returns(labTests);
        
        //Act
        var result = _labTestsController.UpdateLabTest(labTests.LabTestId) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void UpdateLabTests_ShouldReturn_UpdatedWhenSuccessful()
    {
        //Arrange
        var patient = CreatePatient();
        var staff = CreateStaff();
        var labTests = CreateLabTests();
        var httpContext = new DefaultHttpContext();

        labTests.Patient = patient;
        labTests.Staff = staff;
        labTests.Date = DateTime.Now;

        _patientRepository.Setup(x => x.GetPatientById(patient.PatientId)).Returns(patient);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _labTestsRepository.Setup(x => x.UpdateLabTest(labTests)).Verifiable();
        _labTestsController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Laboratory Test has been successful updated."
        };
        //Act
        var result = _labTestsController.UpdateLabTest(labTests) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _labTestsController.TempData["SuccessMessage"].Should().Be("Laboratory Test has been successful updated.");
    }

    [Fact]
    public void DeleteLabTests_ShouldReturn_View()
    {
        //Arrange
        var labTests = CreateLabTests();
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _labTestsRepository.Setup(x => x.GetLabTestById(labTests.LabTestId)).Returns(labTests);
        
        //Act
        var result = _labTestsController.DeleteLabTest(labTests.LabTestId) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void DeleteLabTests_ShouldReturn_DeletedWhenSuccessful()
    {
        //Arrange
        var labTests = CreateLabTests();
        var httpContext = new DefaultHttpContext();

        _labTestsRepository.Setup(x => x.GetLabTestById(labTests.LabTestId)).Returns(labTests);
        _labTestsRepository.Setup(x => x.RemoveLabTest(labTests.LabTestId)).Returns(true);
        
        _labTestsController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Laboratory Test has been successful deleted."
        };
        
        //Act
        var result = _labTestsController.DeleteLabTest(labTests) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _labTestsController.TempData["SuccessMessage"].Should().Be("Laboratory Test has been successful deleted.");
    }
    
    
}