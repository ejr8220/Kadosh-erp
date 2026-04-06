using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.General
{
    public class ContactFormPerson :AuditoryEntity
    {
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

        public int ContactFormId { get; set; }
        public ContactForm ContactForm { get; set; } = null!;

        public ContactContextType ContextType { get; set; } // Ej: contacto como Cliente, Vendedor, etc.
        public string Value { get; set; } = null!;
        public bool IsPrimary { get; set; } = false;
    }
}
