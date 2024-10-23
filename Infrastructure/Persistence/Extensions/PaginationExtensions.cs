using Microsoft.EntityFrameworkCore;
using Shared.Common.General;

namespace Infrastructure.Persistence.Extensions
{
    internal static class PaginationExtensions
    {
        public async static Task<PaginatedList<TEntity>> PaginateAsync<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize)
        {
            var items = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await query.LongCountAsync();

            return new PaginatedList<TEntity>(items, count, pageIndex, pageSize);
        }
    }
}
