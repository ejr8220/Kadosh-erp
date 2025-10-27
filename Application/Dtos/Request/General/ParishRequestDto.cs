using Domain.Entities;
using Domain.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Request.General
{
    public class ParishRequestDto : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int CityId { get; set; }
        //public CityDto City { get; set; } = null!;
    }
}