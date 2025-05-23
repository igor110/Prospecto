using Prospecto.Models.Infos.Base;
using Prospecto.Models.ViewModel;
using System.Text.Json.Serialization;

namespace Prospecto.Models.Infos
{
    public class BranchInfo : DeletableBaseInfo
    {
        public string Description { get; set; }
        public int? CompanyId { get; set; }
        public decimal SalesGoal { get; set; }

        #region Relationships           
        [JsonIgnore]
        public CompanyInfo Company { get; set; }
        #endregion

        public BranchViewModel AsBranchViewMode()
        {
            return new BranchViewModel
            {
                Id = Id,
                Description = Description,
                CompanyId = CompanyId,
                SalesGoal = SalesGoal,
                Company = new CompanyViewModel
                {
                    Description = Company?.Description,
                    Id = CompanyId.Value
                }
            };

        }

    }
}
