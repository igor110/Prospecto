namespace Prospecto.Models.Enums
{
    public enum ReschedulingOriginEnum
    {
        TELEPHONE = 1,
        EMAIL = 2,
        STORE = 3,
        INTERNET = 4,
        PROSPECTION = 5,
        IMPORT = 6
    }

    public class ReschedulingOrigin
    {
        public const string TELEPHONE = "Telefone";
        public const string EMAIL = "Email";
        public const string STORE = "Loja";
        public const string INTERNET = "Internet";
        public const string PROSPECTION = "Prospecção";
        public const string IMPORT = "Importação";

        public static string FromReschedulingOrigin(ReschedulingOriginEnum reschedulingOrigin)
        {
            switch (reschedulingOrigin)
            {
                case ReschedulingOriginEnum.TELEPHONE: return TELEPHONE;
                case ReschedulingOriginEnum.EMAIL: return EMAIL;
                case ReschedulingOriginEnum.STORE: return STORE;
                case ReschedulingOriginEnum.INTERNET: return INTERNET;
                case ReschedulingOriginEnum.PROSPECTION: return PROSPECTION;
                case ReschedulingOriginEnum.IMPORT: return IMPORT;
                default:
                    return "";
            }
        }
    }
}
