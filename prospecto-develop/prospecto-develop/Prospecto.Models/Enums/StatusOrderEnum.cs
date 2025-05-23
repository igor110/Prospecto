namespace Prospecto.Models.Enums
{
    public enum StatusOrderEnum
    {
        GAIN = 1,
        LOSS = 2
    }

    public class StatusOrder
    {
        public const string GAIN = "Ganho";
        public const string LOSS = "Perda";

        public static string FromStatusOrder(StatusOrderEnum statusOrder)
        {
            switch (statusOrder)
            {
                case StatusOrderEnum.GAIN: return GAIN;
                case StatusOrderEnum.LOSS: return LOSS;
                default:
                    return "";
            }
        }
    }
}
