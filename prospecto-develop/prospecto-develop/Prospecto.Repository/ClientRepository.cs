using Microsoft.EntityFrameworkCore;
using Prospecto.Data;
using Prospecto.Models.Infos;
using Prospecto.Respository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Prospecto.Repository
{
    public class ClientRepository : AbstractRepositoryBase<ClientInfo>, IClientRepository
    {
        public ClientRepository(ProspectoContext dbContext) : base(dbContext) { }

        public IQueryable<ClientInfo> ListWithRelations(Expression<Func<ClientInfo, bool>> predicate)
        {
            return GetQuery(predicate).Include(x => x.Company).Include(b => b.Branch);
        }
    }
}
