using System.ComponentModel;

namespace Prospecto.Models.Enums
{
    public enum ClientTypePersonEnum
    {
        PHYSICAL = 1,
        LEGAL = 2
    }

    public class ClientTypePerson
    {
        public const string PHYSICAL = "Física";
        public const string LEGAL = "Jurídica";

        public static string FromClientTypePerson(ClientTypePersonEnum clientTypePerson)
        {
            switch (clientTypePerson)
            {
                case ClientTypePersonEnum.PHYSICAL: return PHYSICAL;
                case ClientTypePersonEnum.LEGAL: return LEGAL;
                default:
                    throw new InvalidEnumArgumentException(nameof(clientTypePerson));
            }
        }
    }
}
