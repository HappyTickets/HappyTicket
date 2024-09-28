using LanguageExt.Common;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs;
using Shared.DTOs.TicketDTOs;

namespace Client.Services.Interfaces
{
    public interface IBOrderService
    {
        Task<Result<BaseListResponse<IEnumerable<OrderDto>>>> GetPaginatedOrdersAsync(PaginationParams paginationParams, bool useCache, CancellationToken cancellationToken = default);

        Task<Result<BaseResponse<long>>> GetOrdersCountAsync(CancellationToken cancellationToken = default);
        Task<Result<BaseListResponse<IEnumerable<TicketDto>>>> GetMyOrdersAsync(CancellationToken cancellationToken = default);
    }
}
