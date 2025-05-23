using System;

namespace Prospecto.Models.ViewModel
{
    public class SalesChartDataLabelFiltersViewModel
    {
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public int? UserId { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
