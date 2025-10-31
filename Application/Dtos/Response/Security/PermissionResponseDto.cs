using Domain.Entities;

namespace Application.Dtos.Response.Security
{
    public class PermissionResponseDto : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<int> RoleIds { get; set; } = new();
    }
}