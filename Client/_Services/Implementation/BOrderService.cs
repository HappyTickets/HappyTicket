//using Client.Services.Interfaces;
//using LanguageExt.Common;
//using Shared.Common;
//using Shared.Common.General;
//using Shared.DTOs.TicketDTOs;

//namespace Client.Services.Implementation
//{
//    public class BOrderService : IBOrderService
//    {
//        private readonly IHttpClientHelper _httpClientHelper;

//        public BOrderService(IHttpClientHelper httpClientHelper)
//        {
//            _httpClientHelper = httpClientHelper;
//        }

//        public async Task<Result<BaseListResponse<IEnumerable<OrderDto>>>> GetPaginatedOrdersAsync(PaginationSearchModel paginationParams, bool useCache, CancellationToken cancellationToken = default)
//        {
//            var response = await _httpClientHelper.PostBaseAsync<PaginationSearchModel, BaseListResponse<IEnumerable<OrderDto>>>($"api/Order/GetPaginatedOrders?useCache={useCache}&cancellationToken={cancellationToken}", paginationParams);
//            return response;
//        }
//        public async Task<Result<BaseListResponse<IEnumerable<TicketDto>>>> GetMyOrdersAsync(CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                // Call the API to get the response as Result<BaseResponse<IEnumerable<OrderDto>>>
//                var responseResult = await _httpClientHelper.GetBaseAsync<BaseListResponse<IEnumerable<TicketDto>>>(
//                    "api/Order/GetMyOrders",
//                    queryParams: null,
//                    useAuth: true
//                );

//                // If the Result is faulted, return the faulted result (handle the exception using Match)
//                if (responseResult.IsFaulted)
//                {
//                    // Use IfFail or Match to return the exception if faulted
//                    return responseResult.Match(
//                        succ => succ, // Success case (not needed since we're handling the faulted case)
//                        fail => new Result<BaseListResponse<IEnumerable<TicketDto>>>(fail) // Handle exception
//                    );
//                }

//                // Extract the BaseListResponse from the Result
//                var response = responseResult.Match(
//                    succ => succ,
//                    fail => throw fail // Handle exception case
//                );

//                // Check if the API call succeeded and the data is not null
//                if (response.IsSuccess && response.Data != null)
//                {
//                    return new Result<BaseListResponse<IEnumerable<TicketDto>>>(response);
//                }
//                else
//                {
//                    // Handle errors in the BaseResponse and return an error
//                    var errorMessage = response.ErrorList?.FirstOrDefault()?.Message ?? "Unknown error occurred.";
//                    return new Result<BaseListResponse<IEnumerable<TicketDto>>>(new Exception(errorMessage));
//                }
//            }
//            catch (Exception ex)
//            {
//                // Return the exception if something went wrong during the API call
//                return new Result<BaseListResponse<IEnumerable<TicketDto>>>(ex);
//            }
//        }


//        public async Task<Result<BaseResponse<long>>> GetOrdersCountAsync(CancellationToken cancellationToken = default)
//        {
//            var response = await _httpClientHelper.GetBaseAsync<BaseResponse<long>>($"api/Order/GetOrdersCount?cancellationToken={cancellationToken}");
//            return response;
//        }
//    }
//}
