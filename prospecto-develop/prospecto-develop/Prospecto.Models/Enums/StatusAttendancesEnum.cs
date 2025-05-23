namespace Prospecto.Models.Enums
{
    public enum StatusAttendancesEnum
    {
        OPEN = 1,
        CLOSED = 2,
        RESCHEDULED = 3,
        WAITING = 4
    }

    public class StatusAttendance
    {
        public const string OPEN = "Aberto";
        public const string CLOSED = "Fechado";
        public const string RESCHEDULED = "Reagendado";

        public static string FromStatusAttendances(StatusAttendancesEnum statusAttendance)
        {
            switch (statusAttendance)
            {
                case StatusAttendancesEnum.OPEN: return OPEN;
                case StatusAttendancesEnum.CLOSED: return CLOSED;
                case StatusAttendancesEnum.RESCHEDULED: return RESCHEDULED;
                default:
                    return "";
            }
        }
    }
}
