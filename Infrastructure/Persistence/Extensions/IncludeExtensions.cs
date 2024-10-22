using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Extensions
{
    internal static class IncludeExtensions
    {
        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> query, IEnumerable<Expression<Func<TEntity, object>>> includes) where TEntity: BaseEntity<long>
            => includes.Aggregate(query, (acc, include) => acc.Include(include));
    }
}
