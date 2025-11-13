using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response.General;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Security;
using Domain.Interfaces;

namespace Application.Services.General
{
    public class CompanyService : ICrudService<CompanyRequestDto, CompanyResponseDto>
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public CompanyService(
            IRepository<Company> companyRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _companyRepository = companyRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(CompanyRequestDto dto)
        {
            var exists = await _companyRepository.AnyAsync(c =>
                c.LegalpresentativeIdentification == dto.LegalpresentativeIdentification &&
                !c.IsDeleted);

            if (exists)
                throw new InvalidOperationException($"Ya existe una empresa con cédula {dto.LegalpresentativeIdentification}.");

            var entity = _mapper.Map<Company>(dto);
            await _companyRepository.AddAsync(entity);
            await _companyRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CompanyRequestDto dto)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null || company.IsDeleted)
                throw new KeyNotFoundException($"Empresa con ID {id} no encontrada.");

            company.PersonId = dto.PersonId;
            company.LegalpresentativeIdentification = dto.LegalpresentativeIdentification;
            company.LegalRepresentative = dto.LegalRepresentative;
            company.AccountantIdentification = dto.AccountantIdentification;
            company.Accountant = dto.Accountant;
            company.LogoUrl = dto.LogoUrl;

            await _companyRepository.UpdateAsync(company);
            await _companyRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null || company.IsDeleted)
                throw new KeyNotFoundException($"Empresa con ID {id} no encontrada.");

            await _companyRepository.DeleteAsync(id);
            await _companyRepository.SaveChangesAsync();
        }

        public async Task<(List<CompanyResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<Company, CompanyResponseDto>(request, mapperConfig);
        }

        public async Task<CompanyResponseDto?> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
                throw new KeyNotFoundException($"Empresa con ID {id} no encontrada.");

            return _mapper.Map<CompanyResponseDto>(company);
        }
    }
}