namespace Prospecto.Models.ViewModel
{
    public class ScheduleEventsViewModel
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Id { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public bool AllDay { get; set; } = false;
    }
}
