using Domain.Entities;

namespace Application.Dtos.Response.Security
{
    public class RoleResponseDto : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<int> PermissionIds { get; set; } = new();
    }
}