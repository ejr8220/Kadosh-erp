using Application.Dtos.Request;
using Application.Dtos.Request.Security;
using Application.Dtos.Response;
using Application.Dtos.Response.Security;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Security;
using Domain.Interfaces;

namespace Application.Services.Security
{
    public class RolePermissionService : ICrudService<RolePermissionRequestDto, RolePermissionResponseDto>
    {
        private readonly IRepository<RolePermission> _rolePermissionRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public RolePermissionService(
            IRepository<RolePermission> rolePermissionRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(RolePermissionRequestDto dto)
        {
            var exists = await _rolePermissionRepository.AnyAsync(rp =>
                rp.RoleId == dto.RoleId &&
                rp.PermissionId == dto.PermissionId &&
                !rp.IsDeleted);

            if (exists)
                throw new InvalidOperationException("La relación rol-permiso ya existe.");

            var entity = _mapper.Map<RolePermission>(dto);
            await _rolePermissionRepository.AddAsync(entity);
            await _rolePermissionRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, RolePermissionRequestDto dto)
        {
            var rolePermission = await _rolePermissionRepository.GetByIdAsync(id);
            if (rolePermission == null || rolePermission.IsDeleted)
                throw new KeyNotFoundException($"No se encontró la relación con ID {id}.");

            rolePermission.RoleId = dto.RoleId;
            rolePermission.PermissionId = dto.PermissionId;

            await _rolePermissionRepository.UpdateAsync(rolePermission);
            await _rolePermissionRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rolePermission = await _rolePermissionRepository.GetByIdAsync(id);
            if (rolePermission == null || rolePermission.IsDeleted)
                throw new KeyNotFoundException($"No se encontró la relación con ID {id}.");

            await _rolePermissionRepository.DeleteAsync(id);
            await _rolePermissionRepository.SaveChangesAsync();
        }

        public async Task<(List<RolePermissionResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<RolePermission, RolePermissionResponseDto>(request, mapperConfig);
        }

        public async Task<RolePermissionResponseDto?> GetByIdAsync(int id)
        {
            var rolePermission = await _rolePermissionRepository.GetByIdAsync(id);
            if (rolePermission == null)
                throw new KeyNotFoundException($"No se encontró la relación con ID {id}.");

            return _mapper.Map<RolePermissionResponseDto>(rolePermission);
        }
    }
}