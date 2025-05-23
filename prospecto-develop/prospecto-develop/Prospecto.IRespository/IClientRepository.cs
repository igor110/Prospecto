using Prospecto.Models.Infos;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Prospecto.Respository.Interface
{
    public interface IClientRepository : IRepositoryBase<ClientInfo>
    {
        IQueryable<ClientInfo> ListWithRelations(Expression<Func<ClientInfo, bool>> predicate);
    }
}
