using System;

namespace Application.Dtos.Request.Security
{
    public class UserRoleRequestDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}