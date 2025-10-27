using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Tax
{
    public class TaxType : AuditoryEntity
    {
        public string Name { get; set; } = null!; // Ej: "IVA", "ISR", "IEPS"
        public string Description { get; set; } = null!;
    }
}
