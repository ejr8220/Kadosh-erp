using System;

namespace Application.Dtos.Request.Security
{
    public class RolePermissionRequestDto
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}