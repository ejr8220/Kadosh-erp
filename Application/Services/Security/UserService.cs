using Application.Dtos.Request;
using Application.Dtos.Request.Security;
using Application.Dtos.Response.Security;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Security;
using Domain.Interfaces;

namespace Application.Services.Security
{
    public class UserService : ICrudService<UserRequestDto, UserResponseDto>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public UserService(
            IRepository<User> userRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(UserRequestDto dto)
        {
            var exists = await _userRepository.AnyAsync(u => u.UserCode == dto.UserCode && !u.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"Ya existe un usuario con el código '{dto.UserCode}'.");

            var entity = _mapper.Map<User>(dto);
            await _userRepository.AddAsync(entity);
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UserRequestDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null || user.IsDeleted)
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

            user.UserCode = dto.UserCode;
            user.Password = dto.Password;
            user.Email = dto.Email;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null || user.IsDeleted)
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

            await _userRepository.DeleteAsync(id);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<(List<UserResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<User, UserResponseDto>(request, mapperConfig);
        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

            return _mapper.Map<UserResponseDto>(user);
        }
    }
}