using Microsoft.AspNetCore.Mvc;
using Prospecto.Models.DTO;
using Prospecto.Service;
using Prospecto.Service.Interface;
using System.Security.Claims;

namespace Prospecto.ViewMvc.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IAttendanceService _attendanceService;

        public NotificationController(INotificationService notificationService, IAttendanceService attendanceService)
        {
            _notificationService = notificationService;
            _attendanceService = attendanceService;
        }

        public IActionResult Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.Sid));
            var notifications = _notificationService.GetUnread(userId);
            return View(notifications);
        }

        [HttpGet]
        public IActionResult ListAttendances()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.Sid));
            var attendances = _attendanceService.GetPendingNotifications(userId);
            return Json(attendances);
        }
    }
}
