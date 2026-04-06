using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Dtos.Request.Security
{
    public class UserRequestDto : BaseEntity
    {
        public string UserCode { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<int> RoleIds { get; set; } = new();
        public List<int> CompanyIds { get; set; } = new();
    }
}