using Application.Dtos.Request;
using Application.Dtos.Request.Security;
using Application.Dtos.Response.Security;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Security;
using Domain.Interfaces;

namespace Application.Services.Security
{
    public class RoleService : ICrudService<RoleRequestDto, RoleResponseDto>
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public RoleService(
            IRepository<Role> roleRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _roleRepository = roleRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(RoleRequestDto dto)
        {
            var exists = await _roleRepository.AnyAsync(r => r.Name == dto.Name && !r.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"Role '{dto.Name}' already exists.");

            var entity = _mapper.Map<Role>(dto);
            await _roleRepository.AddAsync(entity);
            await _roleRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, RoleRequestDto dto)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null || role.IsDeleted)
                throw new KeyNotFoundException($"Role with ID {id} not found.");

            role.Name = dto.Name;
            role.Description = dto.Description;

            await _roleRepository.UpdateAsync(role);
            await _roleRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null || role.IsDeleted)
                throw new KeyNotFoundException($"Role with ID {id} not found.");

            await _roleRepository.DeleteAsync(id);
            await _roleRepository.SaveChangesAsync();
        }

        public async Task<(List<RoleResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<Role, RoleResponseDto>(request, mapperConfig);
        }

        public async Task<RoleResponseDto?> GetByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                throw new KeyNotFoundException($"Role with ID {id} not found.");

            return _mapper.Map<RoleResponseDto>(role);
        }
    }
}