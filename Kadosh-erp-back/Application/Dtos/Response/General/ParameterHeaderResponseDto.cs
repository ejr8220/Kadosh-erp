using Domain.Common.Enums;

namespace Application.Dtos.Response.General
{
    public class ParameterHeaderResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ParameterScope Scope { get; set; }
        public List<ParameterDetailResponseDto> Details { get; set; } = new();
    }
}
