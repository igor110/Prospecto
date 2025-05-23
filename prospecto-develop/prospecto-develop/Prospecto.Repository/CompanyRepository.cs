using Prospecto.Data;
using Prospecto.Models.Infos;
using Prospecto.Respository.Interface;

namespace Prospecto.Repository
{
    public class CompanyRepository : AbstractRepositoryBase<CompanyInfo>, ICompanyRepository
    {
        public CompanyRepository(ProspectoContext dbContext) : base(dbContext) { }
    }
}
