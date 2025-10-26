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
        public string? IsoCode { get; set; } // Optional: ISO 3166-1 alpha-2 or alpha-3
        public ICollection<Province> Provinces { get; set; } = new List<Province>();

    }
}
