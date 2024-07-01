using FizzWare.NBuilder;
using FluentAssertions;
using Lealthy_Hospital_Application_System.Controllers;
using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace LHAS.Tests;

public class Staff_Tests
{
    private readonly Mock<IStaffRepository> _staffRepository;
    private readonly Mock<IStaffSession> _staffSession;
    private  StaffController _staffController;

    public Staff_Tests()
    {
        _staffRepository = new Mock<IStaffRepository>();
        _staffSession = new Mock<IStaffSession>();
        _staffController = new StaffController(_staffRepository.Object, _staffSession.Object);
    }

    private List<StaffModel> CreateStaffsList()
    {
        var GetAllData = Builder<StaffModel>.CreateListOfSize(10).Build().ToList();
        return GetAllData;
    }
    
    private StaffModel CreateStaff()
    {
        var GetAllData = Builder<StaffModel>.CreateNew().Build();
        return GetAllData;
    }
    private StaffWithoutPwdModel CreateNoPwdStaff()
    {
        var GetAllData = Builder<StaffWithoutPwdModel>.CreateNew().Build();
        return GetAllData;
    }

    [Fact]
    public void ListAllStaff_Should_ListAll()
    {
        //Arrange
        var staffs = CreateStaffsList();
        var staff = CreateStaff();
        
        _staffRepository.Setup(x => x.GetAllStaff()).Returns(staffs);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _staffController.Index() as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Model.Should().BeEquivalentTo(staffs);
    }

    [Fact]
    public void CreateStaff_Should_RegisterStaff()
    {
        //Arrange
        var httpContext = new DefaultHttpContext();
        var staff = CreateStaff();
        var staffs = CreateStaffsList();
        staff.Login = "Login100";
        staff.Email = "Email100";
        staff.Mobile = "789745485";
        _staffController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Staff has been created."
        };
        
        _staffRepository.Setup(x => x.GetAllStaff()).Returns(staffs);
        _staffRepository.Setup(x => x.RegisterStaff(staff)).Verifiable();
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _staffController.CreateStaff(staff);
        
        //Assert
        result.Should().NotBeNull();
        _staffController.TempData["SuccessMessage"].Should().Be("Staff has been created.");
    }
    [Fact]
    public void CreateStaff_Should_Not_Allow_Duplicates()
    {
        //Arrange
        var httpContext = new DefaultHttpContext();
        var staff = CreateStaff();
        var staffs = CreateStaffsList();
        
        _staffController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["ErrorMessage"] = "Login already registered."
        };
        
        _staffRepository.Setup(x => x.GetAllStaff()).Returns(staffs);
        _staffRepository.Setup(x => x.RegisterStaff(staff)).Verifiable();
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        _staffController.CreateStaff(staff);
        
        //Assert
        _staffController.TempData["ErrorMessage"].Should().Be("Login already registered.");
    }

    [Fact]
    public void UpdateStaff_Should_Update()
    {
        //Arrange
        var httpContext = new DefaultHttpContext();
        var staffNoPwd = CreateNoPwdStaff();
        var staff = CreateStaff();
        var staffs = CreateStaffsList();
        
        _staffRepository.Setup(x => x.GetAllStaff()).Returns(staffs);
        _staffRepository.Setup(x => x.UpdateStaff(staffNoPwd)).Verifiable();
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);

        _staffController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Staff has been updated."
        };
        
        //Act
        var result = _staffController.UpdateStaff(staffNoPwd) as RedirectToActionResult; 
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _staffController.TempData["SuccessMessage"].Should().Be("Staff has been updated.");
    }
    
    [Fact]
    public void UpdateStaff_Should_ReturnView()
    {
        // Arrange
        var staffNoPwd = CreateNoPwdStaff(); 
        var staff = CreateStaff(); 
        var staffs = CreateStaffsList(); 
        var staffId = staffNoPwd.StaffId;

        var staffModel = new StaffModel
        {
            StaffId = staffId,
            Name = staffNoPwd.Name,
            Email = staffNoPwd.Email,
            Mobile = staffNoPwd.Mobile,
            Address = staffNoPwd.Address,
            DateOfBirth = staffNoPwd.DateOfBirth,
            StaffType = staffNoPwd.Access,
            Login = staffNoPwd.Login
        };

        _staffRepository.Setup(x => x.GetAllStaff()).Returns(staffs);
        _staffRepository.Setup(x => x.GetStaffById(staffId)).Returns(staffModel);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);

     

        // Act
        var result = _staffController.UpdateStaff(staffId) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().BeOfType<StaffWithoutPwdModel>();
        var model = result.Model as StaffWithoutPwdModel;
        model.Should().NotBeNull();
        model.StaffId.Should().Be(staffId);
        model.Name.Should().Be(staffNoPwd.Name);
        model.Email.Should().Be(staffNoPwd.Email);
        model.Mobile.Should().Be(staffNoPwd.Mobile);
        model.Address.Should().Be(staffNoPwd.Address);
        model.DateOfBirth.Should().Be(staffNoPwd.DateOfBirth);
        model.Access.Should().Be(staffNoPwd.Access);
        model.Login.Should().Be(staffNoPwd.Login);
    }

    [Fact]
    public void DeleteStaff_Should_ReturnView()
    {
        //Arrange
        var staff = CreateStaff();
        var staffs = CreateStaffsList();

        _staffRepository.Setup(x => x.GetStaffById(staff.StaffId)).Verifiable();
        _staffRepository.Setup(x => x.GetAllStaff()).Returns(staffs);
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);


        //Act
        var result = _staffController.DeleteStaff(staff.StaffId) as ViewResult;

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
        _staffRepository.Verify(x => x.GetStaffById(staff.StaffId), Times.Once);
    }

    [Fact]
    public void DeleteStaff_Should_ReturnDeleted()
    {
        //Arrange
        var staff = CreateStaff();
        var httpContext = new DefaultHttpContext();

        _staffRepository.Setup(x => x.DeleteStaff(staff.StaffId)).Returns(true);
        _staffController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Staff has been deleted."
        };
        
        //Act
        var result = _staffController.DeleteStaff(staff) as RedirectToActionResult; 
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _staffController.TempData["SuccessMessage"].Should().Be("Staff has been deleted.");
    }
}