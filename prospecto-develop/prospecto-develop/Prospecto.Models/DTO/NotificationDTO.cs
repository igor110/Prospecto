using System;

namespace Prospecto.Models.DTO
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime NotifyDate { get; set; }
        public bool Read { get; set; }
    }
}
