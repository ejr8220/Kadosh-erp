using Domain.Common.Enums;

namespace Domain.Entities.General
{
    public class ParameterHeader : AuditoryEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ParameterScope Scope { get; set; }
        public ICollection<ParameterDetail> Details { get; set; } = new List<ParameterDetail>();
    }
}
