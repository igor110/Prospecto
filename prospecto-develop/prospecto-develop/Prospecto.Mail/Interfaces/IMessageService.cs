using Prospecto.Mail.Models;

namespace Prospecto.Mail.Interfaces
{
    public interface IMessageService
    {
        MailMessage CreateMessageScheduledService(int companyId);
    }
}
