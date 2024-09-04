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

        public static IQueryable<T> AddOrderBy(IQueryable<T> source, string? orderBy, bool? orderDesc)
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                var properties = typeof(T).GetProperties();
                var property = properties.FirstOrDefault(p => string.Equals(p.Name, orderBy, StringComparison.OrdinalIgnoreCase));

                if (property != null)
                {
                    if (orderDesc != true)
                    {
                        source = source.OrderBy((p) => EF.Property<object>(p, property.Name ?? "Id")).AsQueryable();
                    }
                    else
                    {
                        source = source.OrderByDescending((p) => EF.Property<object>(p, property.Name ?? "Id")).AsQueryable();
                    }
                }
                else throw new ArgumentException("Invalid sort column specified");
            }

            return source;
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int? pageNumber = null, int? pageSize = null, string? orderBy = null, bool? orderDesc = null)
        {
            pageNumber = pageNumber ?? 1;
            pageSize = pageSize ?? DefaultPageSize;

            source = AddOrderBy(source, orderBy, orderDesc);

            var result = await _PagedQuery(source, pageNumber.Value, pageSize.Value);
            return new PaginatedList<T>(result.Items, result.Count, pageNumber.Value, pageSize.Value);
        }

        public static async Task<PaginatedList<TOut>> CreateAsync<TOut>(IMapper mapper, IQueryable<T> source, int? pageNumber=null, int? pageSize = null, string? orderBy = null, bool? orderDesc = null)
        {
            pageNumber = pageNumber ?? 1;
            pageSize = pageSize ?? DefaultPageSize;

            source = AddOrderBy(source, orderBy, orderDesc);

            //if (!string.IsNullOrEmpty(orderBy))
            //{
            //    var properties = typeof(T).GetProperties();
            //    var property = properties.FirstOrDefault(p => string.Equals(p.Name, orderBy, StringComparison.OrdinalIgnoreCase));

            //    if (property != null)
            //    {
            //        if (orderDesc != true)
            //        {
            //            source = source.OrderBy((p) => EF.Property<object>(p, property.Name ?? "Id")).AsQueryable();
            //        }
            //        else
            //        {
            //            source = source.OrderByDescending((p) => EF.Property<object>(p, property.Name ?? "Id")).AsQueryable();
            //        }
            //    }
            //    else throw new ArgumentException("Invalid sort column specified");
            //}

            var result = await _PagedQuery(source, pageNumber.Value, pageSize.Value);

            var mappedItems = new List<TOut>();
            foreach (var item in result.Items)
            {
                mappedItems.Add(mapper.Map<T, TOut>(item));
            }
            //var mappedItems = mapper.Map<List<TOut>>(result.Items);
            return new PaginatedList<TOut>(mappedItems, result.Count, pageNumber.Value, pageSize.Value);
        }
    }
}
