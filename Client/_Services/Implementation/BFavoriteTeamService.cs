//using Client.Services.Interfaces;
//using LanguageExt.Common;
//using Shared.Common;
//using Shared.DTOs;
//using Shared.DTOs.Team;


//namespace Client.Services.Implementation
//{
//    public class BFavoriteTeamService : BIFavoriteTeamService
//    {
//        private readonly IHttpClientHelper _httpClientHelper;

//        public BFavoriteTeamService(IHttpClientHelper httpClientHelper)
//        {
//            _httpClientHelper = httpClientHelper;
//        }

//        public async Task<Result<BaseResponse<TeamDto>>> AddFavoriteTeamAsync(string userId, Guid? teamId, Guid matchId, CancellationToken cancellationToken = default)
//        {
//            UserFavoriteTeamDto requestBody = new UserFavoriteTeamDto()
//            {
//                UserId = userId,
//                TeamId = teamId,
//                MatchId = matchId
//            };

//            return await _httpClientHelper.PostBaseAsync<UserFavoriteTeamDto, BaseResponse<TeamDto>>("api/Team/AddFavoriteTeam", requestBody);
//        }

//        public async Task<Result<BaseResponse<TeamDto>>> RemoveFavoriteTeamAsync(string userId, Guid matchId, CancellationToken cancellationToken = default)
//        {
//            UserFavoriteTeamDto requestBody = new UserFavoriteTeamDto()
//            {
//                UserId = userId,
//                MatchId = matchId
//            };
//            return await _httpClientHelper.PostBaseAsync<UserFavoriteTeamDto, BaseResponse<TeamDto>>("api/Team/RemoveFavoriteTeam", requestBody);
//        }

//        public async Task<Result<BaseResponse<TeamDto?>>> GetUserFavoriteTeamAsync(string userId, Guid matchId, bool useCache = false, CancellationToken cancellationToken = default)
//        {
//            var queryParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
//        {
//            { "userId", userId },
//            { "matchId", matchId.ToString() },
//            { "useCache", useCache.ToString() }
//        };

//            var response = await _httpClientHelper.GetBaseAsync<BaseResponse<TeamDto?>>("api/Team/GetUserFavoriteTeam", queryParams);
//            return response;
//        }
//    }
//}
