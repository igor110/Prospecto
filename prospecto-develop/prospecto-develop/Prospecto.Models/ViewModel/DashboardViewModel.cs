using System;
using System.Collections.Generic;

namespace Prospecto.Models.ViewModel
{
    public class DashboardViewModel
    {
        public decimal MetaMensal { get; set; }
        public decimal TotalExecutado { get; set; }
        public decimal Diferenca { get; set; }
        public decimal MediaDiaria { get; set; }
        public decimal Projecao { get; set; }

        public List<VendaPorConsultor> VendasConsultores { get; set; } = new List<VendaPorConsultor>();
    }

    public class VendaPorConsultor
    {
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public decimal PercentualMeta { get; set; }
        public decimal ValorFaltante { get; set; }
        public decimal Projecao { get; set; }
        public decimal MetaConsultor { get; set; }
    }
}
