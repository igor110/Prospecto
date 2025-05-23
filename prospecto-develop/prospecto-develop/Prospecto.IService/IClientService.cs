using Prospecto.Models.DTO;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using System.Collections.Generic;

namespace Prospecto.Service.Interface
{
    public interface IClientService : IServiceBase<ClientDTO, ClientInfo>
    {
        IList<ClientViewModel> ListByFilters(ClientFiltersViewModel filters);
    }
}
