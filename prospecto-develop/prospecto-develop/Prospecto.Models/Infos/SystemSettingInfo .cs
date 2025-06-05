using System;

namespace Prospecto.Models.Infos
{
    public class SystemSettingInfo
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? BranchId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string CompanyName { get; set; }
        public string BranchName { get; set; }
    }
}
