using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Dtos.Response.Security
{
    public class UserResponseDto : BaseEntity
    {
        public string UserCode { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
        public List<string> Companies { get; set; } = new();
    }
}