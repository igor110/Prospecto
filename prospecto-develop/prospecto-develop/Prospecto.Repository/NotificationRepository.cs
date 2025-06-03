using Prospecto.Models.DTO;
using Prospecto.Repository.Interface;
using Prospecto.Data;
using System.Collections.Generic;
using System.Linq;

namespace Prospecto.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ProspectoContext _context;

        public NotificationRepository(ProspectoContext context)
        {
            _context = context;
        }

        public List<NotificationDTO> List()
        {
            return _context.Notifications.ToList();
        }

        public void Insert(NotificationDTO dto)
        {
            _context.Notifications.Add(dto);
            _context.SaveChanges();
        }
    }
}
