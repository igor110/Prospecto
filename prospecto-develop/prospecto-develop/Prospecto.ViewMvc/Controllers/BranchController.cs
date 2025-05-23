using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Prospecto.Models.DTO;
using Prospecto.Models.Enums;
using Prospecto.Models.ViewModel;
using Prospecto.Service.Interface;
using Prospecto.ViewMvc.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Prospecto.ViewMvc.Controllers
{
    public class BranchController : ProspectoControllerBase
    {
        private readonly ILogger<BranchController> _logger;
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;

        private readonly string Title = "Filial";
        private readonly string Message = "Preencha os dados da filial para realizar o cadastro!";

        public BranchController(
            IMapper mapper,
            IBranchService branchService,
            ICompanyService companyService,
            ILogger<BranchController> logger)
        {

            _logger = logger;
            _branchService = branchService;
            _mapper = mapper;
            _companyService = companyService;
        }

        public IActionResult Index(int? Id)
        {
            var obj = LoadIndex(Id);
            return View(obj);
        }

        public IActionResult List()
        {
            GetContextData();
            BranchFiltersViewModel filters = new();

            if (Role == UserTypeEnum.MANAGER.ToString()) filters.IdCompany = CompanyId;

            ViewData["Title"] = Title;
            var obj = _branchService.ListByFilters(filters);
            return View(obj);
        }

        public IActionResult ListPartial()
        {
            GetContextData();
            BranchFiltersViewModel filters = new();

            if (Role == UserTypeEnum.MANAGER.ToString()) filters.IdCompany = CompanyId;

            ViewData["Title"] = Title;
            var obj = _branchService.ListByFilters(filters);
            return PartialView(obj);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(BranchViewModel brancheViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<BranchDTO>(brancheViewModel);
                    if (brancheViewModel.Id > 0)
                    {
                        TempData["success"] = "Filial atualizada com sucesso!";
                        await _branchService.Update(brancheViewModel.Id, dto);
                    }
                    else
                    {
                        TempData["success"] = "Filial inserida com sucesso!";
                        await _branchService.Insert(dto);
                    }

                    TempData["success"] = "Filial salvo com sucesso!";
                    return RedirectToAction("List");
                }
                else
                {
                    var obj = LoadIndex(brancheViewModel.Id);
                    return View("Index", obj);
                }
            }
            catch (System.Exception)
            {
                return RedirectToAction("~/Shared/Error");
            }
        }

        private BranchViewModel LoadIndex(int? Id)
        {
            GetContextData();

            ViewData["Title"] = Title;
            ViewData["Message"] = Message;

            var obj = new BranchViewModel();
            if (Id > 0)
            {
                var _infoResult = _branchService.Get(Id.Value).Result;
                if (_infoResult.Success && _infoResult.Value != null)
                {
                    obj = _infoResult.Value.AsBranchViewMode();
                }
            }


            CompanyFiltersViewModel filters = new();

            if (Role == UserTypeEnum.MANAGER.ToString()) filters.Id = CompanyId;

            IList<SelectListItem> listCompany = new List<SelectListItem>();

            foreach (var item in _companyService.ListByFilters(filters))
            {
                listCompany.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Description, Selected = item.Id == obj.CompanyId });
            }
            ViewBag.ListCompany = listCompany;

            return obj;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
