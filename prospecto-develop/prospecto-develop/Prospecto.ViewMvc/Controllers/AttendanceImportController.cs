using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prospecto.Service.Interface;
using System;
using System.Threading.Tasks;

namespace Prospecto.ViewMvc.Controllers
{
    public class AttendanceImportController : ProspectoControllerBase
    {

        private readonly ILogger<AttendanceImportController> _logger;
        private readonly IAttendanceService _attendanceService;
        private readonly IMapper _mapper;

        private readonly string Title = "Importação dos Atendimentos";
        private readonly string Message = "Preencha os dados do atendimento!";

        public AttendanceImportController(
            IMapper mapper,
            IAttendanceService attendanceService,
            ILogger<AttendanceImportController> logger)
        {
            _logger = logger;
            _attendanceService = attendanceService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = Title;
            ViewData["Message"] = Message;

            return View();
        }

        public async Task<JsonResult> Upload(IFormFile file)
        {
            try
            {
                var result = await _attendanceService.ImportAttendance(file);
                return new JsonResult(new { success = true, mensagem = "Arquivo importado com sucesso!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, mensagem = "Erro ao fazer a importação do arquivo <br>" + ex.Message });
            }
        }

    }
}
