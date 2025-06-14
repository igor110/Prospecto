namespace Prospecto.Models.ViewModel
{
    public class AttendanceMobileViewModel
    {
        public string NameClient { get; set; }
        public string Telephone { get; set; }
        public string Source { get; set; } // Email, Telefone, Loja, etc.
        public string Product { get; set; }
        public decimal Value { get; set; }
        public string DateReturn { get; set; } // "yyyy-MM-dd"
        public string TimeReturn { get; set; } // "HH:mm"
        public string Observation { get; set; }
        public bool Notify { get; set; }
    }
}
