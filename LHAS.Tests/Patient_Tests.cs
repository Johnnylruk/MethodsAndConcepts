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
        staff.Access = RoleAccessEnum.Doctor;

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
        var patient = CreatePatient();
        var staff = CreateStaff();
        staff.Access = RoleAccessEnum.Doctor;

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
    public void CreatePatient_Should_SuccessfulCreated()
    {
        
    }
    
}