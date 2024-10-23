using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Persistence.EntityFramework;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.General;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    internal class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ICurrentUser _currentUser;

        public OrderRepository(AppDbContext context, ICurrentUser currentUser) : base(context)
        {
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<Order>> GetAllOrdersAsync(PaginationSearchModel paginationSearchModel, IEnumerable<string>? includes = null, CancellationToken cancellationToken = default)
        {
            var ordersQuery = Query(); // Start with the base query

            if (includes != null)
            {
                ordersQuery = ordersQuery.Include(includes);
            }

            // Apply search filters based on the SearchIn property
            if (!string.IsNullOrEmpty(paginationSearchModel.SearchKey))
            {
                var searchKey = paginationSearchModel.SearchKey.ToLower();

                switch (paginationSearchModel.SearchIn)
                {
                    case SearchInKey.Order.OwnerName:
                        ordersQuery = ordersQuery.Where(order => order.User.UserName.ToLower().Contains(searchKey));
                        break;

                    case SearchInKey.Order.OwnerPhone:
                        ordersQuery = ordersQuery.Where(order => order.User.PhoneNumber.ToLower().Contains(searchKey));
                        break;

                    case SearchInKey.Order.OwnerEmail:
                        ordersQuery = ordersQuery.Where(order => order.User.Email.ToLower().Contains(searchKey));
                        break;

                    case SearchInKey.Order.CreatedDate:
                        if (DateTime.TryParse(searchKey, out DateTime parsedDate))
                        {
                            ordersQuery = ordersQuery.Where(order => order.CreatedDate.Date == parsedDate.Date);
                        }
                        break;

                    default:
                        throw new ArgumentException("Invalid SearchIn value.");
                }
            }

            if (paginationSearchModel.FromDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(order => order.CreatedDate >= paginationSearchModel.FromDate.Value);
            }

            if (paginationSearchModel.ToDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(order => order.CreatedDate <= paginationSearchModel.ToDate.Value);
            }

            if (!string.IsNullOrEmpty(paginationSearchModel.OrderBy))
            {
                ordersQuery = ApplyOrdering(ordersQuery, paginationSearchModel.OrderBy, paginationSearchModel.IsDescending);
            }

            if (!paginationSearchModel.PaginationOff)
            {
                ordersQuery = ordersQuery.Skip(paginationSearchModel.PageIndex * paginationSearchModel.PageSize)
                                          .Take(paginationSearchModel.PageSize);
            }

            var ordersMatchingCount = await ordersQuery.CountAsync(cancellationToken);
            var filteredList = await ordersQuery.ToListAsync(cancellationToken);

            return new PaginatedList<Order>(filteredList, ordersMatchingCount, paginationSearchModel.PageIndex, paginationSearchModel.PageSize);
        }

        private IQueryable<Order> ApplyOrdering(IQueryable<Order> query, string orderBy, bool isDescending)
        {
            var orderByExpression = GetOrderByExpression(orderBy);

            if (isDescending)
            {
                return query.OrderByDescending(orderByExpression);
            }
            else
            {
                return query.OrderBy(orderByExpression);
            }
        }

        private Expression<Func<Order, object>> GetOrderByExpression(string orderByKey)
        {
            return orderByKey switch
            {
                SearchInKey.Order.CreatedDate => order => order.CreatedDate,
                SearchInKey.Order.OwnerName => order => order.User.UserName,
                SearchInKey.Order.OwnerPhone => order => order.User.PhoneNumber,
                SearchInKey.Order.OwnerEmail => order => order.User.Email,
                SearchInKey.Order.PaymentStatus => order => order.PaymentStatus,
                SearchInKey.Order.TotalAmount => order => order.TotalAmount,
                _ => throw new ArgumentException("Invalid OrderBy value.")
            };
        }

    }
}
