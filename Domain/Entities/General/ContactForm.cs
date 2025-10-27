using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.General
{
    public class ContactForm : AuditoryEntity
    {
        public string Name { get; set; } = null!; // Ej: "Email", "Phone", "WhatsApp"
        public string? Description { get; set; }

        public ICollection<ContactFormPerson> ContactFormPersons { get; set; } = new List<ContactFormPerson>();

    }
}
