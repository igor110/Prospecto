using System.ComponentModel;

namespace Prospecto.Models.Enums
{
    public enum UserTypeEnum
    {
        MANAGER = 1,
        CONSULTANT = 2,
        ADMINISTRATOR = 3
    }

    public class UserType
    {
        public const string MANAGER = "Gerente";
        public const string CONSULTANT = "Consultor";
        public const string ADMINISTRATOR = "Administrador";

        public static string FromUserType(UserTypeEnum userType)
        {
            switch (userType)
            {
                case UserTypeEnum.MANAGER: return MANAGER;
                case UserTypeEnum.CONSULTANT: return CONSULTANT;
                case UserTypeEnum.ADMINISTRATOR: return ADMINISTRATOR;
                default:
                    throw new InvalidEnumArgumentException(nameof(userType));
            }
        }
    }
}
