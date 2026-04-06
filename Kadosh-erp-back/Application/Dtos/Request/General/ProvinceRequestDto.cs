using Domain.Entities;
using Domain.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Request.General
{
    public class ProvinceRequestDto : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }
        // public CountryDto Country { get; set; } = null!;
    }
}