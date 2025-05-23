using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Prospecto.Models.DTO;
using Prospecto.Models.Enums;
using Prospecto.Models.ViewModel;
using Prospecto.Service.Interface;
using Prospecto.ViewMvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Prospecto.ViewMvc.Controllers
{

    public class AttendanceController : ProspectoControllerBase
    {
        private readonly ILogger<AttendanceController> _logger;
        private readonly IAttendanceService _attendanceService;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        private readonly string Title = "Atendimentos";
        private readonly string Message = "Preencha os dados do atendimento!";

        public AttendanceController(
            IMapper mapper,
            IAttendanceService attendanceService,
            IClientService clientService,
            ILogger<AttendanceController> logger)
        {
            _logger = logger;
            _attendanceService = attendanceService;
            _clientService = clientService;
            _mapper = mapper;
        }

        public IActionResult Index(int? Id)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");

            GetContextData();

            ViewData["Title"] = Title;
            ViewData["Message"] = Message;

            var obj = new AttendanceViewModel();
            if (Id > 0)
                obj = _attendanceService.GetWithRelations(Id.Value);

            IList<SelectListItem> listStatusClosed = new List<SelectListItem>
            {
                new SelectListItem { Value = Convert.ToInt32(ReschedulingOriginEnum.EMAIL).ToString(), Text = ReschedulingOrigin.FromReschedulingOrigin(ReschedulingOriginEnum.EMAIL) },
                new SelectListItem { Value = Convert.ToInt32(ReschedulingOriginEnum.TELEPHONE).ToString(), Text = ReschedulingOrigin.FromReschedulingOrigin(ReschedulingOriginEnum.TELEPHONE) },
                new SelectListItem { Value = Convert.ToInt32(ReschedulingOriginEnum.STORE).ToString(), Text = ReschedulingOrigin.FromReschedulingOrigin(ReschedulingOriginEnum.STORE) },
                new SelectListItem { Value = Convert.ToInt32(ReschedulingOriginEnum.INTERNET).ToString(), Text = ReschedulingOrigin.FromReschedulingOrigin(ReschedulingOriginEnum.INTERNET) },
                new SelectListItem { Value = Convert.ToInt32(ReschedulingOriginEnum.PROSPECTION).ToString(), Text = ReschedulingOrigin.FromReschedulingOrigin(ReschedulingOriginEnum.PROSPECTION) }
            };

            ViewBag.ListSource = listStatusClosed;

            return View(obj);
        }

        public IActionResult Delete(int Id)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");

            GetContextData();

            ViewData["Title"] = Title;
            ViewData["Message"] = Message;

            var obj = _attendanceService.GetWithRelations(Id);

            IList<SelectListItem> listStatusClosed = new List<SelectListItem>
            {
                new SelectListItem { Value = Convert.ToInt32(ReschedulingOriginEnum.EMAIL).ToString(), Text = ReschedulingOrigin.FromReschedulingOrigin(ReschedulingOriginEnum.EMAIL) },
                new SelectListItem { Value = Convert.ToInt32(ReschedulingOriginEnum.TELEPHONE).ToString(), Text = ReschedulingOrigin.FromReschedulingOrigin(ReschedulingOriginEnum.TELEPHONE) },
                new SelectListItem { Value = Convert.ToInt32(ReschedulingOriginEnum.STORE).ToString(), Text = ReschedulingOrigin.FromReschedulingOrigin(ReschedulingOriginEnum.STORE) },
                new SelectListItem { Value = Convert.ToInt32(ReschedulingOriginEnum.INTERNET).ToString(), Text = ReschedulingOrigin.FromReschedulingOrigin(ReschedulingOriginEnum.INTERNET) },
                new SelectListItem { Value = Convert.ToInt32(ReschedulingOriginEnum.PROSPECTION).ToString(), Text = ReschedulingOrigin.FromReschedulingOrigin(ReschedulingOriginEnum.PROSPECTION) }
            };

            ViewBag.ListSource = listStatusClosed;

            return View(obj);
        }

        public IActionResult ConfirmDelete(int Id)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            GetContextData();

            ViewData["Title"] = Title;
            ViewData["Message"] = Message;

            try
            {
                _attendanceService.Delete(Id);
                TempData["success"] = "Atendimento excluído com sucesso!";
                return RedirectToAction("List", "Attendance");
            }
            catch (Exception)
            {
                return RedirectToAction("~/Shared/Error");
            }
        }

        public async Task<IActionResult> ClosePartial(AttendanceViewModel attendanceViewModel)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("~/Shared/Error");
            }

            attendanceViewModel.Id = await SaveAttendance(attendanceViewModel);
            return RedirectToAction("Close", "Attendance", new { attendanceViewModel.Id });
        }

        public IActionResult Close(int Id)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            return View(LoadClose(Id));
        }

        private AttendanceViewModel LoadClose(int Id)
        {
            GetContextData();
            ViewData["Title"] = Title;
            ViewData["Message"] = Message;

            var obj = _attendanceService.GetWithRelations(Id);
            IList<SelectListItem> listStatusClosed = new List<SelectListItem>
                {
                    new SelectListItem { Value = Convert.ToInt32(StatusOrderEnum.GAIN).ToString(), Text = StatusOrder.FromStatusOrder(StatusOrderEnum.GAIN), Selected = obj.StatusOrder == StatusOrderEnum.GAIN },
                    new SelectListItem { Value = Convert.ToInt32(StatusOrderEnum.LOSS).ToString(), Text = StatusOrder.FromStatusOrder(StatusOrderEnum.LOSS), Selected = obj.StatusOrder == StatusOrderEnum.LOSS }
                };

            obj.Client.Name = obj.NameClient;
            obj.Client.Telephone = obj.Telephone;
            obj.Client.TypePerson = ClientTypePersonEnum.PHYSICAL;

            IList<SelectListItem> listTypePerson = new List<SelectListItem>
                {
                    new SelectListItem { Value = Convert.ToInt32(ClientTypePersonEnum.PHYSICAL).ToString(), Text = ClientTypePerson.FromClientTypePerson(ClientTypePersonEnum.PHYSICAL), Selected = obj.Id <= 0 || obj.Client.TypePerson == ClientTypePersonEnum.PHYSICAL },
                    new SelectListItem { Value = Convert.ToInt32(ClientTypePersonEnum.LEGAL).ToString(), Text = ClientTypePerson.FromClientTypePerson(ClientTypePersonEnum.LEGAL), Selected = obj.Id > 0 && obj.Client.TypePerson == ClientTypePersonEnum.LEGAL }
                };


            ViewBag.ListTypePerson = listTypePerson;

            ViewBag.ListStatusClosed = listStatusClosed;
            return obj;
        }

        public IActionResult Reschedule(int Id)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");

            GetContextData();

            ViewData["Title"] = Title;
            ViewData["Message"] = Message;

            var obj = _attendanceService.GetWithRelations(Id);
            return View(obj);
        }

        public IActionResult List()
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            GetContextData();
            AttendanceFiltersViewModel filters = new();

            IList<SelectListItem> listTypeDate = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Data registro" },
                new SelectListItem { Value = "2", Text = "Data retorno" },
                new SelectListItem { Value = "3", Text = "Data fechamento" }
            };

            ViewBag.ListTypeDate = listTypeDate;

            filters.BeginDate = DateTime.Now.AddMonths(-1);
            filters.EndDate = DateTime.Now;
            filters.TypeDate = 2;

            ViewBag.BeginDate = filters.BeginDate;
            ViewBag.EndDate = filters.EndDate;

            if (Role != UserTypeEnum.ADMINISTRATOR.ToString()) filters.CompanyId = CompanyId;

            if (Role == UserTypeEnum.CONSULTANT.ToString())
                filters.UserId = UserId;

            ViewData["Title"] = Title;
            var obj = _attendanceService.ListByFilters(filters);
            return View(obj);
        }

        public IActionResult ListPartial(AttendanceFiltersViewModel filters)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            GetContextData();
            if (Role != UserTypeEnum.ADMINISTRATOR.ToString()) filters.CompanyId = CompanyId;

            if (Role == UserTypeEnum.CONSULTANT.ToString())
                filters.UserId = UserId;

            IList<SelectListItem> listTypeDate = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Data registro" },
                new SelectListItem { Value = "2", Text = "Data retorno" },
                new SelectListItem { Value = "3", Text = "Data fechamento" }
            };

            ViewBag.ListTypeDate = listTypeDate;

            ViewData["Title"] = Title;
            ViewBag.BeginDate = filters.BeginDate;
            filters.EndDate = new DateTime(filters.EndDate.Value.Year, filters.EndDate.Value.Month, filters.EndDate.Value.Day, 23, 59, 59);
            ViewBag.EndDate = filters.EndDate;

            var obj = _attendanceService.ListByFilters(filters);
            return View("List", obj);
        }

        public async Task<IActionResult> ReschedulePartial(AttendanceViewModel attendanceViewModel)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            if (!ModelState.IsValid)
            {
                return RedirectToAction("~/Shared/Error");
            }

            attendanceViewModel.Id = await SaveAttendance(attendanceViewModel);
            return RedirectToAction("Reschedule", "Attendance", new { attendanceViewModel.Id });
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveReschedule(AttendanceViewModel AttendanceViewModel)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            var result = await _attendanceService.Reschedule(AttendanceViewModel.Id, AttendanceViewModel.DateReturn);
            TempData["success"] = "Atendimento reagendado com sucesso!";
            return RedirectToAction("Index", "Schedule");

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveClose(AttendanceViewModel AttendanceViewModel)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            GetContextData();

            var obj = _attendanceService.GetWithRelations(AttendanceViewModel.Id);
            var dto = _mapper.Map<AttendanceDTO>(obj);

            if (AttendanceViewModel.StatusOrder > 0)
            {
                dto.Status = StatusAttendancesEnum.CLOSED;
                dto.StatusOrder = AttendanceViewModel.StatusOrder;
                dto.DateClosed = AttendanceViewModel.DateClosed;
                dto.ValueClosed = AttendanceViewModel.ValueClosed;
            }

            var dtoclient = _mapper.Map<ClientDTO>(AttendanceViewModel.Client);
            if (!string.IsNullOrEmpty(dtoclient.CPF)) dtoclient.TypePerson = ClientTypePersonEnum.PHYSICAL;
            else dtoclient.TypePerson = ClientTypePersonEnum.LEGAL;

            dtoclient.CompanyId = dto.CompanyId;
            dtoclient.BranchId = dto.BranchId;
            dtoclient.UserId = dto.UserId;
            if (dtoclient.BranchId == 0) dtoclient.BranchId = null;

            var result = await _clientService.Insert(dtoclient);
            if (result.Success)
            {
                dto.ClientId = result.Value;
                if (dto.BranchId == 0) dto.BranchId = null;
                await _attendanceService.Update(AttendanceViewModel.Id, dto);
            }

            TempData["success"] = "Atendimento fechado com sucesso!";
            return RedirectToAction("Index", "Schedule");

        }
        public IActionResult Kanban(AttendanceFiltersViewModel filters)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            GetContextData();

            ViewData["Title"] = "Kanban de Atendimentos";
            ViewData["Message"] = "Organize seus atendimentos de forma visual.";

            // Lista de tipo de datas para o filtro
            ViewBag.ListTypeDate = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "Data registro", Selected = filters.TypeDate == 1 },
        new SelectListItem { Value = "2", Text = "Data retorno", Selected = filters.TypeDate == 2 },
        new SelectListItem { Value = "3", Text = "Data fechamento", Selected = filters.TypeDate == 3 }
    };

            // Lista de status para Kanban e Filtro
            ViewBag.ListStatus = new List<SelectListItem>
    {
        new SelectListItem { Value = StatusAttendancesEnum.OPEN.ToString(), Text = StatusAttendancesEnum.OPEN.ToString(), Selected = filters.Status.ToString() == StatusAttendancesEnum.OPEN.ToString() },
        new SelectListItem { Value = StatusAttendancesEnum.RESCHEDULED.ToString(), Text = StatusAttendancesEnum.RESCHEDULED.ToString(), Selected = filters.Status.ToString() == StatusAttendancesEnum.RESCHEDULED.ToString() },
        new SelectListItem { Value = StatusAttendancesEnum.CLOSED.ToString(), Text = StatusAttendancesEnum.CLOSED.ToString(), Selected = filters.Status.ToString() == StatusAttendancesEnum.CLOSED.ToString() }
    };

            // Define datas padrão se não preenchidas
            if (filters.BeginDate == null)
                filters.BeginDate = DateTime.Now.AddMonths(-1);

            if (filters.EndDate == null)
                filters.EndDate = DateTime.Now;

            filters.EndDate = new DateTime(
                filters.EndDate.Value.Year,
                filters.EndDate.Value.Month,
                filters.EndDate.Value.Day,
                23, 59, 59
            );

            // Filtra por perfil logado
            if (Role != UserTypeEnum.ADMINISTRATOR.ToString())
                filters.CompanyId = CompanyId;

            if (Role == UserTypeEnum.CONSULTANT.ToString())
                filters.UserId = UserId;

            // Busca os atendimentos filtrados
            var atendimentos = _attendanceService.ListByFilters(filters);

            // Envia dados para a View
            ViewBag.BeginDate = filters.BeginDate;
            ViewBag.EndDate = filters.EndDate;
            ViewBag.TypeDate = filters.TypeDate;

            return View(atendimentos);
        }



        public async Task<IActionResult> Save(AttendanceViewModel attendanceViewModel)
        {
            try
            {
                if (!IsAuthenticate()) return Redirect("/Account/Login");
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("~/Shared/Error");
                }

                attendanceViewModel.Id = await SaveAttendance(attendanceViewModel);
                TempData["success"] = "Atendimento salvo com sucesso!";
                return RedirectToAction("Index", "Attendance", new { attendanceViewModel.Id });
            }
            catch (Exception)
            {
                return RedirectToAction("~/Shared/Error");
            }
        }

        private async Task<int> SaveAttendance(AttendanceViewModel AttendanceViewModel)
        {
            try
            {
                GetContextData();
                if (AttendanceViewModel.Status == 0)
                    AttendanceViewModel.Status = StatusAttendancesEnum.OPEN;

                var dto = _mapper.Map<AttendanceDTO>(AttendanceViewModel);
                if (AttendanceViewModel.Id > 0)
                {
                    var result = await _attendanceService.Get(AttendanceViewModel.Id);
                    if (result.Success)
                    {
                        var attendence = result.Value;
                        dto.UserId = attendence.UserId;
                        dto.CompanyId = attendence.CompanyId.Value;
                        dto.BranchId = attendence.BranchId;

                        await _attendanceService.Update(AttendanceViewModel.Id, dto);
                    }
                }
                else
                {
                    dto.UserId = UserId;
                    dto.CompanyId = CompanyId;
                    dto.BranchId = BranchId;

                    if (dto.BranchId == 0) dto.BranchId = null;

                    dto.DateRegistred = DateTime.Now;
                    var result = await _attendanceService.Insert(dto);
                    if (result.Success)
                    {
                        AttendanceViewModel.Id = result.Value;
                    }
                }

                return AttendanceViewModel.Id;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        [HttpPost]
        public JsonResult UpdateStatus(int id, string status)
        {
            try
            {
                var mapStatus = status switch
                {
                    "Novo" => StatusAttendancesEnum.OPEN,
                    "Em Atendimento" => StatusAttendancesEnum.RESCHEDULED,
                    "Aguardando Retorno" => StatusAttendancesEnum.WAITING,
                    "Concluído" => StatusAttendancesEnum.CLOSED,
                    _ => StatusAttendancesEnum.OPEN
                };

                var atendimento = _attendanceService.GetWithRelations(id);
                if (atendimento != null)
                {
                    atendimento.Status = mapStatus;

                    // Faz o mapeamento do ViewModel para DTO
                    var dto = _mapper.Map<AttendanceDTO>(atendimento);

                    _attendanceService.Update(id, dto);

                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Atendimento não encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
