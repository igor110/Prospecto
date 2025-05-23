using Prospecto.Models.Infos;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Prospecto.Respository.Interface
{
    public interface IUserRepository : IRepositoryBase<UserInfo>
    {
        IQueryable<UserInfo> ListWithRelations(Expression<Func<UserInfo, bool>> predicate);
    }
}
