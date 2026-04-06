using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response.General;

using Application.Interfaces;
using AutoMapper;
using Domain.Entities.General;
using Domain.Interfaces;

namespace Application.Services.General
{
    public class CityService : ICrudService<CityRequestDto, CityResponseDto>
    {
        private readonly IRepository<City> _cityRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public CityService(
            IRepository<City> cityRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _cityRepository = cityRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(CityRequestDto dto)
        {
            var exists = await _cityRepository.AnyAsync(c => c.Name == dto.Name && c.ProvinceId == dto.ProvinceId && !c.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"City '{dto.Name}' already exists.");

            var entity = _mapper.Map<City>(dto);
            await _cityRepository.AddAsync(entity);
            await _cityRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CityRequestDto dto)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null || city.IsDeleted)
                throw new KeyNotFoundException($"City with ID {id} not found.");

            city.Name = dto.Name;
            city.ProvinceId = dto.ProvinceId;

            await _cityRepository.UpdateAsync(city);
            await _cityRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null || city.IsDeleted)
                throw new KeyNotFoundException($"City with ID {id} not found.");

            await _cityRepository.DeleteAsync(id);
            await _cityRepository.SaveChangesAsync();
        }

        public async Task<(List<CityResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<City, CityResponseDto>(request, mapperConfig);
        }

        public async Task<CityResponseDto?> GetByIdAsync(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
                throw new KeyNotFoundException($"City with ID {id} not found.");

            return _mapper.Map<CityResponseDto>(city);
        }
    }
}