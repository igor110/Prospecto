using Prospecto.Models.Enums;
using System;

namespace Prospecto.Models.DTO
{
    public class AttendanceDTO
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
        public int UserId { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public int? ClientId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
