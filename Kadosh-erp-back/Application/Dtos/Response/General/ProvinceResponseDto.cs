using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response.General
{
    public class ProvinceResponseDto : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }
        // public CountryResponseDto Country { get; set; } = null!;
    }
}