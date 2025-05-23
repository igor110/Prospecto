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
    public class ClientController : ProspectoControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;

        private readonly string Title = "Clientes";
        private readonly string Message = "Preencha os dados do cliente para realizar o cadastro!";

        public ClientController(
            IMapper mapper,
            IClientService ClientService,
            ICompanyService companyService,
            ILogger<ClientController> logger)
        {
            _logger = logger;
            _clientService = ClientService;
            _mapper = mapper;
            _companyService = companyService;
        }
        public IActionResult Index(int? Id)
        {

            GetContextData();

            ViewData["Title"] = Title;
            ViewData["Message"] = Message;

            var obj = new ClientViewModel
            {
                TypePerson = ClientTypePersonEnum.PHYSICAL
            };

            if (Id > 0)
            {
                obj = _clientService.Get(Id.Value).Result.Value.AsClientViewMode();
                if (Role != UserTypeEnum.ADMINISTRATOR.ToString())
                {
                    TempData["error"] = "Não é possivel visualizar os dados desse cliente! Pertece a outro usuário.";
                    if (obj.CompanyId != CompanyId) return RedirectToAction("List");
                }
            }

            IList<SelectListItem> listTypePerson = new List<SelectListItem>
                {
                    new SelectListItem { Value = Convert.ToInt32(ClientTypePersonEnum.PHYSICAL).ToString(), Text = ClientTypePerson.FromClientTypePerson(ClientTypePersonEnum.PHYSICAL), Selected = obj.Id <= 0 || obj.TypePerson == ClientTypePersonEnum.PHYSICAL },
                    new SelectListItem { Value = Convert.ToInt32(ClientTypePersonEnum.LEGAL).ToString(), Text = ClientTypePerson.FromClientTypePerson(ClientTypePersonEnum.LEGAL), Selected = obj.Id > 0 && obj.TypePerson == ClientTypePersonEnum.LEGAL }
                };

            ViewBag.ListTypePerson = listTypePerson;

            return View(obj);
        }

        public IActionResult List()
        {
            GetContextData();
            ViewData["Title"] = Title;
            var filters = new ClientFiltersViewModel { CompanyId = CompanyId, BranchId = BranchId };
            if (Role != UserTypeEnum.ADMINISTRATOR.ToString()) filters.UserId = UserId;
            var obj = _clientService.ListByFilters(filters);
            return View(obj);
        }

        public IActionResult ListPartial()
        {
            GetContextData();
            var obj = _clientService.ListByFilters(new ClientFiltersViewModel { CompanyId = CompanyId, BranchId = BranchId });
            return PartialView(obj);
        }

        public async Task<IActionResult> Save(ClientViewModel clientViewModel)
        {
            try
            {
                GetContextData();
                var dto = _mapper.Map<ClientDTO>(clientViewModel);
                if (clientViewModel.Id > 0)
                    await _clientService.Update(clientViewModel.Id, dto);
                else
                {
                    if (CompanyId > 0)
                        dto.CompanyId = CompanyId;

                    if (BranchId > 0)
                        dto.BranchId = BranchId;

                    if (UserId > 0) dto.UserId = UserId;

                    await _clientService.Insert(dto);
                    TempData["success"] = "Cliente salvo com sucesso!";
                }

                return RedirectToAction("List");
            }
            catch (Exception)
            {
                return RedirectToAction("~/Shared/Error");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
