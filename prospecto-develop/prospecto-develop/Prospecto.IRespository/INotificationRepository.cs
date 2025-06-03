using Prospecto.Models.DTO;
using System.Collections.Generic;

namespace Prospecto.Repository.Interface
{
    public interface INotificationRepository
    {
        List<NotificationDTO> List();
        void Insert(NotificationDTO dto);
    }
}
