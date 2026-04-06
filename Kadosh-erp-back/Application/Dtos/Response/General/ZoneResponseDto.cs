using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response.General
{
    public class ZoneResponseDto : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int ParishId { get; set; }
    }
}
