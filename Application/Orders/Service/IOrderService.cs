using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.OrderDtos.Request;
using Shared.DTOs.OrderDtos.Response;

namespace Application.Interfaces
{
    //Task<Result<IEnumerable<TicketDto>>> GetTicketsByUserIdAsync(string userId, bool useCache = false, CancellationToken cancellationToken = default);
    //Task<Result<IEnumerable<OrderDto>>> GetPaginatedOrdersAsync(PaginationSearchModel paginationParams, bool useCache = false, Func<OrderO, OrderDto>? customMapper = null, CancellationToken cancellationToken = default);
    public interface IOrderService
    {
        ValueTask<BaseResponse<object?>> CreateOrderAsync(CreateOrderDto dto, CancellationToken cancellationToken = default);

        ValueTask<BaseResponse<OrderDto?>> GetOrderByIdAsync(long id, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<PaginatedList<OrderDto>>> GetAllOrdersAsync(PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken = default);

        ValueTask<BaseResponse<object?>> UpdateOrderAsync(UpdateOrderDto dto, CancellationToken cancellationToken = default);

        ValueTask<BaseResponse<object?>> SoftDeleteOrderAsync(long id, CancellationToken cancellationToken = default);

        ValueTask<BaseResponse<object?>> HardDeleteOrderAsync(long id, CancellationToken cancellationToken = default);
    }

}
