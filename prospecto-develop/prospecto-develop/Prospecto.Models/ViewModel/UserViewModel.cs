using Prospecto.Models.Enums;

namespace Prospecto.Models.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            TypeUser = UserTypeEnum.CONSULTANT;
            IsActive = true;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public bool IsVisible { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; }
        public UserTypeEnum TypeUser { get; set; }
        public CompanyViewModel Company { get; set; }
        public BranchViewModel Branch { get; set; }
    }
}
