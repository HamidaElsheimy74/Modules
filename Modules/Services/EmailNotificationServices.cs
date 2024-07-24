using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Modules.Helpers;
using Modules.Interfaces;

namespace Modules.Services;

public class EmailNotificationServices : IEmailNotificationServices
{

    private readonly EmailSettings _emailSettings;
    public EmailNotificationServices(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;

    }
    public async Task SendMail(string receiverEmail, string title)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DeveloperTest", $"{_emailSettings.Email}"));
            message.To.Add(new MailboxAddress($"{receiverEmail}", $"{receiverEmail}"));
            message.Subject = $"{title}";

            message.Body = new TextPart("plain")
            {
                Text = string.Format(ModulesConstants.EmailBody, $"{receiverEmail}", $"{title}")
            };

            using (var client = new SmtpClient())
            {
                client.Connect($"{_emailSettings.Host}", 587, MailKit.Security.SecureSocketOptions.StartTls);

                client.Authenticate($"{_emailSettings.Email}", $"{_emailSettings.Password}");

                client.Send(message);
                client.Disconnect(true);
            }

        }
        catch (Exception)
        {

            throw;
        }

    }
}
