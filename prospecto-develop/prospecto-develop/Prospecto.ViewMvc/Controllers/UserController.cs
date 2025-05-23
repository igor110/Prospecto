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
    public class UserController : ProspectoControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;
        private readonly IBranchService _branchService;

        private readonly string Title = "Usuários";
        private readonly string Message = "Preencha os dados do usuário para realizar o cadastro!";


        public UserController(
            IMapper mapper,
            IUserService userService,
            ICompanyService companyService,
            IBranchService branchService,
            ILogger<UserController> logger)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _companyService = companyService;
            _branchService = branchService;
        }

        private UserViewModel LoadIndex(int? Id)
        {

            ViewData["Title"] = Title;
            ViewData["Message"] = Message;

            GetContextData();

            var obj = new UserViewModel();


            if (Id > 0)
                obj = _userService.Get(Id.Value).Result.Value.AsUserViewMode();

            IList<SelectListItem> listCompany = new List<SelectListItem>();
            CompanyFiltersViewModel filters = new();

            if (Role == UserTypeEnum.MANAGER.ToString()) filters.Id = CompanyId;
            if (Role == UserTypeEnum.ADMINISTRATOR.ToString()) listCompany.Add(new SelectListItem { Text = "Selecione uma empresa" });

            foreach (var itemCompany in _companyService.ListByFilters(filters))
            {
                listCompany.Add(new SelectListItem
                {
                    Value = itemCompany.Id.ToString(),
                    Text = itemCompany.Description,
                    Selected = itemCompany.Id == obj.CompanyId
                });
            }

            IList<SelectListItem> listType = new List<SelectListItem>();
            if (Role == UserTypeEnum.ADMINISTRATOR.ToString())
            {
                listType.Add(new SelectListItem { Value = Convert.ToInt32(UserTypeEnum.ADMINISTRATOR).ToString(), Text = UserType.FromUserType(UserTypeEnum.ADMINISTRATOR) });
            }

            if (Role == UserTypeEnum.ADMINISTRATOR.ToString() || Role == UserTypeEnum.MANAGER.ToString())
            {
                listType.Add(new SelectListItem { Value = Convert.ToInt32(UserTypeEnum.MANAGER).ToString(), Text = UserType.FromUserType(UserTypeEnum.MANAGER) });
                listType.Add(new SelectListItem { Value = Convert.ToInt32(UserTypeEnum.CONSULTANT).ToString(), Text = UserType.FromUserType(UserTypeEnum.CONSULTANT) });
            }

            IList<SelectListItem> listBranch = new List<SelectListItem>();
            BranchFiltersViewModel filtersBranch = new();
            if (Role == UserTypeEnum.MANAGER.ToString()) filtersBranch.IdCompany = CompanyId;

            foreach (var item in _branchService.ListByFilters(filtersBranch))
            {
                listBranch.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Description, Selected = item.Id == obj.BranchId });
            }

            IList<SelectListItem> listActive = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Ativo", Selected = obj.Id <= 0 || obj.IsActive },
                new SelectListItem { Value = "false", Text = "Inativo", Selected = obj.Id > 0 && obj.IsActive }
            };

            ViewBag.ListType = listType;
            ViewBag.ListCompany = listCompany;
            ViewBag.ListBranch = listBranch;
            ViewBag.ListActive = listActive;

            return obj;

        }

        public IActionResult Index(int? Id)
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            var obj = LoadIndex(Id);
            return View(obj);
        }

        public IActionResult List()
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            GetContextData();
            UserFiltersViewModel filters = new();

            if (Role == UserTypeEnum.MANAGER.ToString()) filters.CompanyId = CompanyId;

            ViewData["Title"] = Title;
            var obj = _userService.ListByFilters(filters);
            return View(obj);
        }

        public IActionResult ListPartial()
        {
            if (!IsAuthenticate()) return Redirect("/Account/Login");
            GetContextData();
            UserFiltersViewModel filters = new();

            if (Role == UserTypeEnum.MANAGER.ToString()) filters.CompanyId = CompanyId;

            var obj = _userService.ListByFilters(filters);
            return PartialView(obj);
        }

        public async Task<IActionResult> Save(UserViewModel userViewModel)
        {
            try
            {
                if (!IsAuthenticate())
                    return Redirect("/Account/Login");

                var dto = _mapper.Map<UserDTO>(userViewModel);
                await _userService.Save(userViewModel.Id, dto);
                TempData["success"] = "Usuário salvo com sucesso!";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                var obj = LoadIndex(userViewModel.Id);
                obj.Name = userViewModel.Name;
                obj.Email = userViewModel.Email;
                obj.Password = userViewModel.Password;
                obj.Color = userViewModel.Color;

                TempData["error"] = ex.Message;
                return View("Index", obj);
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
