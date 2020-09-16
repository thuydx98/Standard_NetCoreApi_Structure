using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StandardApi.Common.Paging
{
    public class PagedList<T>
    {
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (pageSize < 0)
            {
                throw new ArgumentException("pageSize must be greater than zero");
            }

            TotalCount = totalCount;
            PageSize = pageSize;
            PageIndex = pageIndex;
            Items = source.ToArray();
        }

        public T[] Items { get; }
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalCount { get; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var totalCount = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, pageIndex, pageSize, totalCount);
        }
    }
}
