using Domain.Common.Enums;

namespace Application.Dtos.Request.General
{
    public class ParameterHeaderRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ParameterScope Scope { get; set; }
    }
}
