using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response.General;

using Application.Interfaces;
using AutoMapper;
using Domain.Entities.General;
using Domain.Interfaces;

namespace Application.Services
{
    public class ParishService : ICrudService<ParishRequestDto, ParishResponseDto>
    {
        private readonly IRepository<Parish> _parishRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public ParishService(
            IRepository<Parish> parishRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _parishRepository = parishRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(ParishRequestDto dto)
        {
            var exists = await _parishRepository.AnyAsync(p => p.Name == dto.Name && p.CityId == dto.CityId && !p.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"Parish '{dto.Name}' already exists.");

            var entity = _mapper.Map<Parish>(dto);
            await _parishRepository.AddAsync(entity);
            await _parishRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ParishRequestDto dto)
        {
            var parish = await _parishRepository.GetByIdAsync(id);
            if (parish == null || parish.IsDeleted)
                throw new KeyNotFoundException($"Parish with ID {id} not found.");

            parish.Name = dto.Name;
            parish.CityId = dto.CityId;

            await _parishRepository.UpdateAsync(parish);
            await _parishRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var parish = await _parishRepository.GetByIdAsync(id);
            if (parish == null || parish.IsDeleted)
                throw new KeyNotFoundException($"Parish with ID {id} not found.");

            await _parishRepository.DeleteAsync(id);
            await _parishRepository.SaveChangesAsync();
        }

        public async Task<(List<ParishResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<Parish, ParishResponseDto>(request, mapperConfig);
        }

        public async Task<ParishResponseDto?> GetByIdAsync(int id)
        {
            var parish = await _parishRepository.GetByIdAsync(id);
            if (parish == null)
                throw new KeyNotFoundException($"Parish with ID {id} not found.");

            return _mapper.Map<ParishResponseDto>(parish);
        }
    }
}