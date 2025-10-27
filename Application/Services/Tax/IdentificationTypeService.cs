using Application.Dtos.Request;
using Application.Dtos.Request.Tax;
using Application.Dtos.Response.Tax;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Tax;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Application.Services.Tax
{
    public class IdentificationTypeService : ICrudService<IdentificationTypeRequestDto, IdentificationTypeResponseDto>
    {
        private readonly IRepository<IdentificationType> _identificationTypeRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public IdentificationTypeService(
            IRepository<IdentificationType> identificationTypeRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _identificationTypeRepository = identificationTypeRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(IdentificationTypeRequestDto dto)
        {
            var exists = await _identificationTypeRepository.AnyAsync(i => i.Code == dto.Code && !i.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"IdentificationType '{dto.Code}' already exists.");

            var entity = _mapper.Map<IdentificationType>(dto);
            await _identificationTypeRepository.AddAsync(entity);
            await _identificationTypeRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, IdentificationTypeRequestDto dto)
        {
            var item = await _identificationTypeRepository.GetByIdAsync(id);
            if (item == null || item.IsDeleted)
                throw new KeyNotFoundException($"IdentificationType with ID {id} not found.");

            item.Name = dto.Name;
            item.Code = dto.Code;
            item.Maxlength = dto.Maxlength;

            await _identificationTypeRepository.UpdateAsync(item);
            await _identificationTypeRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _identificationTypeRepository.GetByIdAsync(id);
            if (item == null || item.IsDeleted)
                throw new KeyNotFoundException($"IdentificationType with ID {id} not found.");

            await _identificationTypeRepository.DeleteAsync(id);
            await _identificationTypeRepository.SaveChangesAsync();
        }

        public async Task<(List<IdentificationTypeResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<IdentificationType, IdentificationTypeResponseDto>(request, mapperConfig);
        }

        public async Task<IdentificationTypeResponseDto?> GetByIdAsync(int id)
        {
            var item = await _identificationTypeRepository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"IdentificationType with ID {id} not found.");

            return _mapper.Map<IdentificationTypeResponseDto>(item);
        }
    }
}