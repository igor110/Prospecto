using Microsoft.AspNetCore.Http;
using Prospecto.Models.DTO;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prospecto.Service.Interface
{
    public interface IAttendanceService : IServiceBase<AttendanceDTO, AttendanceInfo>
    {
        IList<AttendanceViewModel> ListByFilters(AttendanceFiltersViewModel filters);
        AttendanceViewModel GetWithRelations(int id);
        Task<bool> ImportAttendance(IFormFile file);
        Task<bool> Reschedule(int idAttendace, DateTime date);
        SalesChartDataViewModel SaleByConsultant(SalesChartDataLabelFiltersViewModel filters);
        IList<RankingByConsultantViewModel> RankingByConsultant(RankingByConsultantFiltersViewModel filters);
        IList<ScheduleServiceViewModel> ScheduledService(AttendanceFiltersViewModel filters);
    }
}
