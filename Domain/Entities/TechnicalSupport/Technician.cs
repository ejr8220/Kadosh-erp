using Domain.Entities.General;
using Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TechnicalSupport
{
    public class Technician : AuditoryEntity
    {
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;
        public string Specialty { get; set; } = null!; // Ej: "Electrical", "Mechanical", "IT"
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
    }
}
