namespace Lealthy_Hospital_Application_System.Helper;

public interface IEmail
{
    bool SendEmailLink(string email, string message, string subject);
}