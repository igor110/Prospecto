using AutoMapper;
using Prospecto.Models.DTO;
using Prospecto.Models.Extensions;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using Prospecto.Respository.Interface;
using Prospecto.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prospecto.Service
{
    public class UserService : AbstractServiceBase<UserDTO, UserInfo, IUserRepository>, IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(
            IUserRepository repository,
            IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
        }

        public UserViewModel GetUser(string email, string password)
        {
            return _repository
                .GetQuery(x => x.Email == email && x.Password == password && x.IsActive == true)
                .Select(x => x.AsUserViewMode()).FirstOrDefault();

        }

        public IList<UserViewModel> ListByFilters(UserFiltersViewModel filters)
        {
            filters.IsVisible = true;

            var lstUser = _repository
                .ListWithRelations(x => x.IsVisible == true);

            return lstUser
                .WhereIf(filters.CompanyId > 0, x => x.CompanyId == filters.CompanyId)
                .WhereIf(filters.BranchId > 0, x => x.BranchId == filters.BranchId)
                .WhereIf(filters.TypeUser > 0, x => x.TypeUser == filters.TypeUser)
                .WhereIf(filters.Id > 0, x => x.Id == filters.Id)
                .Select(x => x.AsUserViewMode()).ToList();

        }

        public async Task<int> Save(int Id, UserDTO userDTO)
        {
            try
            {
                var count = await _repository.Count(x => x.Email == userDTO.Email && x.Id != Id);
                if (count > 0) throw new Exception("Email já cadastrado");

                if (Id > 0)
                    await base.Update(Id, userDTO);
                else
                {
                    var result = await base.Insert(userDTO);
                    if (result.Success) return result.Value;
                }
                return Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
