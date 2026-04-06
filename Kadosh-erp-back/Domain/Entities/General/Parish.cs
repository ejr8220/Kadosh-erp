using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.General
{
    public class Parish : AuditoryEntity
    {
    public string Name { get; set; } = null!;
    public int CityId { get; set; }
    public City City { get; set; } = null!;
    public ICollection<Zone> Zones { get; set; } = new List<Zone>();

    }
}
