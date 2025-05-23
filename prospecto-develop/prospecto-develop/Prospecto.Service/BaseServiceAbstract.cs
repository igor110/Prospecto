using AutoMapper;
using Prospecto.Models.DTO;
using Prospecto.Models.Infos.Base;
using Prospecto.Respository.Interface;

namespace Prospecto.Service
{
    public class BaseServiceAbstract : AbstractServiceBase<BaseDTO, BaseInfo, IRepositoryBase<BaseInfo>>
    {
        public BaseServiceAbstract(IRepositoryBase<BaseInfo> repositoryBase, IMapper mapper) : base(repositoryBase, mapper)
        { }
    }
}
