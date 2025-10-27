using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Tax
{
    public class TaxTypeDetail : AuditoryEntity
    {
        public int TaxTypeId { get; set; }
        public TaxType TaxType { get; set; } = null!;
        public decimal Value { get; set; } 
        public decimal Rate { get; set; } // Ej: 0.16m, 0.0m
        public string Description { get; set; } = null!;
    }
}
