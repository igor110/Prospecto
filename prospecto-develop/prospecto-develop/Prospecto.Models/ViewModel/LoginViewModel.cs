using System.ComponentModel.DataAnnotations;

namespace Prospecto.Models.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name = "Endereço de email")]
        [Required(ErrorMessage = "O endereço do email é obrigatório")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [Required(ErrorMessage = "A senha é obrigatório")]
        public string Password { get; set; }
    }
}
