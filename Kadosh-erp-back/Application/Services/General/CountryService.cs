using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response.General;

using Application.Interfaces;
using AutoMapper;
using Domain.Entities.General;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.General
{
    public class CountryService : ICrudService<CountryRequestDto, CountryResponseDto>
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public CountryService(
            IRepository<Country> countryRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _countryRepository = countryRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(CountryRequestDto dto)
        {
            var exists = await _countryRepository.AnyAsync(c => c.Name == dto.Name && !c.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"Country '{dto.Name}' already exists.");

            var entity = _mapper.Map<Country>(dto);
            await _countryRepository.AddAsync(entity);
            await _countryRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CountryRequestDto dto)
        {
            var country = await _countryRepository.GetByIdAsync(id);
            if (country == null || country.IsDeleted)
                throw new KeyNotFoundException($"Country with ID {id} not found.");

            country.Name = dto.Name;
            country.IsoCode = dto.IsoCode;

            await _countryRepository.UpdateAsync(country);
            await _countryRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var country = await _countryRepository.GetByIdAsync(id);
            if (country == null || country.IsDeleted)
                throw new KeyNotFoundException($"Country with ID {id} not found.");

            await _countryRepository.DeleteAsync(id);
            await _countryRepository.SaveChangesAsync();
        }

        public async Task<(List<CountryResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<Country, CountryResponseDto>(request, mapperConfig);
        }

        public async Task<CountryResponseDto?> GetByIdAsync(int id)
        {
            var country = await _countryRepository.GetByIdAsync(id);
            if (country == null)
                throw new KeyNotFoundException($"Country with ID {id} not found.");

            return _mapper.Map<CountryResponseDto>(country);
        }
    }
}