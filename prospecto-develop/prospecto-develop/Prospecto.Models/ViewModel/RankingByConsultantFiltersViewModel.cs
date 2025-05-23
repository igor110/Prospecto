using System;

namespace Prospecto.Models.ViewModel
{
    public class RankingByConsultantFiltersViewModel
    {
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public int? UserId { get; set; }
        public DateTime RakingBeginDate { get; set; }
        public DateTime RakingEndDate { get; set; }

    }
}
