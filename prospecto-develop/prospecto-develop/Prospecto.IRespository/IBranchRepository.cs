using Prospecto.Models.Infos;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Prospecto.Respository.Interface
{
    public interface IBranchRepository : IRepositoryBase<BranchInfo>
    {
        IQueryable<BranchInfo> ListWithRelations(Expression<Func<BranchInfo, bool>> predicate);
    }
}
