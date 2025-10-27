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
    public class ProvinceService : ICrudService<ProvinceRequestDto, ProvinceResponseDto>
    {
        private readonly IRepository<Province> _provinceRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public ProvinceService(
            IRepository<Province> provinceRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _provinceRepository = provinceRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(ProvinceRequestDto dto)
        {
            var exists = await _provinceRepository.AnyAsync(p => p.Name == dto.Name && p.CountryId == dto.CountryId && !p.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"Province '{dto.Name}' already exists.");

            var entity = _mapper.Map<Province>(dto);
            await _provinceRepository.AddAsync(entity);
            await _provinceRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ProvinceRequestDto dto)
        {
            var province = await _provinceRepository.GetByIdAsync(id);
            if (province == null || province.IsDeleted)
                throw new KeyNotFoundException($"Province with ID {id} not found.");

            province.Name = dto.Name;
            province.CountryId = dto.CountryId;

            await _provinceRepository.UpdateAsync(province);
            await _provinceRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var province = await _provinceRepository.GetByIdAsync(id);
            if (province == null || province.IsDeleted)
                throw new KeyNotFoundException($"Province with ID {id} not found.");

            await _provinceRepository.DeleteAsync(id);
            await _provinceRepository.SaveChangesAsync();
        }

        public async Task<(List<ProvinceResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<Province, ProvinceResponseDto>(request, mapperConfig);
        }

        public async Task<ProvinceResponseDto?> GetByIdAsync(int id)
        {
            var province = await _provinceRepository.GetByIdAsync(id);
            if (province == null)
                throw new KeyNotFoundException($"Province with ID {id} not found.");

            return _mapper.Map<ProvinceResponseDto>(province);
        }
    }
}