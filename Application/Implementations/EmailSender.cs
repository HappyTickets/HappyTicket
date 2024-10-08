using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Shared.Configs;
using System.Net;
using System.Net.Mail;

namespace Application.Implementations;

public interface IEmailSender
{
    Task<Result<Unit>> SendEmailAsync(string recipient, string subject, string message, IEnumerable<Attachment>? attachments = null, CancellationToken cancellationToken = default);
    Result<Unit> SendEmail(string recipient, string subject, string message, IEnumerable<Attachment>? attachments = null);
}

public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;
    private readonly EmailConfig _emailConfig;

    public EmailSender(ILogger<EmailSender> logger, EmailConfig emailConfig)
    {
        _logger = logger;
        _emailConfig = emailConfig;
    }

    public async Task<Result<Unit>> SendEmailAsync(string recipient, string subject, string message, IEnumerable<Attachment>? attachments = null, CancellationToken cancellationToken = default)
    {
        try
        {
            attachments = attachments?.ToList() ?? [];
            _logger.LogInformation("Sending email to '{recipient}' about '{subject}' with {Count} attachments.", recipient, subject, attachments.Count());
            var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password)
            };
            var mailMessage = new MailMessage(_emailConfig.From, recipient, subject, message) { IsBodyHtml = true };

            foreach (var attachment in attachments)
            {
                mailMessage.Attachments.Add(attachment);
            }

            await client.SendMailAsync(mailMessage, cancellationToken);
            _logger.LogInformation("Email sent successfully to '{recipient}'.", recipient);
            return new(new Unit());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't send the email to '{recipient}'. Error message: {error}", recipient, ex.Message);
            return new(ex);
        }
    }

    public Result<Unit> SendEmail(string recipient, string subject, string message, IEnumerable<Attachment>? attachments = null)
    {
        try
        {
            attachments = attachments?.ToList() ?? [];
            _logger.LogInformation("Sending email to '{recipient}' about '{subject}' with {Count} attachments.", recipient, subject, attachments.Count());
            var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password)
            };
            var mailMessage = new MailMessage(_emailConfig.From, recipient, subject, message) { IsBodyHtml = true };

            foreach (var attachment in attachments)
            {
                mailMessage.Attachments.Add(attachment);
            }

            client.Send(mailMessage);
            _logger.LogInformation("Email sent successfully to '{recipient}'.", recipient);
            return new(new Unit());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't send the email to '{recipient}'. Error message: {error}", recipient, ex.Message);
            return new(ex);
        }
    }
}