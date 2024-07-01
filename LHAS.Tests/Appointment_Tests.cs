using FizzWare.NBuilder;
using FizzWare.NBuilder.Extensions;
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

public class Appointment_Tests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepository;
    private readonly Mock<IStaffRepository> _staffRepository;
    private readonly Mock<IPatientRepository> _patientRepository;
    private readonly Mock<IStaffSession> _staffSession;
    private AppointmentController _appointmentController;
    
    public Appointment_Tests()
    {
        _appointmentRepository = new Mock<IAppointmentRepository>();
        _staffRepository = new Mock<IStaffRepository>();
        _patientRepository = new Mock<IPatientRepository>();
        _staffSession = new Mock<IStaffSession>();
        _appointmentController = new AppointmentController(_appointmentRepository.Object, _staffRepository.Object, _patientRepository.Object
                                                            ,_staffSession.Object);
    }

    private List<AppointmentModel> CreateAppointmentList()
    {
        var createAppointmentList = Builder<AppointmentModel>.CreateListOfSize(5).Build().ToList();
        return createAppointmentList;
    }

    private List<PatientModel> CreatePatientList()
    {
        var createPatientList = Builder<PatientModel>.CreateListOfSize(5).Build().ToList();
        return createPatientList;
    }

    private List<StaffModel> CreateStaffList()
    {
        var createStaffList = Builder<StaffModel>.CreateListOfSize(5).Build().ToList();
        return createStaffList;
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
    public void Index_ShouldReturn_View()
    {
        //Arrange
        var appointmentList = CreateAppointmentList();
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _appointmentRepository.Setup(x => x.GetAllAppointments()).Returns(appointmentList);
        
        //Act
        var result = _appointmentController.Index() as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void CreateAppointment_ShouldReturn_CreateView()
    {
        //Arrange
        var staffList = CreateStaffList();
        var patientList = CreatePatientList();
        var staff = CreateStaff();
        staff.StaffType = RoleAccessEnum.Doctor;

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _staffRepository.Setup(x => x.GetAllStaff()).Returns(staffList);
        _patientRepository.Setup(x => x.GetAllPatients()).Returns(patientList);
        
        //Act
        var result = _appointmentController.CreateAppointment() as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void CreateAppointment_ShouldReturn_SuccessfulWhenCreate()
    {
        //Arrange
        var appointment = CreateAppointment();
        var staff = CreateStaff();
        var  httpContext = new DefaultHttpContext();
        PatientModel patient = new PatientModel()
        {
            PatientId = 1
        };
        appointment.PatientId = 1;
        appointment.StaffId = staff.StaffId;
        appointment.StaffName = "John";
        appointment.PatientName = "Patient";

        _patientRepository.Setup(x => x.GetPatientById(1)).Returns(patient);
        _staffRepository.Setup(x => x.GetStaffById(staff.StaffId)).Returns(staff);
        _appointmentRepository.Setup(x => x.CreateAppointment(appointment)).Returns(true);
        _appointmentController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Appointment has been successful created."
        };
        
        //Act
        var result = _appointmentController.CreateAppointment(appointment) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _appointmentController.TempData["SuccessMessage"].Should().Be("Appointment has been successful created.");
    }

    [Fact]
    public void CreateAppointment_ShouldReturn_ErrorMessageWhenDuplicated()
    {
        var appointment = CreateAppointment();
        var staff = CreateStaff();
        var  httpContext = new DefaultHttpContext();
        PatientModel patient = new PatientModel()
        {
            PatientId = 1
        };
        appointment.PatientId = 1;
        appointment.StaffId = staff.StaffId;
        appointment.StaffName = "John";
        appointment.PatientName = "Patient";

        _patientRepository.Setup(x => x.GetPatientById(1)).Returns(patient);
        _staffRepository.Setup(x => x.GetStaffById(staff.StaffId)).Returns(staff);
        _appointmentRepository.Setup(x => x.CreateAppointment(appointment)).Returns(false);
        _appointmentController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["ErrorMessage"] = $"{appointment.StaffName} already have an appointment for this time."
        };
        
        //Act
        var result = _appointmentController.CreateAppointment(appointment) as RedirectToActionResult;
        
        //Assert
        result.ActionName.Should().Be("Index");
        _appointmentController.TempData["ErrorMessage"].Should()
            .Be($"{appointment.StaffName} already have an appointment for this time.");
    }

    [Fact]
    public void UpdateAppointment_ShouldReturn_View()
    {
        //Arrange
        var staffList = CreateStaffList();
        var patientList = CreatePatientList();
        var appointment = CreateAppointment();
        var staff = CreateStaff();
        staff.StaffType = RoleAccessEnum.Doctor;

        _appointmentRepository.Setup(x => x.GetAppointmentById(appointment.AppointmentId)).Returns(appointment);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _staffRepository.Setup(x => x.GetAllStaff()).Returns(staffList);
        _patientRepository.Setup(x => x.GetAllPatients()).Returns(patientList);
        
        //Act
        var result = _appointmentController.UpdateAppointment(appointment.AppointmentId) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void UpdateAppointment_ShouldReturn_SuccessfulUpdated()
    {
        //Arrange
        var appointment = CreateAppointment();
        var staff = CreateStaff();
        var  httpContext = new DefaultHttpContext();
        PatientModel patient = new PatientModel()
        {
            PatientId = 1
        };
        appointment.PatientId = 1;
        appointment.StaffId = staff.StaffId;
        appointment.StaffName = "John";
        appointment.PatientName = "Patient";

        _patientRepository.Setup(x => x.GetPatientById(1)).Returns(patient);
        _staffRepository.Setup(x => x.GetStaffById(staff.StaffId)).Returns(staff);
        _appointmentRepository.Setup(x => x.UpdateAppointment(appointment)).Returns(true);
        _appointmentController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Appointment has been successful updated."
        };
        
        //Act
        var result = _appointmentController.UpdateAppointment(appointment) as RedirectToActionResult;

        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _appointmentController.TempData["SuccessMessage"].Should().Be("Appointment has been successful updated.");
    }

    [Fact]
    public void UpdateAppointment_ShouldReturn_ErrorMessageWhenDuplicated()
    {
        //Arrange
        var appointment = CreateAppointment();
        var staff = CreateStaff();
        var  httpContext = new DefaultHttpContext();
        PatientModel patient = new PatientModel()
        {
            PatientId = 1
        };
        appointment.PatientId = 1;
        appointment.StaffId = staff.StaffId;
        appointment.StaffName = "John";
        appointment.PatientName = "Patient";

        _patientRepository.Setup(x => x.GetPatientById(1)).Returns(patient);
        _staffRepository.Setup(x => x.GetStaffById(staff.StaffId)).Returns(staff);
        _appointmentRepository.Setup(x => x.UpdateAppointment(appointment)).Returns(false);
        _appointmentController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["ErrorMessage"] = $"{appointment.StaffName} already have an appointment for this time."
        };
        
        //Act
        var result = _appointmentController.UpdateAppointment(appointment) as RedirectToActionResult;

        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _appointmentController.TempData["ErrorMessage"].Should().Be($"{appointment.StaffName} already have an appointment for this time.");
    }

    [Fact]
    public void DeleteAppointment_ShouldReturn_View()
    {
        //Arrange
        var staffList = CreateStaffList();
        var patientList = CreatePatientList();
        var appointment = CreateAppointment();
        var staff = CreateStaff();
        staff.StaffType = RoleAccessEnum.Doctor;

        _appointmentRepository.Setup(x => x.GetAppointmentById(appointment.AppointmentId)).Returns(appointment);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _staffRepository.Setup(x => x.GetAllStaff()).Returns(staffList);
        _patientRepository.Setup(x => x.GetAllPatients()).Returns(patientList);
        
        //Act
        var result = _appointmentController.DeleteAppointment(appointment.AppointmentId) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void DeleteAppointment_ShouldReturn_DeletedWhenSuccessful()
    {
        //Arrange
        var appointment = CreateAppointment();
        var  httpContext = new DefaultHttpContext();

        _appointmentRepository.Setup(x => x.DeleteAppointment(appointment.AppointmentId)).Returns(true);
        _appointmentController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Appointment has been successful deleted."
        };
        
        //Act
        var result = _appointmentController.DeleteAppointment(appointment) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _appointmentController.TempData["SuccessMessage"].Should().Be("Appointment has been successful deleted.");
    }

    [Fact]
    public void DeleteAppointment_ShouldReturn_ViewWhenDeleteFail()
    {
        //Arrange
        var appointment = CreateAppointment();

        _appointmentRepository.Setup(x => x.DeleteAppointment(appointment.AppointmentId)).Returns(false);
        
        //Act
        var result = _appointmentController.DeleteAppointment(appointment) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }
}