using FizzWare.NBuilder;
using FluentAssertions;
using Lealthy_Hospital_Application_System.Controllers;
using Lealthy_Hospital_Application_System.Enum;
using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Lealthy_Hospital_Application_System.Repositories;
using Lealthy_Hospital_Application_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace LHAS.Tests;

public class Login_Tests
{
    private readonly Mock<IStaffRepository>  _staffRepository;
    private readonly Mock<IStaffSession> _staffSession;
    private LoginController _loginController;

    public Login_Tests()
    {
        _staffRepository = new Mock<IStaffRepository>();
        _staffSession = new Mock<IStaffSession>();
        _loginController = new LoginController(_staffRepository.Object, _staffSession.Object);
    }

    private StaffModel CreateStaff()
    {
        var createStaff = Builder<StaffModel>.CreateNew().Build();
        return createStaff;
    }
    private List<StaffModel> CreateStaffList()
    {
        var createStaffList = Builder<StaffModel>.CreateListOfSize(5).Build().ToList();
        return createStaffList;
    }

    private LoginModel CreateLogin()
    {
        var createLogin = Builder<LoginModel>.CreateNew().Build();
        return createLogin;
    }

    [Fact]
    public void Index_ShouldRedirectTo_HomePageView()
    {
        //Arrange
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);

        //Act
        var result = _loginController.Index() as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        result.ControllerName.Should().Be("Home");
    }
    [Fact]
    public void Index_ShouldReturn_LoginView()
    {
        //Arrange
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        //Act
        var result = _loginController.Index() as ViewResult;
        
        //Assert
        result.Should().Be(null);
    }

    [Fact]
    public void Logout_ShouldRemoveUserSession()
    {
        //Arrange
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _staffSession.Setup(x => x.RemoveLoginSession()).Verifiable();
        
        //Act
        var result = _loginController.Logout() as RedirectToActionResult;
        
        //Arrange
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        result.ControllerName.Should().Be("Login");
    }

    [Fact]
    public void Privacy_ShouldReturn_View()
    {
        //Arrange
        var staff = CreateStaff();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _loginController.Privacy() as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void Login_ShouldRedirectToLogin_WhenSuccessful()
    {
        //Arrange
        var staffList = CreateStaffList();
        var staff = CreateStaff();
        var login = CreateLogin();
        
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(login.Password);
        staff.Password = hashedPassword;
        
        _staffRepository.Setup(x => x.GetAllStaff()).Returns(staffList);
        _staffRepository.Setup(x => x.GetByLogin(staff.Login)).Returns(staff);
        _staffSession.Setup(x => x.CreateLoginSession(staff)).Verifiable();

        
        //Act
        var result = _loginController.Login(login) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _staffSession.Verify(x => x.CreateLoginSession(staff), Times.Once);
    }
    
    [Fact]
    public void Login_ShouldNotLogin_WhenWrongDataInput()
    {
        //Arrange
        var staffList = CreateStaffList();
        var staff = CreateStaff();
        var login = CreateLogin();
        var httpContext = new DefaultHttpContext();

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(login.Password);
        staff.Password = hashedPassword;
        login.Password = "wrongpassword";
        
        
        _staffRepository.Setup(x => x.GetAllStaff()).Returns(staffList);
        _staffRepository.Setup(x => x.GetByLogin(staff.Login)).Returns(staff);
        _staffSession.Setup(x => x.CreateLoginSession(staff)).Verifiable();
        _loginController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["ErrorMessage"] = "Login and/or password invalid"
        };
        
        //Act
        var result = _loginController.Login(login) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("Index");
        _loginController.TempData["ErrorMessage"].Should().Be("Login and/or password invalid");
    }

    [Fact]
    public void Login_ShouldCreateAdminStaff_WhenNoStaffOnDatabase()
    {
        // Arrange
        var login = new LoginModel { Login = "TestAdmin", Password = "TestAdmin@123" };
        var adminStaff = new StaffModel
        {
            Name = "TestAdmin",
            Email = "TestAdmin@gmail.com",
            Mobile = "784578965",
            Address = "Some Address",
            DateOfBirth = DateTime.Today,
            Access = RoleAccessEnum.Administrator,
            Login = "TestAdmin",
            Password = BCrypt.Net.BCrypt.HashPassword("TestAdmin@123") 
        };
        
        _staffRepository.Setup(x => x.RegisterStaff(It.IsAny<StaffModel>())).Callback<StaffModel>(s => adminStaff = s);
        _staffRepository.Setup(x => x.GetByLogin("TestAdmin")).Returns(adminStaff);
        _staffSession.Setup(x => x.CreateLoginSession(It.IsAny<StaffModel>())).Verifiable();

        // Act
        var result = _loginController.Login(login) as RedirectToActionResult;

        // Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        _staffSession.Verify(x => x.CreateLoginSession(It.Is<StaffModel>(s => s.Login == "TestAdmin")), Times.Once);
        _staffRepository.Verify(x => x.RegisterStaff(It.Is<StaffModel>(s => s.Login == "TestAdmin")), Times.Once);
    }

}