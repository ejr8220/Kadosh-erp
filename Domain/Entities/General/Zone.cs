using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.General
{
    public class Zone : AuditoryEntity
    {
        public string Name { get; set; } = null!;
        public int ParishId { get; set; }
        public Parish Parish { get; set; } = null!;

    }
}
