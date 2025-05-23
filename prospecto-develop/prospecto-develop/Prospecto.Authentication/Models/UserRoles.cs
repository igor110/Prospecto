using Prospecto.Models.Enums;
using System.ComponentModel;

namespace Prospecto.Authentication.Models
{

    public class UserRoles
    {
        public const string MANAGER = "Manager";
        public const string CONSULTANT = "Consultant";
        public const string ADMINISTRATOR = "Administrator";

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
