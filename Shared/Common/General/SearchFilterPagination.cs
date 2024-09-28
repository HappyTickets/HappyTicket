using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using System.Linq.Expressions;

namespace Shared.Common.General
{
    public static class SearchFilterPagination
    {
        public static IQueryable<T> Filter<T>(IQueryable<T> Data, PaginationSearchModel paginationSearchModel)
        {

            if (!string.IsNullOrEmpty(paginationSearchModel.SearchIn) && !string.IsNullOrEmpty(paginationSearchModel.SearchKey))
            { return ApplySearchFilters(Data, paginationSearchModel); }


            if (paginationSearchModel.FromDate is not null)
            {

                return ApplyDateFilters(Data, paginationSearchModel);
            }

            return Data;



        }


        private static IQueryable<T> ApplySearchFilters<T>(IQueryable<T> data, PaginationSearchModel pagination)
        {

            var parameter = Expression.Parameter(typeof(T), "x");
            var searchKey = Expression.Constant(pagination.SearchKey);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            if (containsMethod == null)
                throw new InvalidOperationException("String.Contains method not found");

            Expression finalExpression = null;

            // Build expressions based on the selected search fields
            var nameProperty = Expression.PropertyOrField(parameter, "Name");

            if (pagination.SearchIn == SearchInKey.Name || pagination.SearchIn == SearchInKey.Both)
            {
                var nameContainsExpression = Expression.Call(nameProperty, containsMethod, searchKey);
                finalExpression = nameContainsExpression;
            }

            if (pagination.SearchIn == SearchInKey.Description || pagination.SearchIn == SearchInKey.Both)
            {
                var descriptionProperty = Expression.PropertyOrField(parameter, "Description");
                var descriptionContainsExpression = Expression.Call(descriptionProperty, containsMethod, searchKey);
                if (finalExpression == null)
                {
                    finalExpression = descriptionContainsExpression;
                }
                else
                {
                    finalExpression = Expression.OrElse(finalExpression, descriptionContainsExpression);
                }
            }

            var predicate = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
            data = data.Where(predicate);

            // Always order by Name

            var orderByLambda = Expression.Lambda<Func<T, object>>(Expression.Convert(nameProperty, typeof(object)), parameter);

            data = pagination.OrderBy == SearchInKey.DESC ?
                   data.OrderByDescending(orderByLambda) :
                   data.OrderBy(orderByLambda);

            return data;
        }


        public static IQueryable<T> ApplyDateFilters<T>(IQueryable<T> data, PaginationSearchModel pagination)
        {

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, "CreationDate");
            var fromDate = Expression.Constant(pagination.FromDate, typeof(DateTime));
            var toDate = Expression.Constant(pagination.ToDate ?? DateTime.Now, typeof(DateTime));
            var greaterThanOrEqual = Expression.GreaterThanOrEqual(property, fromDate);
            var lessThanOrEqual = Expression.LessThanOrEqual(property, toDate);
            var andExpression = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

            var predicate = Expression.Lambda<Func<T, bool>>(andExpression, parameter);
            data = data.Where(predicate);

            data = pagination.OrderBy == SearchInKey.DESC ?
          data.OrderByDescending(predicate) :
         data.OrderBy(predicate);

            return data;
        }


        public async static Task<List<T>> PaginateData<T>(IQueryable<T> data, PaginationSearchModel pagination)
        {
            return await data.Skip(pagination.PageIndex * pagination.PageSize).Take(pagination.PageSize).ToListAsync();
        }
    }
}

