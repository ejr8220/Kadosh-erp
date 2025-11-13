using Application.Dtos.Request;
using Application.Interfaces;
using Kadosh_erp.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Syncfusion.EJ2.Base;
using Domain.Common.Enums;

namespace Application.Services
{
    public class GenericFilterService : IGenericFilterService
{
    private readonly AppDbContext _context;

    public GenericFilterService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<TDto> result, int count)> FilterAsync<TEntity, TDto>(
        CustomDataManagerRequest request,
        IConfigurationProvider mapperConfig)
        where TEntity : class
    {
        request.Where ??= new();
        request.Sorted ??= new();
        request.Search ??= new();

        var queryable = _context.Set<TEntity>()
            .AsNoTracking()
            .ProjectTo<TDto>(mapperConfig);

        var queryableOperation = new QueryableOperation();

        var searchFilters = request.Search.Select(s => new Syncfusion.EJ2.Base.SearchFilter
        {
            Fields = s.Fields,
            Key = s.Key,
            Operator = s.Operator,
            IgnoreAccent = s.IgnoreAccent
        }).ToList();

        var whereFilters = request.Where.Select(w =>
        {
            var isComplex = w.Predicates?.Count > 0;

            return new Syncfusion.EJ2.Base.WhereFilter
            {
                Field = w.Field,
                Operator = w.Operator,
                value = w.Value,
                IgnoreCase = w.IgnoreCase,
                IgnoreAccent = w.IgnoreAccent,
                Condition = w.Condition,
                IsComplex = true,
                predicates = isComplex
                    ? w.Predicates.Select(p => new Syncfusion.EJ2.Base.WhereFilter
                    {
                        Field = p.Field,
                        Operator = p.Operator,
                        value = p.Value,
                        IgnoreCase = p.IgnoreCase,
                        IgnoreAccent = p.IgnoreAccent,
                        Condition = p.Condition
                    }).ToList()
                    : new List<Syncfusion.EJ2.Base.WhereFilter>
                    {
                        new()
                        {
                            Field = w.Field,
                            Operator = w.Operator,
                            value = w.Value,
                            IgnoreCase = w.IgnoreCase,
                            IgnoreAccent = w.IgnoreAccent,
                            Condition = w.Condition
                        }
                    }
            };
        }).ToList();

        var sortedFields = request.Sorted.Select(s =>
        {
            Enum.TryParse<SortDirection>(s.Direction, true, out var direction);
            return new Syncfusion.EJ2.Base.Sort
            {
                Name = s.Name,
                Direction = direction.ToString()
            };
        }).ToList();

        if (searchFilters.Count > 0)
            queryable = queryableOperation.PerformSearching(queryable, searchFilters);

        if (whereFilters.Count > 0)
        {
            foreach (var condition in whereFilters)
                queryable = queryableOperation.PerformFiltering(queryable, condition.predicates, condition.Condition);
        }

        if (sortedFields.Count > 0)
            queryable = queryableOperation.PerformSorting(queryable, sortedFields);

        var count = await queryable.CountAsync();

        if (request.Skip > 0)
            queryable = queryable.Skip(request.Skip);

        if (request.Take > 0)
            queryable = queryable.Take(request.Take);

        var result = await queryable.ToListAsync();
        return (result, count);
    }
}

}
