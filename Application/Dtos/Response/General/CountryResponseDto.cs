using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response.General
{
    public class CountryResponseDto : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? IsoCode { get; set; }
        // public IEnumerable<ProvinceResponseDto> Provinces { get; set; } = new List<ProvinceResponseDto>();
    }
}