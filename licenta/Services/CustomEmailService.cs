using licenta.Helpers;
using licenta.Helpers.Dtos;
using licenta.Models;
using System.Net;
using System.Net.Mail;

namespace licenta.Services
{
    public class CustomEmailService : ICustomEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly SmtpCredentials _smtpCredentials;
        public CustomEmailService()
        {
            _smtpCredentials = JsonHelper.ReadJson(Constants.SmtpJsonCredentialsPath);
            _smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(_smtpCredentials.email, _smtpCredentials.password),
                Timeout = 20000
            };

        }

        public void SendEmail(EmailMessage emailMessage)
        {
            var fromAddress = new MailAddress(_smtpCredentials.email, "BooITSM SYSTEM");
            var toAddress = new MailAddress(emailMessage.recieverAddress, emailMessage.recieverName);

            
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = emailMessage.subject,
                Body = emailMessage.body
            })
            {
                _smtpClient.Send(message);
            }
        }
    }
}