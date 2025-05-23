using System.ComponentModel.DataAnnotations;

namespace Prospecto.Models.ViewModel
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A descrição da empresa é obrigatório")]
        public string Description { get; set; }
    }
}
