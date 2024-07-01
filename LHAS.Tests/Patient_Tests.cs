using System.Collections.Immutable;
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

public class Patient_Tests
{
    private readonly Mock<IPatientRepository> _patientRepository;
    private readonly Mock<IAppointmentRepository> _appointmentRepository;
    private readonly Mock<IStaffSession> _staffSession;
    private PatientController _patientController;

    public Patient_Tests()
    {
        _patientRepository = new Mock<IPatientRepository>();
        _appointmentRepository = new Mock<IAppointmentRepository>();
        _staffSession = new Mock<IStaffSession>();
        _patientController = new PatientController(_patientRepository.Object, _staffSession.Object, _appointmentRepository.Object);
    }

    private List<PatientModel> CreateListOfPatients()
    {
        var createList = Builder<PatientModel>.CreateListOfSize(5).Build().ToList();
        return createList;
    }
    private List<AppointmentModel> CreateListOfAppointments()
    {
        var createList = Builder<AppointmentModel>.CreateListOfSize(5).Build().ToList();
        return createList;
    }

    private PatientModel CreatePatient()
    {
        var createPatient = Builder<PatientModel>.CreateNew().Build();
        return createPatient;
    }
    
    private AppointmentModel CreateAppointment()
    {
        var createAppointment = Builder<AppointmentModel>.CreateNew().Build();
        return createAppointment;
    }

    private StaffModel CreateStaff()
    {
        var createStaff = Builder<StaffModel>.CreateNew().Build();
        return createStaff;
    }
    
    [Fact]
    public void Index_ShouldReturnView()
    {
        //Arrange
        var patientList = CreateListOfPatients();
        var appointmentList = CreateListOfAppointments();
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _appointmentRepository.Setup(x => x.GetAllAppointments()).Returns(appointmentList);
        _patientRepository.Setup(x => x.GetAllPatients()).Returns(patientList);
        
        //Act
        var result = _patientController.Index() as ViewResult;

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }
    
    [Fact]
    public void Index_ShouldReturn_PrivateViewForDoctors()
    {
        //Arrange
        var patientList = CreateListOfPatients();
        var appointmentList = new List<AppointmentModel>();
        var patient = CreatePatient();
        var staff = CreateStaff();
        staff.StaffType = RoleAccessEnum.Doctor;

        foreach (var appointment in CreateListOfAppointments())
        {
            appointment.Patient = CreatePatient();
            appointmentList.Add(appointment);
        }
        
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _appointmentRepository.Setup(x => x.GetAllAppointments()).Returns(appointmentList);
        _patientRepository.Setup(x => x.GetAllPatients()).Returns(patientList);
     
        
        //Act
        var result = _patientController.Index() as ViewResult;

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void Index_ShouldReturn_NoPatient()
    {
        //Arrange
        var httpContext = new DefaultHttpContext();
        var patientList = CreateListOfPatients();
        var appointmentList = new List<AppointmentModel>();
        var staff = CreateStaff();
        staff.StaffType = RoleAccessEnum.Doctor;

        foreach (var appointment in CreateListOfAppointments())
        {
            appointment.Patient = CreatePatient();
            appointment.StaffId = 10;
            appointmentList.Add(appointment);
        }

        _patientController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["ErrorMessage"] = "You do not have any Patient."
        };
        
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _appointmentRepository.Setup(x => x.GetAllAppointments()).Returns(appointmentList);
        _patientRepository.Setup(x => x.GetAllPatients()).Returns(patientList);
     
        
        //Act
        var result = _patientController.Index() as RedirectToActionResult;

        //Assert
        result.ActionName.Should().Be("Index");
        _patientController.TempData["ErrorMessage"].Should().Be("You do not have any Patient.");
    }
    
    
    
    [Fact]
    public void CreatePatient_Should_ReturnView()
    {
        //Arrange
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _patientController.CreatePatient() as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
    }
    [Fact]
    public void CreatePatient_Should_SuccessfulCreate()
    {
        //Arrange
        var patients = CreateListOfPatients();
        var httpContext = new DefaultHttpContext();
        PatientModel patient = new PatientModel()
        {
            PatientId = 10,
            Name = "SomeName",
            Mobile = "07853651354",
            Address = "SomeAddress",
            DateOfBirth = DateTime.Today
        };
        
        
        _patientRepository.Setup(x => x.GetAllPatients()).Returns(patients);
        _patientRepository.Setup(x => x.RegisterPatient(patient)).Verifiable();
        _patientController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Patient has been successful created."
        };
        
        //Act
        var result = _patientController.CreatePatient(patient) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _patientController.TempData["SuccessMessage"].Should().Be("Patient has been successful created.");
    }
    [Fact]
    public void CreatePatient_Should_NotCreateWhenDuplicated()
    {
        //Arrange
        var patients = CreateListOfPatients();
        var patient = CreatePatient();
        var httpContext = new DefaultHttpContext();
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _patientRepository.Setup(x => x.GetAllPatients()).Returns(patients);
        _patientRepository.Setup(x => x.RegisterPatient(patient)).Verifiable();
        _patientController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["ErrorMessage"] = "This email already exist."
        };
        
        //Act
        _patientController.CreatePatient(patient);
        
        //Assert
        _patientController.TempData["ErrorMessage"].Should().Be("This email already exist.");
    }
    
    [Fact]
    public void UpdatePatient_Should_ReturnView()
    {
        //Arrange
        var staff = CreateStaff();
        var patient = CreatePatient();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _patientController.UpdatePatient(patient.PatientId) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
    }
    
    [Fact]
    public void UpdatePatient_Should_SuccessfulUpdate()
    {
        //Arrange
        var patients = CreateListOfPatients();
        var httpContext = new DefaultHttpContext();
        var patient = CreatePatient();
        
        _patientRepository.Setup(x => x.GetAllPatients()).Returns(patients);
        _patientRepository.Setup(x => x.UpdatePatient(patient)).Verifiable();
        _patientController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Patient has been successful updated."
        };
        
        //Act
        var result = _patientController.CreatePatient(patient) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _patientController.TempData["SuccessMessage"].Should().Be("Patient has been successful updated.");
    }
    [Fact]
    public void UpdatePatient_Should_NotUpdateWhenDuplicated()
    {
        //Arrange
        var patients = CreateListOfPatients();
        var patient = CreatePatient();
        var httpContext = new DefaultHttpContext();
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _patientRepository.Setup(x => x.GetAllPatients()).Returns(patients);
        _patientRepository.Setup(x => x.UpdatePatient(patient)).Verifiable();
        _patientController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["ErrorMessage"] = "This email already exist."
        };
        
        //Act
        _patientController.UpdatePatient(patient);
        
        //Assert
        _patientController.TempData["ErrorMessage"].Should().Be("This email already exist.");
    }
    [Fact]
    public void DeletePatient_Should_ReturnView()
    {
        //Arrange
        var staff = CreateStaff();
        var patient = CreatePatient();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _patientController.DeletePatient(patient.PatientId) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void DeletePatient_Should_DeletePatientWhenSuccessful()
    {
        //Arrange
        var patient = CreatePatient();
        var httpContext = new DefaultHttpContext();

        _patientRepository.Setup(x => x.DeletePatient(patient.PatientId)).Returns(true);
        _patientController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Patient has been successful deleted."
        };
        //Act
        var restult = _patientController.DeletePatient(patient) as RedirectToActionResult;
        
        //Assert
        restult.ActionName.Should().Be("Index");
        _patientController.TempData["SuccessMessage"].Should().Be("Patient has been successful deleted.");

    }
}