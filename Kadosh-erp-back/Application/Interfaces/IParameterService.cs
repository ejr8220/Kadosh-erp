using Application.Dtos.Request.General;
using Application.Dtos.Response.General;

namespace Application.Interfaces
{
    public interface IParameterService
    {
        Task<int> CreateHeaderAsync(ParameterHeaderRequestDto request);
        Task<int> CreateDetailAsync(ParameterDetailRequestDto request);
        Task<List<ParameterHeaderResponseDto>> GetHeadersAsync();
        Task<string?> ResolveValueAsync(string code, int? companyId, DateTime? at = null);
    }
}
