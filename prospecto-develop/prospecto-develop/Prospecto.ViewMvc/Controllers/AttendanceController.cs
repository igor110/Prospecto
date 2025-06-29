﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Prospecto.IService;
using Prospecto.Models.DTO;
using Prospecto.Models.Enums;
using Prospecto.Models.Request;
using Prospecto.Models.ViewModel;
using Prospecto.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ISystemSettingService _systemSettingService;
        public AttendanceController(
            IMapper mapper,
            IAttendanceService attendanceService,
            IClientService clientService,
            ISystemSettingService systemSettingService,
            ILogger<AttendanceController> logger)
        {
            _logger = logger;
            _attendanceService = attendanceService;
            _clientService = clientService;
            _mapper = mapper;
            _systemSettingService = systemSettingService;
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
                return RedirectToAction("Error", "Shared");
            }
        }

        public async Task<IActionResult> ClosePartial(AttendanceViewModel attendanceViewModel)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Shared");
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

            var filters = new AttendanceFiltersViewModel
            {
                BeginDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(30),
                TypeDate = 2, // Data de Retorno
                StatusId = (int)StatusAttendancesEnum.OPEN
            };

            if (Role != UserTypeEnum.ADMINISTRATOR.ToString())
                filters.CompanyId = CompanyId;

            if (Role == UserTypeEnum.CONSULTANT.ToString())
                filters.UserId = UserId;

            ViewBag.ListTypeDate = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Data registro", Selected = filters.TypeDate == 1 },
                new SelectListItem { Value = "2", Text = "Data retorno", Selected = filters.TypeDate == 2 },
                new SelectListItem { Value = "3", Text = "Data fechamento", Selected = filters.TypeDate == 3 }
            };

            ViewBag.TypeDate = filters.TypeDate;
            ViewBag.BeginDate = filters.BeginDate;
            ViewBag.EndDate = filters.EndDate;
            ViewBag.StatusId = filters.StatusId;
            filters.StatusId = 1;

            ViewData["Title"] = Title;

            var obj = _attendanceService.ListByFilters(filters);
            return View(obj);
        }


        public IActionResult ListPartial(AttendanceFiltersViewModel filters)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            GetContextData();

            // Corrige horário final (para incluir o dia inteiro)
            if (filters.EndDate.HasValue)
            {
                filters.EndDate = filters.EndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            }

            if (Role != UserTypeEnum.ADMINISTRATOR.ToString())
                filters.CompanyId = CompanyId;

            if (Role == UserTypeEnum.CONSULTANT.ToString())
                filters.UserId = UserId;

            // Repassa os filtros para a view (caso a tela precise recarregar valores)
            ViewBag.ListTypeDate = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Data registro", Selected = filters.TypeDate == 1 },
                new SelectListItem { Value = "2", Text = "Data retorno", Selected = filters.TypeDate == 2 },
                new SelectListItem { Value = "3", Text = "Data fechamento", Selected = filters.TypeDate == 3 }
            };

            ViewBag.TypeDate = filters.TypeDate;
            ViewBag.BeginDate = filters.BeginDate;
            ViewBag.EndDate = filters.EndDate;
            ViewBag.StatusId = filters.StatusId;

            ViewData["Title"] = Title;

            var result = _attendanceService.ListByFilters(filters);
            return View("List", result); // Recarrega a view completa com os filtros aplicados
        }


        public async Task<IActionResult> ReschedulePartial(AttendanceViewModel attendanceViewModel)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Shared");
            }
            attendanceViewModel.Id = await SaveAttendance(attendanceViewModel);
            return RedirectToAction("Reschedule", "Attendance", new { attendanceViewModel.Id });
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveReschedule(AttendanceViewModel AttendanceViewModel)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            var date = AttendanceViewModel.DateReturn;
            if (AttendanceViewModel.TimeReturn.HasValue)
                date = date.Date.Add(AttendanceViewModel.TimeReturn.Value);
            var result = await _attendanceService.Reschedule(AttendanceViewModel.Id, date, AttendanceViewModel.TimeReturn);
            TempData["success"] = "Atendimento reagendado com sucesso!";
            return RedirectToAction("List", "Attendance");
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveClose(AttendanceViewModel AttendanceViewModel)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            GetContextData();
            var obj = _attendanceService.GetWithRelations(AttendanceViewModel.Id);
            var dto = _mapper.Map<AttendanceDTO>(obj);
            // Atualiza os dados do atendimento
            if (AttendanceViewModel.StatusOrder > 0)
            {
                dto.Status = StatusAttendancesEnum.CLOSED;
                dto.StatusOrder = AttendanceViewModel.StatusOrder;
                dto.DateClosed = AttendanceViewModel.DateClosed;
                dto.ValueClosed = AttendanceViewModel.ValueClosed;
            }
            // Verifica se é um cliente existente (autocomplete selecionado)
            int? clientId = AttendanceViewModel.Client?.Id;
            if (clientId.HasValue && clientId.Value > 0)
            {
                dto.ClientId = clientId;
            }
            else
            {
                var dtoclient = _mapper.Map<ClientDTO>(AttendanceViewModel.Client ?? new());

                if (!string.IsNullOrEmpty(dtoclient.CPF))
                    dtoclient.TypePerson = ClientTypePersonEnum.PHYSICAL;
                else
                    dtoclient.TypePerson = ClientTypePersonEnum.LEGAL;

                dtoclient.CompanyId = dto.CompanyId;
                dtoclient.BranchId = dto.BranchId == 0 ? null : dto.BranchId;
                dtoclient.UserId = dto.UserId;

                var result = await _clientService.Insert(dtoclient);
                if (result.Success && result.Value != null)
                {
                    dto.ClientId = result.Value;
                }
                else
                {
                    TempData["error"] = "Erro ao salvar o cliente.";
                    return RedirectToAction("List", "Attendance");
                }
            }
            // Finaliza o atendimento
            if (dto.BranchId == 0)
                dto.BranchId = null;
            await _attendanceService.Update(AttendanceViewModel.Id, dto);
            TempData["success"] = "Atendimento fechado com sucesso!";
            return RedirectToAction("List", "Attendance");
        }

        public async Task<IActionResult> Kanban(AttendanceFiltersViewModel filters)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            GetContextData();

            ViewData["Title"] = "Kanban de Atendimentos";
            ViewData["Message"] = "Organize seus atendimentos de forma visual.";

            filters.BeginDate ??= DateTime.Today;
            filters.EndDate ??= DateTime.Today.AddDays(30);
            filters.EndDate = filters.EndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            filters.TypeDate = filters.TypeDate == 0 ? 2 : filters.TypeDate;
            if (Role != UserTypeEnum.ADMINISTRATOR.ToString())
                filters.CompanyId = CompanyId;
            filters.BranchId = BranchId;

            if (Role == UserTypeEnum.CONSULTANT.ToString())
                filters.UserId = UserId;

            // 🔒 Garante que só atendimentos abertos serão carregados
            filters.StatusId = (int)StatusAttendancesEnum.OPEN;

            var atendimentos = _attendanceService.ListByFilters(filters);

            var statusList = await _systemSettingService.ListAsync("kanban-status", CompanyId, BranchId);
            if (!statusList.Any() && BranchId > 0)
                statusList = await _systemSettingService.ListAsync("kanban-status", CompanyId, null);

            var statusEtapas = statusList
                .FirstOrDefault()?.Value?
                .Split(',')?
                .Select(s => s.Trim())
                .ToList() ?? new List<string>();

            foreach (var item in atendimentos)
            {
                if (item.StatusKanban is null || item.StatusKanban < 0 || item.StatusKanban >= statusEtapas.Count)
                    item.StatusKanban = null;
            }

            ViewBag.listTypeDate = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Data registro", Selected = filters.TypeDate == 1 },
                new SelectListItem { Value = "2", Text = "Data retorno", Selected = filters.TypeDate == 2 }
                //new SelectListItem { Value = "3", Text = "Data fechamento", Selected = filters.TypeDate == 3 }
            };

            ViewBag.KanbanStatus = statusEtapas;
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
                    return RedirectToAction("Error", "Shared");
                }
                attendanceViewModel.Id = await SaveAttendance(attendanceViewModel);
                TempData["success"] = "Atendimento salvo com sucesso!";
                //return RedirectToAction("Index", "Attendance", new { attendanceViewModel.Id }); PERMANECIA EM EDIÇÃO COM O ULTIMO ATENDIMENTO
                return RedirectToAction("List", "Attendance");

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        private async Task<int> SaveAttendance(AttendanceViewModel AttendanceViewModel)
        {
            try
            {
                GetContextData();
                if (AttendanceViewModel.Status == 0)
                    AttendanceViewModel.Status = StatusAttendancesEnum.OPEN;
                // Junta data e hora de retorno
                if (AttendanceViewModel.DateReturn != DateTime.MinValue && AttendanceViewModel.TimeReturn != null)
                {
                    AttendanceViewModel.DateReturn = AttendanceViewModel.DateReturn.Date.Add(AttendanceViewModel.TimeReturn.Value);
                }
                var dto = _mapper.Map<AttendanceDTO>(AttendanceViewModel);
                // Resto do código (insert/update)...
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
        public async Task<JsonResult> UpdateStatus([FromBody] KanbanUpdateRequest request)
        {
            try
            {
                GetContextData();

                var statusList = await _systemSettingService.ListAsync("kanban-status", CompanyId, BranchId);
                if (!statusList.Any() && BranchId > 0)
                    statusList = await _systemSettingService.ListAsync("kanban-status", CompanyId, null);

                string Normalize(string value) => value?.Trim().ToLower().Replace("ç", "c").Replace("á", "a").Replace("é", "e");

                var statusEtapas = statusList
                    .FirstOrDefault()?.Value?
                    .Split(',')?
                    .Select(s => Normalize(s))
                    .ToList() ?? new List<string>();

                var normalizedStatus = Normalize(request.Status);

                var newStatusIndex = statusEtapas
                    .Select((s, i) => new { Name = s, Index = i })
                    .FirstOrDefault(x => x.Name == normalizedStatus)?.Index ?? -1;


                if (newStatusIndex == -1)
                    return Json(new { success = false, message = $"Status inválido: '{request.Status}'" });

                var atendimento = _attendanceService.GetWithRelations(request.Id);
                if (atendimento == null)
                    return Json(new { success = false, message = "Atendimento não encontrado." });

                atendimento.StatusKanban = newStatusIndex;

                if (atendimento.BranchId == 0)
                    atendimento.BranchId = null;

                var dto = _mapper.Map<AttendanceDTO>(atendimento);
                await _attendanceService.Update(request.Id, dto);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Erro completo: {ex.Message}\n\nStackTrace: {ex.StackTrace}\n\nInner: {(ex.InnerException?.Message ?? "sem inner exception")}"

                });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/attendance")]
        public async Task<JsonResult> CreateFromMobile([FromBody] AttendanceMobileViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Dados inválidos." });

            try
            {
                var entity = new AttendanceViewModel
                {
                    NameClient = model.NameClient,
                    Telephone = model.Telephone,
                    NameProduct = model.Product,
                    Value = model.Value,
                    Observation = model.Observation,
                    DateRegistred = DateTime.Now,
                    DateReturn = DateTime.Parse($"{model.DateReturn} {model.TimeReturn}"),
                    TimeReturn = TimeSpan.Parse(model.TimeReturn),
                    NotifyAt = model.Notify ? DateTime.Parse($"{model.DateReturn} {model.TimeReturn}") : (DateTime?)null,
                    Status = StatusAttendancesEnum.OPEN,
                    StatusOrder = StatusOrderEnum.GAIN,
                    ReschedulingOrigin = ParseReschedulingOrigin(model.Source),
                    UserId = 10,       // Ajustar futuramente com base no usuário logado
                    CompanyId = 2,    // Ajustar dinamicamente se necessário
                    BranchId = 1      // Ajustar conforme necessário
                };

                var result = await _attendanceService.InsertFromMobile(entity);

                if (!result.Success)
                    return Json(new { success = false, message = result.ErrorId ?? "Erro ao salvar atendimento." });

                return Json(new { success = true, message = "Atendimento salvo com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro interno: {ex.Message}" });
            }
        }


        private ReschedulingOriginEnum ParseReschedulingOrigin(string source)
        {
            return source?.Trim().ToLower() switch
            {
                var s when s == ReschedulingOrigin.EMAIL.ToLower() => ReschedulingOriginEnum.EMAIL,
                var s when s == ReschedulingOrigin.TELEPHONE.ToLower() => ReschedulingOriginEnum.TELEPHONE,
                var s when s == ReschedulingOrigin.STORE.ToLower() => ReschedulingOriginEnum.STORE,
                var s when s == ReschedulingOrigin.INTERNET.ToLower() => ReschedulingOriginEnum.INTERNET,
                var s when s == ReschedulingOrigin.PROSPECTION.ToLower() => ReschedulingOriginEnum.PROSPECTION,
                var s when s == ReschedulingOrigin.IMPORT.ToLower() => ReschedulingOriginEnum.IMPORT,
                _ => ReschedulingOriginEnum.TELEPHONE // padrão de fallback
            };
        }



    }
}

