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
    public class ConsultantController : ProspectoControllerBase
    {
        private readonly ILogger<ConsultantController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;
        private readonly IBranchService _branchService;

        private readonly string Title = "Consultor";
        private readonly string Message = "Preencha os dados do consultor para realizar o cadastro!";


        public ConsultantController(
            IMapper mapper,
            IUserService userService,
            ICompanyService companyService,
            IBranchService branchService,
            ILogger<ConsultantController> logger)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _companyService = companyService;
            _branchService = branchService;
        }

        public IActionResult Index(int? Id)
        {
            ViewData["Title"] = Title;
            ViewData["Message"] = Message;
            GetContextData();

            var obj = new UserViewModel();
            if (Id > 0)
                obj = _userService.Get(Id.Value).Result.Value.AsUserViewMode();

            IList<SelectListItem> listBranch = new List<SelectListItem>();

            foreach (var item in _branchService.ListByFilters(new BranchFiltersViewModel { IdCompany = CompanyId }))
            {
                listBranch.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Description, Selected = item.Id == obj.CompanyId });
            }

            IList<SelectListItem> listType = new List<SelectListItem>
            {
                new SelectListItem { Value = Convert.ToInt32(UserTypeEnum.MANAGER).ToString(), Text = UserType.FromUserType(UserTypeEnum.MANAGER), Selected = obj.TypeUser == UserTypeEnum.MANAGER },
                new SelectListItem { Value = Convert.ToInt32(UserTypeEnum.CONSULTANT).ToString(), Text = UserType.FromUserType(UserTypeEnum.CONSULTANT), Selected = obj.TypeUser == UserTypeEnum.CONSULTANT }

            };

            ViewBag.ListType = listType;
            ViewBag.ListBranch = listBranch;

            return View(obj);
        }

        public IActionResult List()
        {
            GetContextData();
            ViewData["Title"] = Title;
            var obj = _userService.ListByFilters(new UserFiltersViewModel { CompanyId = CompanyId, TypeUser = UserTypeEnum.CONSULTANT });
            return View(obj);
        }

        public IActionResult ListPartial()
        {
            GetContextData();
            var obj = _userService.ListByFilters(new UserFiltersViewModel { CompanyId = CompanyId, TypeUser = UserTypeEnum.CONSULTANT });
            return PartialView(obj);
        }

        public async Task<IActionResult> Save(UserViewModel userViewModel)
        {
            try
            {

                if (userViewModel.Id > 0)
                {
                    var obj = _userService.Get(userViewModel.Id).Result;
                    var dto = _mapper.Map<UserDTO>(userViewModel);
                    dto.IsActive = obj.Value.IsActive;
                    dto.BranchId = obj.Value.BranchId;
                    dto.CompanyId = obj.Value.CompanyId;
                    dto.Color = obj.Value.Color;
                    dto.TypeUser = obj.Value.TypeUser;

                    await _userService.Update(userViewModel.Id, dto);
                    TempData["success"] = "Dados do consultor atualizado com sucesso!";
                }

                return RedirectToAction("Index", new { id = userViewModel.Id });
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
    }
}
