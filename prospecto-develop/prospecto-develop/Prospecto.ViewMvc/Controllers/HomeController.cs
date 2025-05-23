using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Prospecto.Mail.Interfaces;
using Prospecto.Models.Enums;
using Prospecto.Models.ViewModel;
using Prospecto.Service.Interface;
using Prospecto.ViewMvc.Models;
using System;
using System.Diagnostics;

namespace Prospecto.ViewMvc.Controllers
{
    public class HomeController : ProspectoControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAttendanceService _attendanceService;
        private readonly IMailerService _mailerService;
        private readonly IMessageService _messageService;

        public HomeController(
            IMessageService messageService,
            IMailerService mailerService,
            IAttendanceService attendanceService,
            ILogger<HomeController> logger)
        {
            _logger = logger;
            _attendanceService = attendanceService;
            _mailerService = mailerService;
            _messageService = messageService;
        }

        public IActionResult Index()
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            GetContextData();


            var filters = new RankingByConsultantFiltersViewModel
            {
                RakingBeginDate = date,
                RakingEndDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59)
            };

            if (Role != UserTypeEnum.ADMINISTRATOR.ToString()) filters.CompanyId = CompanyId;

            if (Role == UserTypeEnum.CONSULTANT.ToString())
                filters.BranchId = BranchId;

            ViewBag.RakingBeginDate = filters.RakingBeginDate;
            ViewBag.RakingEndDate = filters.RakingEndDate;

            var lstRanking = _attendanceService.RankingByConsultant(filters);
            return View(lstRanking);
        }

        public IActionResult Ranking(RankingByConsultantFiltersViewModel filters)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            GetContextData();

            filters.RakingEndDate = new DateTime(filters.RakingEndDate.Year, filters.RakingEndDate.Month, DateTime.DaysInMonth(filters.RakingEndDate.Year, filters.RakingEndDate.Month), 23, 59, 59);

            if (Role != UserTypeEnum.ADMINISTRATOR.ToString()) filters.CompanyId = CompanyId;

            if (Role == UserTypeEnum.CONSULTANT.ToString())
                filters.BranchId = BranchId;

            ViewBag.RakingBeginDate = filters.RakingBeginDate;
            ViewBag.RakingEndDate = filters.RakingEndDate;
            var lstRanking = _attendanceService.RankingByConsultant(filters);
            return View("Index", lstRanking);
        }

        [HttpPost]
        public JsonResult Dashboard(string dateInit, string dateEnd, int? userId)
        {
            if (!IsAuthenticate()) Redirect("/Account/Login");
            GetContextData();

            var filters = new SalesChartDataLabelFiltersViewModel { CompanyId = CompanyId, BranchId = BranchId };

            var obj = _attendanceService.SaleByConsultant(filters);
            JObject salesChartData = JObject.FromObject(obj);

            return Json(new { success = true, salesChartData });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
