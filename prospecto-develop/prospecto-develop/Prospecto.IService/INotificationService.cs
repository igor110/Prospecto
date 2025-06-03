using Prospecto.Models.DTO;
using System.Collections.Generic;

namespace Prospecto.Service.Interface
{
    public interface INotificationService
    {
        List<NotificationDTO> GetUnread(int userId);
        void Create(NotificationDTO dto);
    }
}
