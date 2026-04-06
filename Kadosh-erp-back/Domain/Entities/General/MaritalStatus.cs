using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.General
{
    public class MaritalStatus : AuditoryEntity
    {
        public string Name { get; set; } = null!;
    }
}
