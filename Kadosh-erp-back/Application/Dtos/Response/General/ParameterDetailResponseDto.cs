namespace Application.Dtos.Response.General
{
    public class ParameterDetailResponseDto
    {
        public int Id { get; set; }
        public int ParameterHeaderId { get; set; }
        public int? CompanyId { get; set; }
        public string Value { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; }
    }
}
