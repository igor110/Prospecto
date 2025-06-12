using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Prospecto.Models.DTO;
using Prospecto.Models.Enums;
using Prospecto.Models.ViewModel;
using Prospecto.Respository.Interface;
using Prospecto.Service.Interface;
using Prospecto.ViewMvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace Prospecto.ViewMvc.Controllers
{
    public class ClientController : ProspectoControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;
        private readonly IClientRepository _clientRepository;
        private readonly string Title = "Clientes";
        private readonly string Message = "Preencha os dados do cliente para realizar o cadastro!";

        public ClientController(
            IMapper mapper,
            IClientService ClientService,
            ICompanyService companyService,
            ILogger<ClientController> logger,
            IClientRepository clientRepository)
        {
            _logger = logger;
            _clientService = ClientService;
            _mapper = mapper;
            _companyService = companyService;
            _clientRepository = clientRepository;
        }


        public IActionResult Index(int? Id)
        {
            try
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
                    var result = _clientService.Get(Id.Value).Result;

                    if (!result.Success)
                    {
                        TempData["error"] = "Cliente não encontrado.";
                        return RedirectToAction("List");
                    }

                    obj = result.Value.AsClientViewMode();

                    if (Role == UserTypeEnum.CONSULTANT.ToString() && obj.UserId != UserId)
                    {
                        TempData["error"] = "Você não tem permissão para editar esse cliente.";
                        return RedirectToAction("List");
                    }
                }

                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Erro ao carregar cliente: {ex.Message}";
                return RedirectToAction("List");
            }
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
                {
                    // Recupera o cliente atual no banco
                    var existingClient = await _clientService.Get(clientViewModel.Id);
                    if (!existingClient.Success)
                    {
                        TempData["error"] = "Cliente não encontrado.";
                        return RedirectToAction("List");
                    }

                    // Preserva o UserId já salvo (evita null ou zero)
                    dto.UserId = existingClient.Value.UserId;

                    await _clientService.Update(clientViewModel.Id, dto);
                }
                else
                {
                    if (CompanyId > 0)
                        dto.CompanyId = CompanyId;

                    if (BranchId > 0)
                        dto.BranchId = BranchId;

                    if (UserId > 0)
                        dto.UserId = UserId;

                    await _clientService.Insert(dto);
                    TempData["success"] = "Cliente salvo com sucesso!";
                }

                return RedirectToAction("List");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Shared");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public JsonResult SearchClients(string term)
        {
            try
            {
                // Aqui usamos a claim correta definida no login (SID = user.Id)
                var claim = User.FindFirst(System.Security.Claims.ClaimTypes.Sid);
                if (claim == null || !int.TryParse(claim.Value, out int userId))
                {
                    return Json(new { success = false, message = "Usuário não autenticado corretamente." });
                }

                var clients = _clientRepository
                    .ListWithRelations(c => c.UserId == userId && c.Name.Contains(term))
                    .Select(c => new {
                        id = c.Id,
                        name = c.Name,
                        telephone = c.Telephone,
                        email = c.Email,
                        cpf = c.CPF,
                        cnpj = c.CNPJ,
                        typePerson = (int)c.TypePerson,
                        address = c.Address,
                        number = c.Number,
                        complement = c.Complement,
                        zipCode = c.ZipCode,
                        neighborhood = c.Neighborhood,
                        city = c.City
                    })
                    .ToList();

                return Json(clients);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao buscar clientes: " + ex.Message });
            }
        }



    }
}
