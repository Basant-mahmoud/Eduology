using Eduology.Application.Interface;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "eduology24@outlook.com";
            var password = "eduology@2024"; 
             var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            var mailMessage = new MailMessage(from: mail, to: email, subject: subject, body: message);
            return client.SendMailAsync(mailMessage); 
        }
    }
}
