using Domain.Entities;

namespace Application.Dtos.Request.Tax
{
    public class CompanyTypeRequestDto : BaseEntity
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string PersonType { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
    }
}
