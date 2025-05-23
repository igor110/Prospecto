using AutoMapper;
using Prospecto.Data.Extensions;
using Prospecto.Models.DTO;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using Prospecto.Respository.Interface;
using Prospecto.Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Prospecto.Service
{
    public class BranchService : AbstractServiceBase<BranchDTO, BranchInfo, IBranchRepository>, IBranchService
    {
        private readonly IBranchRepository _repository;

        public BranchService(
            IBranchRepository repository,
            IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
        }

        public IList<BranchViewModel> ListByFilters(BranchFiltersViewModel filters)
        {
            return _repository
                .ListWithRelations(x => x.Id > 0)
                .WhereIf(filters.Id > 0, x => x.Id == filters.Id)
                .WhereIf(filters.IdCompany > 0, x => x.CompanyId == filters.IdCompany)
                .Select(x => x.AsBranchViewMode()).ToList();
        }
    }
}
