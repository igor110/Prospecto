namespace Prospecto.Options.Models
{
    public class MailOptions
    {
        public const string SECTION_NAME = "MAIL";
        public string Host { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }
    }
}
