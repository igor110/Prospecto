namespace Prospecto.Models.Infos.Base
{
    public abstract class DeletableBaseInfo : BaseInfo
    {
        public bool IsDeleted { get; set; }
    }
}
