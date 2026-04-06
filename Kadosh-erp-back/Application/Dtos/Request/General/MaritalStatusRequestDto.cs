using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Request.General
{
    public class MaritalStatusRequestDto : BaseEntity
    {
        public string Name { get; set; } = null!;
    }
}