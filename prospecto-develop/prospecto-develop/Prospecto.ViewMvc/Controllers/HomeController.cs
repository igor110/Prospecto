using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Prospecto.Data;
using Prospecto.Mail.Interfaces;
using Prospecto.Models.Enums;
using Prospecto.Models.ViewModel;
using Prospecto.Service.Interface;
using Prospecto.ViewMvc.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace Prospecto.ViewMvc.Controllers
{
    public class HomeController : ProspectoControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAttendanceService _attendanceService;
        private readonly IMailerService _mailerService;
        private readonly IMessageService _messageService;
        private readonly ProspectoContext _context;


        public HomeController(
            IMessageService messageService,
            IMailerService mailerService,
            IAttendanceService attendanceService,
            ILogger<HomeController> logger,
            ProspectoContext context) 
        {
            _logger = logger;
            _attendanceService = attendanceService;
            _mailerService = mailerService;
            _messageService = messageService;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var hoje = DateTime.Now;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
            var diasNoMes = DateTime.DaysInMonth(hoje.Year, hoje.Month);
            var diasCorridos = (hoje - inicioMes).Days + 1;
            var diasRestantes = diasNoMes - diasCorridos;

            // 🔎 Obtém o usuário logado
            var userEmail = User.Identity.Name;

            // 🔁 Busca a meta da filial do usuário (via SalesGoal da Branch)
            var meta = await _context.Users
                .Include(u => u.Branch)
                .Where(u => u.Email == userEmail)
                .Select(u => u.Branch.SalesGoal)
                .FirstOrDefaultAsync();

            // 🗂️ Atendimentos fechados no mês
            var atendimentos = await _context.Attendances
                .Include(a => a.User)
                .Where(a =>
                    a.Status == StatusAttendancesEnum.CLOSED &&
                    a.DateClosed != null &&
                    a.DateClosed >= inicioMes &&
                    a.DateClosed <= hoje)
                .ToListAsync();

            // 📊 Totais gerais
            var totalExecutado = atendimentos.Sum(a => a.ValueClosed);
            var mediaDiariaGeral = diasCorridos > 0 ? totalExecutado / diasCorridos : 0;
            var previsaoGeral = totalExecutado + (mediaDiariaGeral * diasRestantes);
            var diferenca = meta - totalExecutado;

            // 👥 Vendas por consultor
            var totalConsultores = atendimentos.Select(a => a.UserId).Distinct().Count();

            var vendasPorConsultor = atendimentos
                .GroupBy(a => a.User.Name)
                .Select(g =>
                {
                    var soma = g.Sum(x => x.ValueClosed);
                    var media = diasCorridos > 0 ? soma / diasCorridos : 0;
                    return new VendaPorConsultor
                    {
                        Nome = g.Key,
                        Valor = soma,
                        MetaConsultor = totalConsultores > 0 ? meta / totalConsultores : 0,
                        Projecao = soma + (media * diasRestantes)
                    };
                })
                .ToList();

            foreach (var v in vendasPorConsultor)
            {
                v.PercentualMeta = v.MetaConsultor == 0 ? 0 : (v.Valor / v.MetaConsultor) * 100;
                v.ValorFaltante = v.MetaConsultor - v.Valor;
            }

            var viewModel = new DashboardViewModel
            {
                MetaMensal = meta,
                TotalExecutado = totalExecutado,
                MediaDiaria = mediaDiariaGeral,
                Projecao = previsaoGeral,
                Diferenca = diferenca,
                VendasConsultores = vendasPorConsultor
            };

            return View(viewModel);
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
