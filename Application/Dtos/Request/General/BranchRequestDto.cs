using System;

namespace Application.Dtos.Request.General
{
    public class BranchRequestDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int CompanyId { get; set; }
    }
}
