using Domain.Entities;

namespace Application.Dtos.Request.Security
{
    public class CompanyUserRequestDto : BaseEntity
    {
        public int CompanyId { get; set; }
        public int UserId { get; set; }
    }
}