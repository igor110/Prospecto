using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prospecto.Models.DTO;
using Prospecto.Models.ViewModel;
using Prospecto.Service.Interface;
using Prospecto.ViewMvc.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Prospecto.ViewMvc.Controllers
{

    public class CompanyController : ProspectoControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        private readonly string Title = "Empresa";
        private readonly string Message = "Preencha os dados da empresa para realizar o cadastro!";

        public CompanyController(
            IMapper mapper,
            ICompanyService companyService,
            ILogger<CompanyController> logger)
        {

            _logger = logger;
            _companyService = companyService;
            _mapper = mapper;
        }

        //[Authorize(Roles = Policies.ADMINISTRATORS)]        
        public IActionResult Index(int? Id)
        {
            ViewData["Title"] = Title;
            ViewData["Message"] = Message;

            var obj = new CompanyViewModel();
            if (Id > 0)
            {
                var _infoResult = _companyService.Get(Id.Value).Result;
                if (_infoResult.Success && _infoResult.Value != null)
                {
                    obj = _infoResult.Value.AsCompanyViewMode();
                }
            }

            return View(obj);
        }

        public IActionResult List()
        {
            ViewData["Title"] = Title;
            var obj = _companyService.ListByFilters(new CompanyFiltersViewModel { });
            return View(obj);
        }

        public IActionResult ListPartial()
        {
            ViewData["Title"] = Title;
            var obj = _companyService.ListByFilters(new CompanyFiltersViewModel { });
            return PartialView(obj);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CompanyViewModel companyViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<CompanyDTO>(companyViewModel);
                    if (companyViewModel.Id > 0)
                    {
                        TempData["success"] = "Empresa atualizada com sucesso!";
                        await _companyService.Update(companyViewModel.Id, dto);
                    }
                    else
                    {
                        TempData["success"] = "Empresa salvo com sucesso!";
                        await _companyService.Insert(dto);
                    }

                    return RedirectToAction("List");
                }
                else
                {
                    return View("Index", companyViewModel);
                }
            }
            catch (System.Exception)
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
