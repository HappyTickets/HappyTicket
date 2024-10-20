using LanguageExt.Common;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.OrderDtos.Response;
using Shared.DTOs.TicketDTOs;

namespace Client.Services.Interfaces
{
    public interface IBOrderService
    {
        Task<Result<BaseListResponse<IEnumerable<OrderDto>>>> GetPaginatedOrdersAsync(PaginationSearchModel paginationParams, bool useCache, CancellationToken cancellationToken = default);

        Task<Result<BaseResponse<long>>> GetOrdersCountAsync(CancellationToken cancellationToken = default);
        Task<Result<BaseListResponse<IEnumerable<TicketDto>>>> GetMyOrdersAsync(CancellationToken cancellationToken = default);
    }
}
