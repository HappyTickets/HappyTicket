using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.TicketDTOs;

namespace Application.Tickets.Service
{
    public interface ITicketService
    {
        Task<BaseResponse<Empty>> CreateAsync(CreateTicketsDto dto);
        Task<BaseResponse<Empty>> UpdateAsync(UpdateTicketsDto dto);
        Task<BaseResponse<IEnumerable<TicketDto>>> GetDistinctTicketsAsync(long matchId);
        Task<BaseResponse<PaginatedList<TicketDto>>> GetMyTicketsAsync(long userId, PaginationParams pagination);

        //Task<Result<string>> ScanQrCodeAsync(Guid ticketId, CancellationToken cancellationToken = default);
    }
}