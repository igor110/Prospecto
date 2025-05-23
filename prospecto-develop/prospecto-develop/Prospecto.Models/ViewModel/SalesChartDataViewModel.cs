using System.Collections.Generic;

namespace Prospecto.Models.ViewModel
{
    public class SalesChartDataViewModel
    {
        public string[] labels { get; set; }
        public IList<SalesChartDataLabelViewModel> datasets { get; set; }
    }
}
