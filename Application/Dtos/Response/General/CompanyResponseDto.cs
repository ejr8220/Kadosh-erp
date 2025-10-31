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
        public string LegalpresentativeIdentification { get; set; } = string.Empty;
        public string LegalRepresentative { get; set; } = string.Empty;
        public string AccountantIdentification { get; set; } = string.Empty;
        public string Accountant { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        
    }
}
