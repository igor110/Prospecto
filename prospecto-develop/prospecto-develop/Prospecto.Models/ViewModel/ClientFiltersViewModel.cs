using Prospecto.Models.ViewModel.Base;

namespace Prospecto.Models.ViewModel
{
    public class ClientFiltersViewModel : PaginationBase
    {
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
