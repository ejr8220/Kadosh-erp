using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response.General;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.General;
using Domain.Interfaces;

namespace Application.Services.General
{
    public class BranchService : ICrudService<BranchRequestDto, BranchResponseDto>
    {
        private readonly IRepository<Branch> _branchRepository;
        private readonly IGenericFilterService _filterService;
        private readonly IMapper _mapper;

        public BranchService(
            IRepository<Branch> branchRepository,
            IGenericFilterService filterService,
            IMapper mapper)
        {
            _branchRepository = branchRepository;
            _filterService = filterService;
            _mapper = mapper;
        }

        public async Task AddAsync(BranchRequestDto dto)
        {
            var exists = await _branchRepository.AnyAsync(b => b.Code == dto.Code && !b.IsDeleted);
            if (exists)
                throw new InvalidOperationException($"Ya existe una sucursal con el código '{dto.Code}'.");

            var entity = _mapper.Map<Branch>(dto);
            await _branchRepository.AddAsync(entity);
            await _branchRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, BranchRequestDto dto)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null || branch.IsDeleted)
                throw new KeyNotFoundException($"Sucursal con ID {id} no encontrada.");

            branch.Code = dto.Code;
            branch.Name = dto.Name;
            branch.Address = dto.Address;
            branch.Phone = dto.Phone;
            branch.Email = dto.Email;
            branch.CompanyId = dto.CompanyId;

            await _branchRepository.UpdateAsync(branch);
            await _branchRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null || branch.IsDeleted)
                throw new KeyNotFoundException($"Sucursal con ID {id} no encontrada.");

            await _branchRepository.DeleteAsync(id);
            await _branchRepository.SaveChangesAsync();
        }

        public async Task<(List<BranchResponseDto> result, int count)> GetAllFilterAsync(
            CustomDataManagerRequest request,
            IConfigurationProvider mapperConfig)
        {
            return await _filterService.FilterAsync<Branch, BranchResponseDto>(request, mapperConfig);
        }

        public async Task<BranchResponseDto?> GetByIdAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null)
                throw new KeyNotFoundException($"Sucursal con ID {id} no encontrada.");

            return _mapper.Map<BranchResponseDto>(branch);
        }
    }
}