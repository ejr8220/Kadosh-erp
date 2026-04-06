using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Tax
{
    public class CompanyType : AuditoryEntity
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string PersonType { get; set; } = null!;
        public string Description { get; set; } = string.Empty;

    }
}
