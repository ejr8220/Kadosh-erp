using Domain.Entities;
using Domain.Entities.Security;
using Domain.Entities.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response.General
{
    public class PersonResponseDto : BaseEntity
    {
        public int IdentificationTypeId { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public string IdentificationNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }

        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public int ParishId { get; set; }
    }
}