namespace Application.Dtos.Response.General
{
    public class ResolvedParameterValueResponseDto
    {
        public string Code { get; set; } = string.Empty;
        public int? CompanyId { get; set; }
        public string? Value { get; set; }
    }
}
