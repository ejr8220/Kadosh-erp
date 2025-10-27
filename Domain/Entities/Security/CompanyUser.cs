﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    public class CompanyUser : AuditoryEntity
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;


    }
}
