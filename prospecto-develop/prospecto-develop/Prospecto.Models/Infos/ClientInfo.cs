using Prospecto.Models.Enums;
using Prospecto.Models.Infos.Base;
using Prospecto.Models.ViewModel;
using System.Text.Json.Serialization;

namespace Prospecto.Models.Infos
{
    public class ClientInfo : DeletableBaseInfo
    {
        #region Properties
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
        public ClientTypePersonEnum TypePerson { get; set; }

        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public int UserId { get; set; }
        #endregion

        #region Relationships           
        [JsonIgnore]
        public CompanyInfo Company { get; set; }

        [JsonIgnore]
        public BranchInfo Branch { get; set; }

        [JsonIgnore]
        public UserInfo User { get; set; }
        #endregion

        public ClientViewModel AsClientViewMode()
        {
            return new ClientViewModel()
            {
                Name = Name,
                CPF = CPF,
                CNPJ = CNPJ,
                Email = Email,
                Telephone = Telephone,
                CompanyId = CompanyId,
                Address = Address,
                City = City,
                Complement = Complement,
                Id = Id,
                Neighborhood = Neighborhood,
                Number = Number,
                ZipCode = ZipCode,
                BranchId = BranchId,
                TypePerson = TypePerson,
                Branch = new BranchViewModel { Description = Branch?.Description, Id = BranchId ?? 0, CompanyId = CompanyId ?? 0 },
                Company = new CompanyViewModel { Description = Company?.Description, Id = CompanyId ?? 0 },
                User = new UserViewModel { Name = User?.Name, Id = UserId, TypeUser = User?.TypeUser ?? UserTypeEnum.ADMINISTRATOR, Email = User?.Email }
            };
        }
    }
}
