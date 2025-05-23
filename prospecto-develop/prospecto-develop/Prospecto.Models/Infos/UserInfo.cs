using Prospecto.Models.Enums;
using Prospecto.Models.Infos.Base;
using Prospecto.Models.ViewModel;
using System.Text.Json.Serialization;

namespace Prospecto.Models.Infos
{
    public class UserInfo : DeletableBaseInfo
    {
        #region Properties
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserTypeEnum TypeUser { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public bool IsVisible { get; set; } = true;
        public string Color { get; set; }
        public bool IsActive { get; set; }
        #endregion

        #region Relationships           
        [JsonIgnore]
        public CompanyInfo Company { get; set; }
        public BranchInfo Branch { get; set; }
        #endregion

        public UserViewModel AsUserViewMode()
        {
            return new UserViewModel
            {
                CompanyId = CompanyId,
                BranchId = BranchId,
                Email = Email,
                Id = Id,
                Name = Name,
                Password = Password,
                TypeUser = TypeUser,
                IsVisible = IsVisible,
                Color = Color,
                IsActive = IsActive,
                Company = new CompanyViewModel { Description = Company?.Description, Id = CompanyId ?? 0 },
                Branch = new BranchViewModel { Description = Branch?.Description, Id = BranchId ?? 0 }
            };

        }
    }
}
