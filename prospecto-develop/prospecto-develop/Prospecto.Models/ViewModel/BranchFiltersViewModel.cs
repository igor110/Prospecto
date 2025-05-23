using Prospecto.Models.ViewModel.Base;

namespace Prospecto.Models.ViewModel
{
    public class BranchFiltersViewModel : PaginationBase
    {
        public int Id { get; set; }
        public int IdCompany { get; set; }
    }
}
