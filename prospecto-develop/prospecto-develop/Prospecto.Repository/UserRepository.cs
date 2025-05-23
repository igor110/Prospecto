using Microsoft.EntityFrameworkCore;
using Prospecto.Data;
using Prospecto.Models.Infos;
using Prospecto.Respository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Prospecto.Repository
{
    public class UserRepository : AbstractRepositoryBase<UserInfo>, IUserRepository
    {
        public UserRepository(ProspectoContext dbContext) : base(dbContext) { }

        public IQueryable<UserInfo> ListWithRelations(Expression<Func<UserInfo, bool>> predicate)
        {
            return GetQuery(predicate)
                .Include(x => x.Company)
                .Include(x => x.Branch);
        }
    }
}
