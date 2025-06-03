using System;

namespace Prospecto.Models.ViewModel
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime NotifyDate { get; set; }
        public bool Read { get; set; }
    }
}
