using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Prospecto.IRespository;
using Prospecto.IService;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prospecto.Service
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly ISystemSettingRepository _repository;

        public SystemSettingService(ISystemSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SystemSettingInfo>> ListAsync(string keyFilter, int? companyId, int? branchId)
        {
            var results = await _repository.ListAsync(keyFilter, companyId, branchId);

            if (!results.Any() && branchId.HasValue)
                results = await _repository.ListAsync(keyFilter, companyId, null);

            return results;
        }

        public async Task<IEnumerable<SystemSettingInfo>> ListAllAsync(int companyId, int? branchId)
        {
            return await _repository.ListAllAsync(companyId, branchId);
        }

        public async Task<SystemSettingViewModel> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(SystemSettingViewModel model)
        {
            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            await _repository.CreateAsync(model);
        }

        public async Task UpdateAsync(SystemSettingViewModel model)
        {
            model.UpdatedAt = DateTime.Now;
            await _repository.UpdateAsync(model);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
