using Application.Dtos.Request;
using AutoMapper;



namespace Application.Interfaces
{
    public interface IGenericFilterService
    {
        Task<(List<TDto> result, int count)> FilterAsync<TEntity, TDto>(
        CustomDataManagerRequest request,
        IConfigurationProvider mapperConfig)
        where TEntity : class;

    }
}
