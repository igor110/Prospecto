using Prospecto.Models.Enums;
using Prospecto.Models.ViewModel.Base;
using System;

namespace Prospecto.Models.ViewModel
{
    public class AttendanceFiltersViewModel : PaginationBase
    {
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public int? UserId { get; set; }
        public int TypeDate { get; set; }
        public StatusAttendancesEnum? Status { get; set; } //Status do atendimento usado no filtro (aberto, reagendado e fechado)
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? StatusId { get; set; } // Campo para armazenar o filtro
    }
}
