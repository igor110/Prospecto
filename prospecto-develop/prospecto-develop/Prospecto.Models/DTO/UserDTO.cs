using Prospecto.Models.Enums;

namespace Prospecto.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public UserTypeEnum TypeUser { get; set; }
        public bool IsVisible { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; }
    }
}
