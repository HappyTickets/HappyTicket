using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Extensions
{
    internal static class GlobalQueryFilterExtension
    {
        public static ModelBuilder AppendGlobalQueryFilter<TEntity>(this ModelBuilder builder, Expression<Func<TEntity, bool>> filter)
        {
            var entities = builder.Model
                .GetEntityTypes()
                .Where(e => e.ClrType.IsAssignableTo(typeof(TEntity)));

            foreach (var entity in entities)
            {
                var param = Expression.Parameter(entity.ClrType);
                var body = ReplacingExpressionVisitor.Replace(filter.Parameters.Single(), param, filter.Body);

                if(entity.GetQueryFilter() is { } existingFilter)
                {
                    var existingFilterBody = ReplacingExpressionVisitor.Replace(existingFilter.Parameters.Single(), param, existingFilter.Body);
                    body = Expression.AndAlso(existingFilterBody, body);
                }

                entity.SetQueryFilter(Expression.Lambda(body, param));
            }

            return builder;
        }
    }
}
