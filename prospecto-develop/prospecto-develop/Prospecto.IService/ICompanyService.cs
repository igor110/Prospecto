using Prospecto.Models.DTO;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using System.Collections.Generic;

namespace Prospecto.Service.Interface
{
    public interface ICompanyService : IServiceBase<CompanyDTO, CompanyInfo>
    {
        IList<CompanyViewModel> ListByFilters(CompanyFiltersViewModel filters);
    }
}
