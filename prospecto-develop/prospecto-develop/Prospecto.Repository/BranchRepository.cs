using Microsoft.EntityFrameworkCore;
using Prospecto.Data;
using Prospecto.Models.Infos;
using Prospecto.Respository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Prospecto.Repository
{
    public class BranchRepository : AbstractRepositoryBase<BranchInfo>, IBranchRepository
    {
        public BranchRepository(ProspectoContext dbContext) : base(dbContext) { }

        public IQueryable<BranchInfo> ListWithRelations(Expression<Func<BranchInfo, bool>> predicate)
        {
            return GetQuery(predicate).Include(x => x.Company);
        }
    }
}
