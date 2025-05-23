namespace Prospecto.Models.ViewModel
{
    public class SalesChartDataLabelViewModel
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public bool pointRadius { get; set; } = false;
        public string borderColor { get; set; }
        public string pointColor { get; set; }
        public string pointStrokeColor { get; set; }
        public string pointHighlightFill { get; set; }
        public string pointHighlightStroke { get; set; }
        public decimal[] data { get; set; }
    }
}
