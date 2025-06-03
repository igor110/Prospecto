using Microsoft.AspNetCore.Mvc;
using Prospecto.Models.DTO;
using Prospecto.Service.Interface;
using System.Security.Claims;

namespace Prospecto.ViewMvc.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService service)
        {
            _notificationService = service;
        }

        public IActionResult Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.Sid));
            var notifications = _notificationService.GetUnread(userId);
            return View(notifications);
        }
    }
}
