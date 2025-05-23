using System.ComponentModel.DataAnnotations;

namespace Prospecto.Models.ViewModel
{
    public class BranchViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A descrição da filial é obrigatório")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Informe uma empresa corretamente")]
        public int? CompanyId { get; set; }
        public CompanyViewModel Company { get; set; }
        public decimal SalesGoal { get; set; }
    }
}
