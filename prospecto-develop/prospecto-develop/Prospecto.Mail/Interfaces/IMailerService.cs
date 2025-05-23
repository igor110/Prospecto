using Prospecto.Mail.Models;
using Prospecto.Mail.Models.Config;

namespace Prospecto.Mail.Interfaces
{
    public interface IMailerService
    {
        void SendMail(MailMessage message, BaseConfig config = null);
    }
}
