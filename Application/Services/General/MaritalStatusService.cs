using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response.General;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.General;
using Domain.Interfaces;

namespace Application.Services.General
{
    public class MaritalStatusService : ICrudService<MaritalStatusRequestDto, MaritalStatusResponseDto>
    {
        private readonly IRepository<MaritalStatus> _maritalStatusRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public MaritalStatusService(
            IRepository<MaritalStatus> maritalStatusRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _maritalStatusRepository = maritalStatusRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(MaritalStatusRequestDto dto)
        {
            var exists = await _maritalStatusRepository.AnyAsync(m => m.Name == dto.Name && !m.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"MaritalStatus '{dto.Name}' already exists.");

            var entity = _mapper.Map<MaritalStatus>(dto);
            await _maritalStatusRepository.AddAsync(entity);
            await _maritalStatusRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, MaritalStatusRequestDto dto)
        {
            var maritalStatus = await _maritalStatusRepository.GetByIdAsync(id);
            if (maritalStatus == null || maritalStatus.IsDeleted)
                throw new KeyNotFoundException($"MaritalStatus with ID {id} not found.");

            maritalStatus.Name = dto.Name;

            await _maritalStatusRepository.UpdateAsync(maritalStatus);
            await _maritalStatusRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var maritalStatus = await _maritalStatusRepository.GetByIdAsync(id);
            if (maritalStatus == null || maritalStatus.IsDeleted)
                throw new KeyNotFoundException($"MaritalStatus with ID {id} not found.");

            await _maritalStatusRepository.DeleteAsync(id); // activa EntityState.Deleted
            await _maritalStatusRepository.SaveChangesAsync(); // interceptor aplica borrado lógico
        }

        public async Task<(List<MaritalStatusResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<MaritalStatus, MaritalStatusResponseDto>(request, mapperConfig);
        }

        public async Task<MaritalStatusResponseDto?> GetByIdAsync(int id)
        {
            var maritalStatus = await _maritalStatusRepository.GetByIdAsync(id);
            if (maritalStatus == null)
                throw new KeyNotFoundException($"MaritalStatus with ID {id} not found.");

            return _mapper.Map<MaritalStatusResponseDto>(maritalStatus);
        }
    }
}