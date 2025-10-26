using Application.Dtos.Request;
using AutoMapper;


namespace Application.Interfaces
{
    public interface ICrudService<TRequest, TResponse>
    {
        Task<TResponse?> GetByIdAsync(int id);
        Task AddAsync(TRequest dto);
        Task UpdateAsync(int id, TRequest dto);
        Task DeleteAsync(int id);
        Task<(List<TResponse> result, int count)> GetAllFilterAsync(CustomDataManagerRequest request, IConfigurationProvider mapperConfig);
    }

}
    