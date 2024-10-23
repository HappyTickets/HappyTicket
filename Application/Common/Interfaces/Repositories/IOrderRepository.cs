using Domain.Entities;
using Shared.Common.General;

namespace Application.Common.Interfaces.Persistence
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<PaginatedList<Order>> GetAllOrdersAsync(PaginationSearchModel paginationSearchModel,
            IEnumerable<string>? includes = null, CancellationToken cancellationToken = default);
    }
}
