//using Client.Services.Interfaces;
//using LanguageExt;
//using LanguageExt.Common;
//using Shared.Common;
//using Shared.DTOs.TicketDTOs;

//namespace Client.Services.Implementation
//{
//    public class BTicketService : BITicketService
//    {
//        private readonly IHttpClientHelper _httpClientHelper;

//        public BTicketService(IHttpClientHelper httpClientHelper)
//        {
//            _httpClientHelper = httpClientHelper;
//        }

//        public async Task<Result<BaseResponse<Unit>>> CreateTicketsAsync(IEnumerable<TicketDto> tickets, CancellationToken cancellationToken = default)
//        {
//            var response = await _httpClientHelper.PostBaseAsync<IEnumerable<TicketDto>, BaseResponse<Unit>>("api/ticket/AddTickets", tickets);
//            return response;
//        }

//        public async Task<Result<BaseResponse<Unit>>> UpdateTicketsAsync(IEnumerable<TicketDto> tickets, CancellationToken cancellationToken = default)
//        {
//            var response = await _httpClientHelper.PostBaseAsync<IEnumerable<TicketDto>, BaseResponse<Unit>>("/api/tickets/updateTickets", tickets);
//            return response;
//        }

//        public async Task<Result<BaseListResponse<IEnumerable<TicketDto>>>> GetTicketsByMatchIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default)
//        {
//            var response = await _httpClientHelper.GetBaseAsync<BaseListResponse<IEnumerable<TicketDto>>>($"api/ticket/GetTicketsByMatchId?id={id}&useCache={useCache}");
//            return response;
//        }

//        public async Task<Result<BaseListResponse<IEnumerable<TicketDto>>>> GetTicketsByMatchIdAndSelectedTeamAsync(Guid id, Guid? selectedTeamId, bool useCache, CancellationToken cancellationToken = default)
//        {
//            var response = await _httpClientHelper.GetBaseAsync<BaseListResponse<IEnumerable<TicketDto>>>($"api/ticket/GetTicketsByMatchIdAndSelectedTeam?id={id}&selectedTeamId={selectedTeamId}&useCache={useCache}");
//            return response;
//        }

//        public async Task<Result<BaseListResponse<IEnumerable<TicketDto>>>> HardDeleteTicketByMatch(Guid matchId)
//        {
//            var response = await _httpClientHelper.GetBaseAsync<BaseListResponse<IEnumerable<TicketDto>>>($"api/ticket/HardDeleteTicketByMatch?matchId={matchId}");
//            return response;
//        }

//        // Add ScanTicketQrAsync method to call the ScanTicketQr API
//        public async Task<Result<BaseResponse<string>>> ScanTicketQrAsync(Guid ticketId, CancellationToken cancellationToken = default)
//        {
//            var response = await _httpClientHelper.PostBaseAsync<Guid, BaseResponse<string>>("api/ticket/ScanTicketQr", ticketId);
//            return response;
//        }
//    }
//}
