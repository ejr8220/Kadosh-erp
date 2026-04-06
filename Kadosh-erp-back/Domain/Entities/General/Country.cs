using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.General
{
    public class Country : AuditoryEntity
    {
        public string Name { get; set; } = null!;
        public string? IsoCode { get; set; } // 593
        public string? IsoCode2 { get; set; } // Ec
        public ICollection<Province> Provinces { get; set; } = new List<Province>();

    }
}
