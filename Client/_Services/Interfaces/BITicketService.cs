//using LanguageExt;
//using LanguageExt.Common;
//using Shared.Common;
//using Shared.DTOs.TicketDTOs;

//namespace Client.Services.Interfaces
//{
//    public interface BITicketService
//    {
//        //Task<ApiResponse> CreateTicketAsync(TicketDto ticket, CancellationToken cancellationToken = default);
//        Task<Result<BaseResponse<Unit>>> CreateTicketsAsync(IEnumerable<TicketDto> tickets, CancellationToken cancellationToken = default);
//        Task<Result<BaseResponse<Unit>>> UpdateTicketsAsync(IEnumerable<TicketDto> tickets, CancellationToken cancellationToken = default);
//        Task<Result<BaseListResponse<IEnumerable<TicketDto>>>> GetTicketsByMatchIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default);
//        Task<Result<BaseListResponse<IEnumerable<TicketDto>>>> GetTicketsByMatchIdAndSelectedTeamAsync(Guid id, Guid? selectedTeamId, bool useCache, CancellationToken cancellationToken = default);

//        Task<Result<BaseListResponse<IEnumerable<TicketDto>>>> HardDeleteTicketByMatch(Guid matchId);
//        Task<Result<BaseResponse<string>>> ScanTicketQrAsync(Guid ticketId, CancellationToken cancellationToken = default);
//    }
//}
