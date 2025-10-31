using Domain.Entities;

namespace Application.Dtos.Response.Security
{
    public class CompanyUserResponseDto : BaseEntity
    {
        public int CompanyId { get; set; }
        public int UserId { get; set; }
    }
}