using Application.Dtos.Request;
using Application.Dtos.Request.Tax;
using Application.Dtos.Response.Tax;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Tax;
using Domain.Interfaces;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Application.Services.Tax
{
    public class CompanyTypeService : ICrudService<CompanyTypeRequestDto, CompanyTypeResponseDto>
    {
        private readonly IRepository<CompanyType> _companyTypeRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public CompanyTypeService(
            IRepository<CompanyType> companyTypeRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _companyTypeRepository = companyTypeRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(CompanyTypeRequestDto dto)
        {
            var exists = await _companyTypeRepository.AnyAsync(i => i.Code == dto.Code && !i.IsDeleted);
            if (exists)
            {
                throw new InvalidOperationException($"CompanyType '{dto.Code}' already exists.");
            }

            var entity = _mapper.Map<CompanyType>(dto);
            await _companyTypeRepository.AddAsync(entity);
            await _companyTypeRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CompanyTypeRequestDto dto)
        {
            var item = await _companyTypeRepository.GetByIdAsync(id);
            if (item == null || item.IsDeleted)
            {
                throw new KeyNotFoundException($"CompanyType with ID {id} not found.");
            }

            item.Code = dto.Code;
            item.Name = dto.Name;
            item.PersonType = dto.PersonType;
            item.Description = dto.Description;

            await _companyTypeRepository.UpdateAsync(item);
            await _companyTypeRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _companyTypeRepository.GetByIdAsync(id);
            if (item == null || item.IsDeleted)
            {
                throw new KeyNotFoundException($"CompanyType with ID {id} not found.");
            }

            await _companyTypeRepository.DeleteAsync(id);
            await _companyTypeRepository.SaveChangesAsync();
        }

        public async Task<(List<CompanyTypeResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<CompanyType, CompanyTypeResponseDto>(request, mapperConfig);
        }

        public async Task<CompanyTypeResponseDto?> GetByIdAsync(int id)
        {
            var item = await _companyTypeRepository.GetByIdAsync(id);
            if (item == null || item.IsDeleted)
            {
                throw new KeyNotFoundException($"CompanyType with ID {id} not found.");
            }

            return _mapper.Map<CompanyTypeResponseDto>(item);
        }
    }
}
