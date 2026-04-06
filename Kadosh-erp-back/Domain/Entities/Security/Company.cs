using Domain.Entities.General;
using Domain.Entities.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    public class Company : AuditoryEntity
    {
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;
        public string LegalpresentativeIdentification { get; set; } = string.Empty;
        public string LegalRepresentative { get; set; } = string.Empty;
        public string AccountantIdentification { get; set; } = string.Empty;
        public string Accountant { get; set; } = string.Empty;
        public int CompanyTypeId { get; set; }
        public CompanyType CompanyType { get; set; } = null!;
        public string LogoUrl { get; set; } = string.Empty;
        public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
    }
}
