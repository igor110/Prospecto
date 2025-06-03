using System;
using System.ComponentModel.DataAnnotations;

namespace Prospecto.Models
{
    public class MetaMensal
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Mês de Referência")]
        public DateTime MesAno { get; set; } // Sempre armazenar como 01/MM/AAAA

        [Required]
        [Display(Name = "Meta (R$)")]
        public decimal ValorMeta { get; set; }

        [StringLength(100)]
        public string Loja { get; set; } // Opcional, caso tenha mais de uma loja
    }
}
