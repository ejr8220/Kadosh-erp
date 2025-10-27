using Domain.Entities;
using Domain.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Request.General
{
    public class CountryRequestDto : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? IsoCode { get; set; }
    }
}