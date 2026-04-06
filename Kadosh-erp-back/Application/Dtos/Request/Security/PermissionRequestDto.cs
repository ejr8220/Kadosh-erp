using Domain.Entities;

namespace Application.Dtos.Request.Security
{
    public class PermissionRequestDto : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<int> RoleIds { get; set; } = new();
    }
}