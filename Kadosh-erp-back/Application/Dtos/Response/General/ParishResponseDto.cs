using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response.General
{
    public class ParishResponseDto : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int CityId { get; set; }
        // public CityResponseDto City { get; set; } = null!;
    }
}