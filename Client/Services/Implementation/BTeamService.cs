using Client.Services.Interfaces;
using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs;


namespace Client.Services.Implementation
{
    public class BTeamService : BITeamService
    {
        private readonly IHttpClientHelper _httpClientHelper;

        public BTeamService(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }

        public async Task<Result<BaseListResponse<IEnumerable<TeamDto>>>> GetTeamsAsync(bool useCache, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "useCache", useCache.ToString() },
            };

            var response = await _httpClientHelper.GetBaseAsync<BaseListResponse<IEnumerable<TeamDto>>>("api/Team/GetTeams", queryParams);
            return response;
        }

        public async Task<Result<BaseResponse<TeamDto>>> GetTeamByIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.GetBaseAsync<BaseResponse<TeamDto>>($"api/Team/GetTeam?id={id}&useCache={useCache}");
        }

        public async Task<Result<BaseResponse<TeamDto>>> CreateTeamAsync(TeamDto team, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.PostBaseAsync<TeamDto, BaseResponse<TeamDto>>("api/Team/AddTeam", team);
        }

        public async Task<Result<BaseResponse<TeamDto>>> UpdateTeamAsync(TeamDto team, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.PutBaseAsync<TeamDto, BaseResponse<TeamDto>>("api/Team/EditTeam", team);
        }

        public async Task<Result<BaseResponse<TeamDto>>> RecoverTeamByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.GetBaseAsync<BaseResponse<TeamDto>>($"api/Team/RecoverTeam?id={id}");
        }

        public async Task<Result<BaseResponse<TeamDto>>> SoftDeleteTeamByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.GetBaseAsync<BaseResponse<TeamDto>>($"api/Team/SoftDeleteTeam?id={id}");
        }

        public async Task<Result<BaseResponse<TeamDto>>> HardDeleteTeamByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.GetBaseAsync<BaseResponse<TeamDto>>($"api/Team/HardDeleteTeam?id={id}");
        }

    }
}
