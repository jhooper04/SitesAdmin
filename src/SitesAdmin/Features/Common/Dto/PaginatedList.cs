using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace SitesAdmin.Features.Common
{
    public class PaginatedList<T>
    {
        public IReadOnlyCollection<T> Items { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public const int DefaultPageSize = 10;

        public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        private static async Task<(List<T> Items, int Count)> _PagedQuery(IQueryable<T> source, int pageNumber, int pageSize)
        {
            return (await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(), await source.CountAsync());
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int? pageNumber = null, int? pageSize = null)
        {
            pageNumber = pageNumber ?? 1;
            pageSize = pageSize ?? DefaultPageSize;

            var result = await _PagedQuery(source, pageNumber.Value, pageSize.Value);
            return new PaginatedList<T>(result.Items, result.Count, pageNumber.Value, pageSize.Value);
        }

        public static async Task<PaginatedList<TOut>> CreateAsync<TOut>(IMapper mapper, IQueryable<T> source, int? pageNumber=null, int? pageSize = null)
        {
            pageNumber = pageNumber ?? 1;
            pageSize = pageSize ?? DefaultPageSize;

            var result = await _PagedQuery(source, pageNumber.Value, pageSize.Value);

            var mappedItems = mapper.Map<List<TOut>>(result.Items);
            return new PaginatedList<TOut>(mappedItems, result.Count, pageNumber.Value, pageSize.Value);
        }
    }
}
