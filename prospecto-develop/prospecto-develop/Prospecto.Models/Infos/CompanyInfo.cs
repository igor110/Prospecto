using Prospecto.Models.Infos.Base;
using Prospecto.Models.ViewModel;

namespace Prospecto.Models.Infos
{
    public class CompanyInfo : BaseInfo
    {
        public string Description { get; set; }

        public CompanyViewModel AsCompanyViewMode()
        {
            return new CompanyViewModel
            {
                Id = this.Id,
                Description = this.Description
            };

        }
    }
}
