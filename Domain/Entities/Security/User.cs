using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    public class User: AuditoryEntity
    {
        public string UserCode { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();

    }
}
