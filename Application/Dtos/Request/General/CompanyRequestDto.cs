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
        public string LegalpresentativeIdentification { get; set; } = string.Empty;
        public string LegalRepresentative { get; set; } = string.Empty;
        public string AccountantIdentification { get; set; } = string.Empty;
        public string Accountant { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
    }
}