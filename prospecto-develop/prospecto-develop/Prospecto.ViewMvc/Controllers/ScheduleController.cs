using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Prospecto.Models.Enums;
using Prospecto.Models.ViewModel;
using Prospecto.Service.Interface;
using Prospecto.ViewMvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Prospecto.ViewMvc.Controllers
{
    public class ScheduleController : ProspectoControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IAttendanceService _attendanceService;
        private readonly IUserService _userService;

        private readonly string Title = "Agenda";


        public ScheduleController(
            IAttendanceService attendanceService,
            IUserService userService,
            ILogger<ScheduleController> logger)
        {
            _logger = logger;
            _attendanceService = attendanceService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            GetContextData();
            IList<SelectListItem> listConsultant = new List<SelectListItem>();

            var filters = new UserFiltersViewModel { CompanyId = CompanyId };
            if (Role == UserTypeEnum.CONSULTANT.ToString())
                filters.Id = UserId;
            else
                listConsultant.Add(new SelectListItem { Value = "0", Text = "Todos" });

            var lst = _userService.ListByFilters(filters);


            foreach (var item in lst)
            {
                listConsultant.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Id.ToString("0000") + "-" + item.Name, Selected = item.Id == UserId });
            }

            ViewBag.ListConsultant = listConsultant;
            ViewData["Title"] = Title;
            return View();
        }

        [HttpPost]
        public JsonResult EventsSchedule(string dateInit, string dateEnd, int? userId)
        {
            if (!IsAuthenticate()) Redirect("/Account/Login");
            GetContextData();

            var filters = new AttendanceFiltersViewModel { CompanyId = CompanyId };
            var endDate = Convert.ToDateTime(dateEnd);
            filters.BeginDate = Convert.ToDateTime(dateInit);
            filters.EndDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month), 23, 59, 59);

            filters.TypeDate = 2;

            if (Role == UserTypeEnum.CONSULTANT.ToString())
                filters.UserId = UserId;

            if (userId > 0)
                filters.UserId = userId;

            var listEvents = _attendanceService.ListByFilters(filters);

            var itens = listEvents.Where(x => x.Status == StatusAttendancesEnum.OPEN)
                .Select(x => new ScheduleEventsViewModel
                {
                    Description = x.NameClient,
                    Title = x.UserId.ToString("0000") + "-" + x.NameClient + "-" + string.Format("{0:C}", x.Value),
                    Start = string.Format("{0:yyyy-MM-dd HH:mm:ss}", x.DateReturn),
                    End = string.Format("{0:yyyy-MM-dd HH:mm:ss}", x.DateReturn),
                    Id = x.Id,
                    BackgroundColor = x.StatusOrder != StatusOrderEnum.GAIN ? (!string.IsNullOrEmpty(x.User?.Color) ? x.User.Color : "#0073b7") : "#00a65a",
                    BorderColor = x.StatusOrder != StatusOrderEnum.GAIN ? (!string.IsNullOrEmpty(x.User?.Color) ? x.User.Color : "#0073b7") : "#00a65a",
                });

            var oportunidade = string.Format("{0:C}", listEvents.Where(x => x.Status == StatusAttendancesEnum.OPEN).Sum(x => x.Value));

            filters.TypeDate = 3;
            listEvents = _attendanceService.ListByFilters(filters);
            var vendas = string.Format("{0:C}", listEvents.Where(x => x.Status == StatusAttendancesEnum.CLOSED && x.StatusOrder == StatusOrderEnum.GAIN).Sum(x => x.ValueClosed));

            return Json(new { success = true, message = "Atendimento carregado com sucesso!", itens, oportunidade, vendas });
        }

        [HttpPost]
        public async Task<JsonResult> EditEventsSchedule(int idEvent, string date)
        {
            if (!IsAuthenticate()) Redirect("/Account/Login");
            GetContextData();

            var result = await _attendanceService.Reschedule(idEvent, Convert.ToDateTime(date));
            if (result == true) return Json(new { success = true, message = "Atendimento carregado com sucesso!" });
            else return Json(new { success = false, message = "Atendimento não atualizado!" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
