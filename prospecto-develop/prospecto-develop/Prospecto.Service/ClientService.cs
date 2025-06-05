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
    public class ClientService : AbstractServiceBase<ClientDTO, ClientInfo, IClientRepository>, IClientService
    {
        private new readonly IClientRepository _repository;

        public ClientService(
            IClientRepository repository,
            IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
        }

        public IList<ClientViewModel> ListByFilters(ClientFiltersViewModel filters)
        {
            var lstUser = _repository
                .ListWithRelations(x => x.Id > 0);

            return lstUser
                .WhereIf(filters.CompanyId > 0, x => x.CompanyId == filters.CompanyId)
                .WhereIf(filters.BranchId > 0, x => x.BranchId == filters.BranchId)
                .WhereIf(filters.Id > 0, x => x.Id == filters.Id)
                .WhereIf(filters.UserId > 0, x => x.UserId == filters.UserId)
                .Select(x => x.AsClientViewMode()).ToList();

        }
        public IList<ClientInfo> SearchByName(string term)
        {
            return _repository
                .GetQuery(x => x.Name.Contains(term))
                .Take(10)
                .ToList();
        }

    }
}
