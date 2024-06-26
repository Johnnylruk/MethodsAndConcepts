using FizzWare.NBuilder;
using FluentAssertions;
using Lealthy_Hospital_Application_System.Controllers;
using Lealthy_Hospital_Application_System.Helper;
using Lealthy_Hospital_Application_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace LHAS.Tests;

public class Password_Tests
{
    private readonly Mock<IStaffSession> _staffSession;
    private readonly Mock<IPasswordChange> _passwordChange;
    private readonly Mock<IEmail> _email;
    private PasswordController _passwordController;

    public Password_Tests()
    {
        _staffSession = new Mock<IStaffSession>();
        _passwordChange = new Mock<IPasswordChange>();
        _email = new Mock<IEmail>();
        _passwordController = new PasswordController(_staffSession.Object, _passwordChange.Object, _email.Object);
    }

    private StaffModel CreateStaff()
    {
        var createStaff = Builder<StaffModel>.CreateNew().Build();
        return createStaff;
    }

    private ChangePasswordModel CreatePwdModel()
    {
        var createModel = Builder<ChangePasswordModel>.CreateNew().Build();
        return createModel;
    }

    [Fact]
    public void ChangePassword_ShouldReturn_View()
    {
        //Arrange
        var staff = CreateStaff();
        var passwordModel = CreatePwdModel();
        passwordModel.StaffId = staff.StaffId;
        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        
        //Act
        var result = _passwordController.ChangePassword() as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void ChangePassword_ShouldReturn_SuccessfulWhenUpdated()
    {
        //Arrange
        var httpContext = new DefaultHttpContext();
        var staff = CreateStaff();
        var passwordModel = CreatePwdModel();
        passwordModel.StaffId = staff.StaffId;

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _passwordChange.Setup(x => x.ChangePassword(passwordModel)).Verifiable();
        _passwordController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "Password has been successful updated."
        };
        
        //Act
        var result = _passwordController.ChangeStaffPassword(passwordModel) as RedirectToActionResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("ChangePassword");
        _passwordController.TempData["SuccessMessage"].Should().Be("Password has been successful updated.");
    }

    [Fact]
    public void ChangePassword_ShouldReturn_ErrorWhenModelIsInvalid()
    {
        //Arrange
        var httpContext = new DefaultHttpContext();
        var staff = CreateStaff();
        var passwordModel = CreatePwdModel();

        _staffSession.Setup(x => x.GetLoginSession()).Returns(staff);
        _passwordController.ModelState.AddModelError("PropertyName", "ErrorMessage");
        _passwordController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["ErrorMessage"] = "Error when trying to update your password. Contact your administrator."
        };
        
        //Act
        var result = _passwordController.ChangeStaffPassword(passwordModel) as ViewResult;
        
        //Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("ChangePassword");
        _passwordController.TempData["ErrorMessage"].Should().Be("Error when trying to update your password. Contact your administrator.");
    }

    [Fact]
    public void SendPasswordLink_ShouldSendLink_WhenSuccessful()
    {
        //Arrange
        var httpContext = new DefaultHttpContext();
        var staff = CreateStaff();
        ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
        resetPasswordModel.Login = staff.Login;
        resetPasswordModel.Email = staff.Email;

        string generatedPassword = "GeneratedPassword"; // Mocked generated password
        _passwordChange.Setup(x => x.GeneratePassword()).Returns(generatedPassword);

        string message = $"You have requested a new password.\n If you did not request considering change " +
                         $"your current password for safety \n" +
                         $"Your new Password is: {generatedPassword}"; // Use generatedPassword here

        string subject = "LHAS - Lealthy Hospital Management System";
        
        _passwordChange.Setup(x => x.GetStaffByLoginAndEmail(resetPasswordModel.Login, resetPasswordModel.Email)).Returns(staff);
        _email.Setup(x => x.SendEmailLink(staff.Email, message, subject)).Returns(true).Verifiable();
        staff.Password = BCrypt.Net.BCrypt.HashPassword(generatedPassword);
        _passwordChange.Setup(x => x.ResetPassword(staff)).Returns(staff);
        _passwordController.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SuccessMessage"] = "You new password has been sent to your email."
        };
        
        //Act
        var result = _passwordController.SendResetPasswordLink(resetPasswordModel) as RedirectToActionResult;

        //Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("ResetPassword");
        _passwordController.TempData["SuccessMessage"].Should().Be("You new password has been sent to your email.");
    }
    
    
}