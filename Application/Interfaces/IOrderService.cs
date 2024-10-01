using Domain.Entities;
using LanguageExt.Common;
using Shared.DTOs;
using Shared.DTOs.TicketDTOs;

namespace Application.Interfaces
{
    public interface IOrderService : IBaseService<Order, OrderDto>
    {
        Task<Result<IEnumerable<TicketDto>>> GetTicketsByUserIdAsync(string userId, bool useCache = false, CancellationToken cancellationToken = default);
    }
}
