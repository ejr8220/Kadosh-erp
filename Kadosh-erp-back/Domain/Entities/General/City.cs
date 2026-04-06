using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.General
{
    public  class City :AuditoryEntity
    {
        public string Name { get; set; } = null!;
        public int ProvinceId { get; set; }
        public Province Province { get; set; } = null!;
        public ICollection<Parish> Parishes { get; set; } = new List<Parish>();

    }
}
