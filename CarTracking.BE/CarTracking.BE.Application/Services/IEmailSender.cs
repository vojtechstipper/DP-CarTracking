using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CarTracking.BE.Application.Services;

public interface IEmailSender
{
    Task SendPassworResetEmail(string email, string data);
}

public class SendGridEmailSender(IConfiguration configuration) : IEmailSender
{
    public async Task SendPassworResetEmail(string email, string data)
    {
        var client = new SendGridClient(configuration["Sendgrid:ApiKey"]);
        var from = new EmailAddress(configuration["Sendgrid:EmailAddress"], configuration["Sendgrid:EmailName"]);
        var to = new EmailAddress(email, "userName");
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, configuration["Sendgrid:PasswordResetTemplateId"],
            new { resetCode = data });
        await client.SendEmailAsync(msg).ConfigureAwait(false);
    }
}