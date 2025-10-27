using Domain.Entities.Security;
using Domain.Entities.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.General
{
    public class Person : AuditoryEntity
    {
        public IdentificationType identificationType { get; set; }
        public string IdentificationNumber { get; set; } = null!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; } 

        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public int ProvinceId { get; set; }
        public Province? Province { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }
        public int ParishId { get; set; }
        public Parish? Parish { get; set; }
        public Company? Company { get; set; }

    }
}
