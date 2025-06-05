using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prospecto.IService
{
    public interface ISystemSettingService
    {
        Task<IEnumerable<SystemSettingInfo>> ListAsync(string keyFilter, int? companyId, int? branchId);
        Task<SystemSettingViewModel> GetByIdAsync(int id);
        Task CreateAsync(SystemSettingViewModel model);
        Task UpdateAsync(SystemSettingViewModel model);
        Task DeleteAsync(int id);
        Task<IEnumerable<SystemSettingInfo>> ListAllAsync(int companyId, int? branchId);


    }
}
