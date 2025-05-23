using Prospecto.Models.Enums;
using Prospecto.Models.ViewModel.Base;

namespace Prospecto.Models.ViewModel
{
    public class UserFiltersViewModel : PaginationBase
    {
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public UserTypeEnum TypeUser { get; set; }
        public int Id { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActivity { get; set; }
    }
}
