using Application.Dtos.Request;
using Application.Dtos.Request.Security;
using Application.Dtos.Response.Security;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Security;
using Domain.Interfaces;

namespace Application.Services.Security
{
    public class PermissionService : ICrudService<PermissionRequestDto, PermissionResponseDto>
    {
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public PermissionService(
            IRepository<Permission> permissionRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(PermissionRequestDto dto)
        {
            var exists = await _permissionRepository.AnyAsync(p => p.Name == dto.Name && !p.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"Ya existe un permiso con el nombre '{dto.Name}'.");

            var entity = _mapper.Map<Permission>(dto);
            await _permissionRepository.AddAsync(entity);
            await _permissionRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, PermissionRequestDto dto)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null || permission.IsDeleted)
                throw new KeyNotFoundException($"Permiso con ID {id} no encontrado.");

            permission.Name = dto.Name;
            permission.Description = dto.Description;

            await _permissionRepository.UpdateAsync(permission);
            await _permissionRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null || permission.IsDeleted)
                throw new KeyNotFoundException($"Permiso con ID {id} no encontrado.");

            await _permissionRepository.DeleteAsync(id);
            await _permissionRepository.SaveChangesAsync();
        }

        public async Task<(List<PermissionResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<Permission, PermissionResponseDto>(request, mapperConfig);
        }

        public async Task<PermissionResponseDto?> GetByIdAsync(int id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null)
                throw new KeyNotFoundException($"Permiso con ID {id} no encontrado.");

            return _mapper.Map<PermissionResponseDto>(permission);
        }
    }
}