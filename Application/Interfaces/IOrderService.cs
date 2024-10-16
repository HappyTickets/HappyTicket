//using Domain.Entities;
//using LanguageExt.Common;
//using Shared.Common.General;
//using Shared.DTOs;
//using Shared.DTOs.TicketDTOs;

//namespace Application.Interfaces
//{
//    public interface IOrderService : IBaseService<OrderO, OrderDto>
//    {
//        Task<Result<IEnumerable<TicketDto>>> GetTicketsByUserIdAsync(string userId, bool useCache = false, CancellationToken cancellationToken = default);
//        Task<Result<IEnumerable<OrderDto>>> GetPaginatedOrdersAsync(PaginationSearchModel paginationParams, bool useCache = false, Func<OrderO, OrderDto>? customMapper = null, CancellationToken cancellationToken = default);

//    }
//}
