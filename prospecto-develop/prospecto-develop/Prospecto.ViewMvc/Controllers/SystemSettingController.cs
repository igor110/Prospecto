using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prospecto.IService;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using Prospecto.ViewMvc.Extensions;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Prospecto.ViewMvc.Controllers
{
    public class SystemSettingController : ProspectoControllerBase

    {
        private readonly ISystemSettingService _service;
        private readonly IDbConnection _connection;

        public SystemSettingController(ISystemSettingService service, IDbConnection connection)
        {
            _service = service;
            _connection = connection;
        }

        public async Task<IActionResult> Index(string key)
        {
            GetContextData(); // Fundamental

            var list = await _service.ListAsync(key, CompanyId, BranchId);

            if (!list.Any() && BranchId > 0)
                list = await _service.ListAsync(key, CompanyId, null); // Busca nível empresa

            ViewBag.Key = key;
            return View(list);
        }


        public async Task<IActionResult> Form(int? id)
        {
            await LoadDropdowns();

            if (id.HasValue)
            {
                var model = await _service.GetByIdAsync(id.Value);
                return View(model);
            }

            return View(new SystemSettingViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Form(SystemSettingViewModel model)
        {
            await LoadDropdowns();

            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                await _service.CreateAsync(model);
            else
                await _service.UpdateAsync(model);

            return RedirectToAction("Index", new { key = model.Key });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        private async Task LoadDropdowns()
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            var companiesRaw = await _connection.QueryAsync("SELECT Id, Description FROM companies");
            var branchesRaw = await _connection.QueryAsync("SELECT Id, Description FROM branches");

            ViewBag.Companies = companiesRaw.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Description
            }).ToList();

            ViewBag.Branches = branchesRaw.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Description
            }).ToList();
        }
    }
}
