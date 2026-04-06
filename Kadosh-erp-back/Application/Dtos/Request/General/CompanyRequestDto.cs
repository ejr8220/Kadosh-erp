using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Request.General
{
    public class CompanyRequestDto
    {
        public int PersonId { get; set; }
        public int? IdentificationTypeId { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string NombreComercial { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? ParishId { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int? CompanyTypeId { get; set; }
        public string LegalpresentativeIdentification { get; set; } = string.Empty;
        public string LegalRepresentativeIdentification { get; set; } = string.Empty;
        public string LegalRepresentative { get; set; } = string.Empty;
        public string LegalRepresentativeName { get; set; } = string.Empty;
        public string AccountantIdentification { get; set; } = string.Empty;
        public string AccountantName { get; set; } = string.Empty;
        public string Accountant { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string? LogoBase64 { get; set; }
        public List<BranchRequestDto> Branches { get; set; } = new();
    }
}