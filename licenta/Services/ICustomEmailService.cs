using licenta.Models;

namespace licenta.Services
{
    public interface ICustomEmailService
    {
        void SendEmail(EmailMessage emailMessage);
    }
}