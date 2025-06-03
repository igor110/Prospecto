using Prospecto.Models.DTO;
using Prospecto.Repository.Interface;
using Prospecto.Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Prospecto.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;

        public NotificationService(INotificationRepository repo)
        {
            _repo = repo;
        }

        public List<NotificationDTO> GetUnread(int userId)
        {
            return _repo.List().Where(n => n.UserId == userId && !n.Read).ToList();
        }

        public void Create(NotificationDTO dto)
        {
            _repo.Insert(dto);
        }
    }
}
