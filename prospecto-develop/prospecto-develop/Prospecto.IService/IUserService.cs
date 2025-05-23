using Prospecto.Models.DTO;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prospecto.Service.Interface
{
    public interface IUserService : IServiceBase<UserDTO, UserInfo>
    {
        IList<UserViewModel> ListByFilters(UserFiltersViewModel filters);
        UserViewModel GetUser(string email, string password);
        Task<int> Save(int Id, UserDTO userDTO);
    }
}
