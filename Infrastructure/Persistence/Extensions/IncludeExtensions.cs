using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Extensions
{
    internal static class IncludeExtensions
    {
        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> query, IEnumerable<string> includes) where TEntity : BaseEntity<long>
            => includes.Aggregate(query, (acc, include) => acc.Include(include));
    }
}
