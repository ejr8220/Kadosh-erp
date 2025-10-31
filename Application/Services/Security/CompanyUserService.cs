using Application.Dtos.Request;
using Application.Dtos.Request.Security;
using Application.Dtos.Response.Security;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Security;
using Domain.Interfaces;

namespace Application.Services.Security
{
    public class CompanyUserService : ICrudService<CompanyUserRequestDto, CompanyUserResponseDto>
    {
        private readonly IRepository<CompanyUser> _companyUserRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public CompanyUserService(
            IRepository<CompanyUser> companyUserRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _companyUserRepository = companyUserRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(CompanyUserRequestDto dto)
        {
            var exists = await _companyUserRepository.AnyAsync(cu =>
                cu.CompanyId == dto.CompanyId &&
                cu.UserId == dto.UserId &&
                !cu.IsDeleted);

            if (exists)
                throw new InvalidOperationException("La relación empresa-usuario ya existe.");

            var entity = _mapper.Map<CompanyUser>(dto);
            await _companyUserRepository.AddAsync(entity);
            await _companyUserRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CompanyUserRequestDto dto)
        {
            var companyUser = await _companyUserRepository.GetByIdAsync(id);
            if (companyUser == null || companyUser.IsDeleted)
                throw new KeyNotFoundException($"No se encontró la relación con ID {id}.");

            companyUser.CompanyId = dto.CompanyId;
            companyUser.UserId = dto.UserId;

            await _companyUserRepository.UpdateAsync(companyUser);
            await _companyUserRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var companyUser = await _companyUserRepository.GetByIdAsync(id);
            if (companyUser == null || companyUser.IsDeleted)
                throw new KeyNotFoundException($"No se encontró la relación con ID {id}.");

            await _companyUserRepository.DeleteAsync(id);
            await _companyUserRepository.SaveChangesAsync();
        }

        public async Task<(List<CompanyUserResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<CompanyUser, CompanyUserResponseDto>(request, mapperConfig);
        }

        public async Task<CompanyUserResponseDto?> GetByIdAsync(int id)
        {
            var companyUser = await _companyUserRepository.GetByIdAsync(id);
            if (companyUser == null)
                throw new KeyNotFoundException($"No se encontró la relación con ID {id}.");

            return _mapper.Map<CompanyUserResponseDto>(companyUser);
        }
    }
}