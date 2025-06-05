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
        IList<NotificationViewModel> ListNotifications();
        AttendanceViewModel GetWithRelations(int id);
        Task<bool> ImportAttendance(IFormFile file);
        Task<bool> Reschedule(int idAttendance, DateTime date, TimeSpan? time = null);
        SalesChartDataViewModel SaleByConsultant(SalesChartDataLabelFiltersViewModel filters);
        IList<RankingByConsultantViewModel> RankingByConsultant(RankingByConsultantFiltersViewModel filters);
        IList<ScheduleServiceViewModel> ScheduledService(AttendanceFiltersViewModel filters);
        IList<NotificationViewModel> GetPendingNotifications(int userId);

    }
}
