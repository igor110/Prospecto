using Prospecto.Models.Enums;

namespace Prospecto.Models.ViewModel
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public int UserId { get; set; }
        public ClientTypePersonEnum TypePerson { get; set; }

        public CompanyViewModel Company { get; set; }
        public BranchViewModel Branch { get; set; }
        public UserViewModel User { get; set; }
    }
}
