using Domain.Entities;

namespace Application.Dtos.Response.Tax
{
    public class CompanyTypeResponseDto : BaseEntity
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string PersonType { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
    }
}
