using Domain.Entities.Security;

namespace Domain.Entities.General
{
    public class ParameterDetail : AuditoryEntity
    {
        public int ParameterHeaderId { get; set; }
        public ParameterHeader ParameterHeader { get; set; } = null!;
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
        public string Value1 { get; set; } = string.Empty;
        public string? Value2 { get; set; }
        public string? Value3 { get; set; }
        public string? Value4 { get; set; }
        public string? Value5 { get; set; }
        public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
