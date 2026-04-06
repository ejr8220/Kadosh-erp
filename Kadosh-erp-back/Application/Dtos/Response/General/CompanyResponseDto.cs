using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response.General
{
    public class CompanyResponseDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int IdentificationTypeId { get; set; }
        public int CompanyTypeId { get; set; }
        public string CompanyTypeName { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string NombreComercial { get; set; } = string.Empty;
        public int? CountryId { get; set; }
        public string Pais { get; set; } = string.Empty;
        public int? ProvinceId { get; set; }
        public string Provincia { get; set; } = string.Empty;
        public int? CityId { get; set; }
        public string Ciudad { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public List<CompanyContactFormResponseDto> ContactForms { get; set; } = new();
        public string PersonIdentificationNumber { get; set; } = string.Empty;
        public string PersonFirstName { get; set; } = string.Empty;
        public string PersonLastName { get; set; } = string.Empty;
        public string PersonFullName { get; set; } = string.Empty;
        public string LegalpresentativeIdentification { get; set; } = string.Empty;
        public string LegalRepresentativeName { get; set; } = string.Empty;
        public string AccountantIdentification { get; set; } = string.Empty;
        public string AccountantName { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class CompanyContactFormResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }
}
