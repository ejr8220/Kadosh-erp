using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response.General;

using Application.Interfaces;
using AutoMapper;
using Domain.Entities.General;
using Domain.Interfaces;

namespace Application.Services
{
    public class ZoneService : ICrudService<ZoneRequestDto, ZoneResponseDto>
    {
        private readonly IRepository<Zone> _zoneRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public ZoneService(
            IRepository<Zone> zoneRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _zoneRepository = zoneRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(ZoneRequestDto dto)
        {
            var exists = await _zoneRepository.AnyAsync(z => z.Name == dto.Name && !z.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"Zone '{dto.Name}' already exists.");

            var entity = _mapper.Map<Zone>(dto);
            await _zoneRepository.AddAsync(entity);
            await _zoneRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ZoneRequestDto dto)
        {
            var zone = await _zoneRepository.GetByIdAsync(id);
            if (zone == null || zone.IsDeleted)
                throw new KeyNotFoundException($"Zone with ID {id} not found.");

            zone.Name = dto.Name;
            zone.ParishId = dto.ParishId;

            await _zoneRepository.UpdateAsync(zone);
            await _zoneRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var zone = await _zoneRepository.GetByIdAsync(id);
            if (zone == null || zone.IsDeleted)
                throw new KeyNotFoundException($"Zone with ID {id} not found.");

            await _zoneRepository.DeleteAsync(id); // activa EntityState.Deleted
            await _zoneRepository.SaveChangesAsync(); // interceptor aplica borrado lógico
        }

        public async Task<(List<ZoneResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<Zone, ZoneResponseDto>(request, mapperConfig);
        }

        public async Task<ZoneResponseDto?> GetByIdAsync(int id)
        {
            var zone = await _zoneRepository.GetByIdAsync(id);
            if (zone == null )
                throw new KeyNotFoundException($"Zone with ID {id} not found.");

            return _mapper.Map<ZoneResponseDto>(zone);

        }
    }
}