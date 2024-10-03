using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using System.Linq.Expressions;
using static Shared.Common.Enums.SearchInKey;

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
            string orderByProperty = string.Empty;

            if (containsMethod == null)
                throw new InvalidOperationException("String.Contains method not found");

            Expression finalExpression = null;

            // Check if T is ApplicationUser
            if (typeof(T).Name == typeof(ApplicationUser).Name)
            {
                switch (pagination.SearchIn)
                {
                    case ApplicationUser.UserName:
                        var userNameProperty = Expression.Property(parameter, nameof(ApplicationUser.UserName));
                        var userNameContainsExpression = Expression.Call(userNameProperty, containsMethod, searchKey);
                        finalExpression = userNameContainsExpression;
                        break;

                    case ApplicationUser.Email:
                        var emailProperty = Expression.Property(parameter, nameof(ApplicationUser.Email));
                        var emailContainsExpression = Expression.Call(emailProperty, containsMethod, searchKey);
                        if (finalExpression == null)
                        {
                            finalExpression = emailContainsExpression;
                        }
                        else
                        {
                            finalExpression = Expression.OrElse(finalExpression, emailContainsExpression);
                        }
                        break;

                    case ApplicationUser.PhoneNumber:
                        var phoneNumberProperty = Expression.Property(parameter, nameof(ApplicationUser.PhoneNumber));
                        var phoneNumberContainsExpression = Expression.Call(phoneNumberProperty, containsMethod, searchKey);
                        if (finalExpression == null)
                        {
                            finalExpression = phoneNumberContainsExpression;
                        }
                        else
                        {
                            finalExpression = Expression.OrElse(finalExpression, phoneNumberContainsExpression);
                        }
                        break;

                    default:
                        throw new ArgumentException("Invalid SearchIn value.");
                }
                orderByProperty = ApplicationUser.UserName;
            }

            // Check if we have a valid final expression to build the predicate
            if (finalExpression != null)
            {
                var predicate = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
                data = data.Where(predicate);
            }

            // Always order by the appropriate property

            var orderByLambda = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(parameter, orderByProperty), typeof(object)), parameter);

            data = pagination.OrderBy == DESC ?
                   data.OrderByDescending(orderByLambda) :
                   data.OrderBy(orderByLambda);

            return data;
        }


        public static IQueryable<T> ApplyDateFilters<T>(IQueryable<T> data, PaginationSearchModel pagination)
        {

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, "CreatedDate");
            var fromDate = Expression.Constant(pagination.FromDate, typeof(DateTime));
            var toDate = Expression.Constant(pagination.ToDate ?? DateTime.Now, typeof(DateTime));
            var greaterThanOrEqual = Expression.GreaterThanOrEqual(property, fromDate);
            var lessThanOrEqual = Expression.LessThanOrEqual(property, toDate);
            var andExpression = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

            var predicate = Expression.Lambda<Func<T, bool>>(andExpression, parameter);
            data = data.Where(predicate);

            data = pagination.OrderBy == DESC ?
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

