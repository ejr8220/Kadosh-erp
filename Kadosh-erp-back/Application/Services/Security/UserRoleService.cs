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
    public class UserRoleService : ICrudService<UserRoleRequestDto, UserRoleResponseDto>
    {
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public UserRoleService(
            IRepository<UserRole> userRoleRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(UserRoleRequestDto dto)
        {
            var exists = await _userRoleRepository.AnyAsync(ur =>
                ur.UserId == dto.UserId &&
                ur.RoleId == dto.RoleId &&
                !ur.IsDeleted);

            if (exists)
                throw new InvalidOperationException("La relación usuario-rol ya existe.");

            var entity = _mapper.Map<UserRole>(dto);
            await _userRoleRepository.AddAsync(entity);
            await _userRoleRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UserRoleRequestDto dto)
        {
            var userRole = await _userRoleRepository.GetByIdAsync(id);
            if (userRole == null || userRole.IsDeleted)
                throw new KeyNotFoundException($"No se encontró la relación con ID {id}.");

            userRole.UserId = dto.UserId;
            userRole.RoleId = dto.RoleId;

            await _userRoleRepository.UpdateAsync(userRole);
            await _userRoleRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var userRole = await _userRoleRepository.GetByIdAsync(id);
            if (userRole == null || userRole.IsDeleted)
                throw new KeyNotFoundException($"No se encontró la relación con ID {id}.");

            await _userRoleRepository.DeleteAsync(id);
            await _userRoleRepository.SaveChangesAsync();
        }

        public async Task<(List<UserRoleResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<UserRole, UserRoleResponseDto>(request, mapperConfig);
        }

        public async Task<UserRoleResponseDto?> GetByIdAsync(int id)
        {
            var userRole = await _userRoleRepository.GetByIdAsync(id);
            if (userRole == null)
                throw new KeyNotFoundException($"No se encontró la relación con ID {id}.");

            return _mapper.Map<UserRoleResponseDto>(userRole);
        }
    }
}