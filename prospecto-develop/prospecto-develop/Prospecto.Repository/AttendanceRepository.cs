using Prospecto.Data;
using Prospecto.Models.Infos;
using Prospecto.Respository.Interface;

namespace Prospecto.Repository
{
    public class AttendanceRepository : AbstractRepositoryBase<AttendanceInfo>, IAttendanceRepository
    {
        public AttendanceRepository(ProspectoContext dbContext) : base(dbContext) { }
    }
}
