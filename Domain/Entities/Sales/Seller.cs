using Domain.Entities.General;
using Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Sales
{
    public class Seller : AuditoryEntity
    {
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;
        public string Observations { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
    }
}
