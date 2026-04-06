using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response.General;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.General;
using Domain.Entities.Security;
using Domain.Interfaces;
using Kadosh_erp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.General
{
    public class CompanyService : ICrudService<CompanyRequestDto, CompanyResponseDto>
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Branch> _branchRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public CompanyService(
            IRepository<Company> companyRepository,
            IRepository<Person> personRepository,
            IRepository<Branch> branchRepository,
            IGenericFilterService filterService,
            IMapper mapper,
            AppDbContext context)
        {
            _companyRepository = companyRepository;
            _personRepository = personRepository;
            _branchRepository = branchRepository;
            _filterService = filterService;
            _mapper = mapper;
            _context = context;
        }

        public async Task AddAsync(CompanyRequestDto dto)
        {
            var legalIdentification = FirstNotEmpty(
                dto.LegalpresentativeIdentification,
                dto.LegalRepresentativeIdentification,
                dto.Identificacion);

            var exists = await _companyRepository.AnyAsync(c =>
                c.LegalpresentativeIdentification == legalIdentification &&
                !c.IsDeleted);

            if (exists)
                throw new InvalidOperationException($"Ya existe una empresa con cédula {legalIdentification}.");

            var personId = dto.PersonId > 0 ? dto.PersonId : await CreatePersonAsync(dto, legalIdentification);

            var entity = new Company
            {
                PersonId = personId,
                CompanyTypeId = dto.CompanyTypeId ?? 1,
                LegalpresentativeIdentification = legalIdentification,
                LegalRepresentative = FirstNotEmpty(
                    dto.LegalRepresentative,
                    dto.LegalRepresentativeName,
                    dto.RazonSocial,
                    dto.NombreComercial),
                AccountantIdentification = FirstNotEmpty(dto.AccountantIdentification),
                Accountant = FirstNotEmpty(dto.Accountant, dto.AccountantName),
                LogoUrl = FirstNotEmpty(dto.LogoUrl)
            };

            await _companyRepository.AddAsync(entity);
            await _companyRepository.SaveChangesAsync();

            if (dto.Branches is { Count: > 0 })
            {
                var requestCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var branchDto in dto.Branches)
                {
                    var code = FirstNotEmpty(branchDto.Code).ToUpperInvariant();
                    var name = FirstNotEmpty(branchDto.Name);

                    if (!requestCodes.Add(code))
                        throw new InvalidOperationException($"Existe una sucursal repetida con el código '{code}' en la solicitud.");

                    var existsBranch = await _branchRepository.AnyAsync(b =>
                        b.CompanyId == entity.Id &&
                        b.Code == code &&
                        !b.IsDeleted);

                    if (existsBranch)
                        throw new InvalidOperationException($"Ya existe una sucursal con el código '{code}'.");

                    await _branchRepository.AddAsync(new Branch
                    {
                        CompanyId = entity.Id,
                        Code = code,
                        Name = name,
                        Email = string.IsNullOrWhiteSpace(branchDto.Email) ? null : branchDto.Email.Trim(),
                        Phone = string.IsNullOrWhiteSpace(branchDto.Phone) ? null : branchDto.Phone.Trim(),
                        Address = string.IsNullOrWhiteSpace(branchDto.Address) ? null : branchDto.Address.Trim()
                    });
                }

                await _branchRepository.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(int id, CompanyRequestDto dto)
        {
            var company = await _context.Set<Company>()
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (company == null)
                throw new KeyNotFoundException($"Empresa con ID {id} no encontrada.");

            var legalIdentification = FirstNotEmpty(
                dto.LegalpresentativeIdentification,
                dto.LegalRepresentativeIdentification,
                dto.Identificacion,
                company.LegalpresentativeIdentification);

            company.CompanyTypeId = dto.CompanyTypeId ?? company.CompanyTypeId;
            company.LegalpresentativeIdentification = legalIdentification;
            company.LegalRepresentative = FirstNotEmpty(
                dto.LegalRepresentative,
                dto.LegalRepresentativeName,
                company.LegalRepresentative);
            company.AccountantIdentification = FirstNotEmpty(dto.AccountantIdentification, company.AccountantIdentification);
            company.Accountant = FirstNotEmpty(dto.Accountant, dto.AccountantName, company.Accountant);
            company.LogoUrl = FirstNotEmpty(dto.LogoUrl, company.LogoUrl);

            // Actualizar datos de la persona asociada
            if (company.Person != null)
            {
                company.Person.FirstName = FirstNotEmpty(dto.Nombres, dto.RazonSocial, dto.NombreComercial, company.Person.FirstName);
                company.Person.LastName = FirstNotEmpty(dto.Apellidos, dto.NombreComercial, company.Person.LastName);
                if (dto.CountryId.HasValue) company.Person.CountryId = dto.CountryId.Value;
                if (dto.ProvinceId.HasValue) company.Person.ProvinceId = dto.ProvinceId.Value;
                if (dto.CityId.HasValue) company.Person.CityId = dto.CityId.Value;
                if (!string.IsNullOrWhiteSpace(dto.Address)) company.Person.Address = dto.Address;
                else if (!string.IsNullOrWhiteSpace(dto.Direccion)) company.Person.Address = dto.Direccion;
            }

            await _context.SaveChangesAsync();
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
            var company = await _context.Set<Company>()
                .Include(c => c.CompanyType)
                .Include(c => c.Person)
                    .ThenInclude(p => p.Country)
                .Include(c => c.Person)
                    .ThenInclude(p => p.Province)
                .Include(c => c.Person)
                    .ThenInclude(p => p.City)
                .Include(c => c.Person)
                    .ThenInclude(p => p.ContactFormPersons)
                        .ThenInclude(cfp => cfp.ContactForm)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (company == null)
                throw new KeyNotFoundException($"Empresa con ID {id} no encontrada.");

            return _mapper.Map<CompanyResponseDto>(company);
        }

        private async Task<int> CreatePersonAsync(CompanyRequestDto dto, string fallbackIdentification)
        {
            var identificationNumber = FirstNotEmpty(dto.Identificacion, fallbackIdentification);
            var cityId = dto.CityId ?? 0;
            var person = new Person
            {
                IdentificationTypeId = dto.IdentificationTypeId ?? (identificationNumber.Length == 13 ? 2 : 1),
                IdentificationNumber = identificationNumber,
                FirstName = FirstNotEmpty(dto.Nombres, dto.RazonSocial, dto.NombreComercial),
                LastName = FirstNotEmpty(dto.Apellidos, dto.NombreComercial),
                CountryId = dto.CountryId ?? 1,
                ProvinceId = dto.ProvinceId ?? 1,
                CityId = cityId > 0 ? cityId : 1,
                ParishId = dto.ParishId ?? (cityId > 0 ? cityId : 1),
                Address = FirstNotEmpty(dto.Address, dto.Direccion)
            };

            await _personRepository.AddAsync(person);
            await _personRepository.SaveChangesAsync();
            return person.Id;
        }

        private static string FirstNotEmpty(params string[] values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                    return value.Trim();
            }

            return string.Empty;
        }
    }
}