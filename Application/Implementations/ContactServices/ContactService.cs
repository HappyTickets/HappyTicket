using Application.Interfaces.IContactService;
using Domain.Entities.ContactEntity;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Application.Implementations.ContactServices
{
    public class ContactService : IContactService
    {
        private readonly SmtpSettings _smtpSettings;
        public ContactService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task SendMessageAsync(Contact contact, CancellationToken cancellationToken)
        {
            string toEmail = "tm76498@gmail.com";
            string subject = $"Contact Us Message from {contact.Username}";
            string body = $"Name: {contact.Username}\nEmail: {contact.Email}\nMessage: {contact.Message}";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_smtpSettings.Username);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    smtp.EnableSsl = true;

                    try
                    {
                        await smtp.SendMailAsync(mail, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Error Sending Email Please Send Again", ex);
                    }
                }
            }
        }
    }
}
