using Application.Dtos.Request.General;
using Application.Dtos.Response.General;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.Enums;
using Domain.Entities.General;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services.General
{
    public class ParameterService : IParameterService
    {
        private readonly IRepository<ParameterHeader> _headerRepository;
        private readonly IRepository<ParameterDetail> _detailRepository;
        private readonly IMapper _mapper;

        public ParameterService(
            IRepository<ParameterHeader> headerRepository,
            IRepository<ParameterDetail> detailRepository,
            IMapper mapper)
        {
            _headerRepository = headerRepository;
            _detailRepository = detailRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateHeaderAsync(ParameterHeaderRequestDto request)
        {
            var code = request.Code.Trim().ToUpperInvariant();

            var exists = await _headerRepository.AnyAsync(x => x.Code == code && !x.IsDeleted);
            if (exists)
            {
                throw new UserException($"Ya existe un parametro con codigo '{code}'.");
            }

            var header = new ParameterHeader
            {
                Code = code,
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                Scope = request.Scope
            };

            await _headerRepository.AddAsync(header);
            await _headerRepository.SaveChangesAsync();

            return header.Id;
        }

        public async Task<int> CreateDetailAsync(ParameterDetailRequestDto request)
        {
            var header = await _headerRepository.GetByIdAsync(request.ParameterHeaderId);
            if (header is null)
            {
                throw new UserException("Cabecera de parametro no encontrada.");
            }

            if (header.Scope == ParameterScope.System && request.CompanyId.HasValue)
            {
                throw new UserException("Un parametro de sistema no admite CompanyId.");
            }

            if (header.Scope == ParameterScope.Company && !request.CompanyId.HasValue)
            {
                throw new UserException("Un parametro de empresa requiere CompanyId.");
            }

            var detail = new ParameterDetail
            {
                ParameterHeaderId = request.ParameterHeaderId,
                CompanyId = request.CompanyId,
                Value1 = request.Value,
                EffectiveFrom = request.EffectiveFrom,
                EffectiveTo = request.EffectiveTo,
                IsActive = request.IsActive
            };

            await _detailRepository.AddAsync(detail);
            await _detailRepository.SaveChangesAsync();

            return detail.Id;
        }

        public async Task<List<ParameterHeaderResponseDto>> GetHeadersAsync()
        {
            var headers = await _headerRepository.GetAllAsync();
            var details = await _detailRepository.GetAllAsync();

            var detailLookup = details
                .GroupBy(x => x.ParameterHeaderId)
                .ToDictionary(g => g.Key, g => g.Select(_mapper.Map<ParameterDetailResponseDto>).ToList());

            return headers.Select(h => new ParameterHeaderResponseDto
            {
                Id = h.Id,
                Code = h.Code,
                Name = h.Name,
                Description = h.Description,
                Scope = h.Scope,
                Details = detailLookup.TryGetValue(h.Id, out var values) ? values : new List<ParameterDetailResponseDto>()
            }).ToList();
        }

        public async Task<string?> ResolveValueAsync(string code, int? companyId, DateTime? at = null)
        {
            var now = at ?? DateTime.UtcNow;
            var normalizedCode = code.Trim().ToUpperInvariant();

            var headerSet = await _headerRepository.FindAsync(x => x.Code == normalizedCode && !x.IsDeleted);
            var header = headerSet.FirstOrDefault();
            if (header is null)
            {
                return null;
            }

            var detailSet = await _detailRepository.FindAsync(x =>
                x.ParameterHeaderId == header.Id &&
                x.IsActive &&
                x.EffectiveFrom <= now &&
                (x.EffectiveTo == null || x.EffectiveTo >= now) &&
                !x.IsDeleted);

            var detailList = detailSet.ToList();

            if (companyId.HasValue)
            {
                var companySpecific = detailList
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.EffectiveFrom)
                    .FirstOrDefault();

                if (companySpecific is not null)
                {
                    return companySpecific.Value1;
                }
            }

            var systemFallback = detailList
                .Where(x => x.CompanyId == null)
                .OrderByDescending(x => x.EffectiveFrom)
                .FirstOrDefault();

            return systemFallback?.Value1;
        }
    }
}
