//using Client.Services.Interfaces;
//using LanguageExt.Common;
//using Shared.Common;
//using Shared.DTOs.MatchDtos;

//namespace Client.Services.Implementation
//{
//    public class BMatchService : BIMatchService
//    {
//        private readonly IHttpClientHelper _httpClientHelper;

//        public BMatchService(IHttpClientHelper httpClientHelper)
//        {
//            _httpClientHelper = httpClientHelper;
//        }

//        public async Task<Result<BaseListResponse<IEnumerable<MatchDto>>>> GetMatchesAsync(bool useCache, CancellationToken cancellationToken = default)
//        {
//            var queryParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
//            {
//                { "useCache", useCache.ToString() },
//            };

//            var response = await _httpClientHelper.GetBaseAsync<BaseListResponse<IEnumerable<MatchDto>>>("api/Match/GetMatches", queryParams);
//            return response;
//        }

//        public async Task<Result<BaseListResponse<IEnumerable<MatchDto>>>> GetActiveMatches(bool useCache, CancellationToken cancellationToken = default)
//        {
//            return await _httpClientHelper.GetBaseAsync<BaseListResponse<IEnumerable<MatchDto>>>($"api/Match/GetActiveMatches?useCache={useCache}");
//        }

//        public async Task<Result<BaseResponse<MatchDto>>> GetMatchByIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default)
//        {
//            return await _httpClientHelper.GetBaseAsync<BaseResponse<MatchDto>>($"api/Match/GetMatch?id={id}&useCache={useCache}");
//        }

//        public async Task<Result<BaseResponse<MatchCommandDto>>> CreateMatchAsync(MatchCommandDto match, CancellationToken cancellationToken = default)
//        {
//            return await _httpClientHelper.PostBaseAsync<MatchCommandDto, BaseResponse<MatchCommandDto>>("api/Match/AddMatch", match);
//        }

//        public async Task<Result<BaseResponse<MatchDto>>> UpdateMatchAsync(MatchDto match, CancellationToken cancellationToken = default)
//        {
//            return await _httpClientHelper.PutBaseAsync<MatchDto, BaseResponse<MatchDto>>("api/Match/UpdateMatch", match);
//        }

//        //public async Task<ApiResponse> SoftDeleteMatchByIdAsync(Guid id, CancellationToken cancellationToken = default)
//        //{
//        //    return await _httpClientHelper.DeleteAsync($"api/Match/SoftDeleteMatch?id={id}");
//        //}

//        //public async Task<ApiResponse> HardDeleteMatchByIdAsync(Guid id, CancellationToken cancellationToken = default)
//        //{
//        //    return await _httpClientHelper.DeleteAsync($"api/Match/HardDeleteMatch?id={id}");
//        //}
//    }
//}
