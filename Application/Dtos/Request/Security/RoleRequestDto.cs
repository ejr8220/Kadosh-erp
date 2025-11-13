using Domain.Entities;

namespace Application.Dtos.Request.Security
{
    public class RoleRequestDto : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<int> PermissionIds { get; set; } = new();
    }
}