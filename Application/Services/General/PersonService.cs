using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response.General;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.General;
using Domain.Interfaces;

namespace Application.Services.General
{
    public class PersonService : ICrudService<PersonRequestDto, PersonResponseDto>
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public PersonService(
            IRepository<Person> personRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _personRepository = personRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(PersonRequestDto dto)
        {
            var exists = await _personRepository.AnyAsync(p => p.IdentificationNumber == dto.IdentificationNumber && !p.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"Person with IdentificationNumber '{dto.IdentificationNumber}' already exists.");

            var entity = _mapper.Map<Person>(dto);
            await _personRepository.AddAsync(entity);
            await _personRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, PersonRequestDto dto)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null || person.IsDeleted)
                throw new KeyNotFoundException($"Person with ID {id} not found.");

            if (!string.Equals(person.IdentificationNumber, dto.IdentificationNumber, StringComparison.OrdinalIgnoreCase))
            {
                var otherExists = await _personRepository.AnyAsync(p => p.IdentificationNumber == dto.IdentificationNumber && p.Id != id && !p.IsDeleted);
                if (otherExists)
                    throw new InvalidOperationException($"Another person with IdentificationNumber '{dto.IdentificationNumber}' already exists.");
            }

            person.IdentificationTypeId = dto.IdentificationTypeId;
            person.IdentificationNumber = dto.IdentificationNumber;
            person.FirstName = dto.FirstName;
            person.LastName = dto.LastName;
            person.BirthDate = dto.BirthDate;
            person.Gender = dto.Gender;
            person.CountryId = dto.CountryId;
            person.ProvinceId = dto.ProvinceId;
            person.CityId = dto.CityId;
            person.ParishId = dto.ParishId;

            await _personRepository.UpdateAsync(person);
            await _personRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null || person.IsDeleted)
                throw new KeyNotFoundException($"Person with ID {id} not found.");

            await _personRepository.DeleteAsync(id); // activa EntityState.Deleted
            await _personRepository.SaveChangesAsync(); // interceptor aplica borrado lógico
        }

        public async Task<(List<PersonResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<Person, PersonResponseDto>(request, mapperConfig);
        }

        public async Task<PersonResponseDto?> GetByIdAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null)
                throw new KeyNotFoundException($"Person with ID {id} not found.");

            return _mapper.Map<PersonResponseDto>(person);
        }
    }
}