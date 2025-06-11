using Prospecto.Models.Enums;
using System;

namespace Prospecto.Models.ViewModel
{
    public class AttendanceViewModel
    {
        public int Id { get; set; }
        public string NameClient { get; set; }
        public string Telephone { get; set; }
        public string NameProduct { get; set; }
        public decimal Value { get; set; }
        public decimal ValueClosed { get; set; }
        public DateTime DateRegistred { get; set; }
        public DateTime DateReturn { get; set; }
        public DateTime? DateClosed { get; set; }
        public StatusAttendancesEnum Status { get; set; }
        public StatusOrderEnum StatusOrder { get; set; }
        public ReschedulingOriginEnum ReschedulingOrigin { get; set; }
        public string Observation { get; set; }
        public UserViewModel User { get; set; }
        public int UserId { get; set; }
        public CompanyViewModel Company { get; set; }
        public int CompanyId { get; set; }
        public BranchViewModel Branch { get; set; }
        public int? BranchId { get; set; }
        public ClientViewModel Client { get; set; }
        public DateTime? DateNotification { get; set; }
        public DateTime? NotifyAt { get; set; } // novo campo: data e hora exata da notificação
        public TimeSpan? TimeReturn { get; set; } // novo campo auxiliar
        public string StatusLabel { get; set; }
        public int? StatusKanban { get; set; } = 0; // campo de status do kanban
        public int? ClientId { get; set; }


        public string StatusDescription
        {
            get
            {
                return Status switch
                {
                    StatusAttendancesEnum.OPEN => "Novo",
                    StatusAttendancesEnum.RESCHEDULED => "Em Atendimento",
                    StatusAttendancesEnum.WAITING => "Aguardando Retorno",
                    StatusAttendancesEnum.CLOSED => "Concluído",
                    _ => "Desconhecido"
                };
            }
        }
    }
}
