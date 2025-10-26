using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Tax
{
    internal class IdentificationType : AuditoryEntity
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int Maxlength { get; set; }
    }
}
