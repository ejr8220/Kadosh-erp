namespace Application.Dtos.Request.General
{
    public class ParameterDetailRequestDto
    {
        public int ParameterHeaderId { get; set; }
        public int? CompanyId { get; set; }
        public string Value { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
