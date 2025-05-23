using AutoMapper;
using Prospecto.Models.DTO;
using Prospecto.Models.Extensions;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using Prospecto.Respository.Interface;
using Prospecto.Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Prospecto.Service
{
    public class CompanyService : AbstractServiceBase<CompanyDTO, CompanyInfo, ICompanyRepository>, ICompanyService
    {
        private readonly ICompanyRepository _repository;

        public CompanyService(
            ICompanyRepository repository,
            IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
        }

        public IList<CompanyViewModel> ListByFilters(CompanyFiltersViewModel filters)
        {
            return _repository
                .GetQuery(x => x.Id > 0)
                .WhereIf(filters.Id > 0, x => x.Id == filters.Id)
                .Select(x => x.AsCompanyViewMode()).ToList();
        }
    }
}
