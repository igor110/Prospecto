using Prospecto.Models.Enums;
using Prospecto.Models.Infos.Base;
using Prospecto.Models.ViewModel;
using System;
using System.Text.Json.Serialization;

namespace Prospecto.Models.Infos
{
    public class AttendanceInfo : DeletableBaseInfo
    {
        #region Properties
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
        public string Observation { get; set; }
        public int UserId { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public int? ClientId { get; set; }
        public ReschedulingOriginEnum ReschedulingOrigin { get; set; }
        #endregion

        #region Relationships           
        [JsonIgnore]
        public CompanyInfo Company { get; set; }

        [JsonIgnore]
        public BranchInfo Branch { get; set; }

        [JsonIgnore]
        public UserInfo User { get; set; }

        [JsonIgnore]
        public ClientInfo Client { get; set; }
        #endregion


        public AttendanceViewModel AsAttendanceViewMode() => new()
        {
            Id = Id,
            NameClient = NameClient,
            DateReturn = DateReturn,
            DateRegistred = DateRegistred,
            DateClosed = DateClosed,
            Observation = Observation,
            NameProduct = NameProduct,
            Status = Status,
            Telephone = Telephone,
            Value = Value,
            ValueClosed = ValueClosed,
            StatusOrder = StatusOrder,
            ReschedulingOrigin = ReschedulingOrigin,
            User = new UserViewModel
            {
                Email = User?.Email,
                Name = User?.Name,
                Color = User?.Color
            },
            UserId = UserId,
            Company = new CompanyViewModel
            {
                Description = Company?.Description,
                Id = CompanyId ?? 0
            },
            CompanyId = CompanyId ?? 0,
            BranchId = BranchId ?? 0,
            Branch = new BranchViewModel
            {
                CompanyId = CompanyId,
                Id = BranchId ?? 0,
                Description = Branch?.Description
            },
            Client = new ClientViewModel
            {
                City = Client?.City,
                Address = Client?.Address,
                Complement = Client?.Complement,
                CPF = Client?.CPF,
                Email = Client?.Email,
                Id = Client?.Id ?? 0,
                Name = Client?.Name,
                Neighborhood = Client?.Neighborhood,
                Number = Client?.Number,
                Telephone = Client?.Telephone,
                ZipCode = Client?.ZipCode
            }
        };
    }
}
