namespace Modules.Interfaces;

public interface IEmailNotificationServices
{
    Task SendMail(string receiverEmail, string title);
}
