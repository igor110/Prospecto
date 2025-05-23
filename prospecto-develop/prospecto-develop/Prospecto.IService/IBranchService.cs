using Prospecto.Models.DTO;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using System.Collections.Generic;

namespace Prospecto.Service.Interface
{
    public interface IBranchService : IServiceBase<BranchDTO, BranchInfo>
    {
        IList<BranchViewModel> ListByFilters(BranchFiltersViewModel filters);
    }
}
