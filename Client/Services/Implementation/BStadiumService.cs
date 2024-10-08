using Client.Services.Interfaces;
using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs;

namespace Client.Services.Implementation
{
    public class BStadiumService : BIStadiumService
    {
        private readonly IHttpClientHelper _httpClientHelper;

        public BStadiumService(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }

        public async Task<Result<BaseListResponse<IEnumerable<StadiumDto>>>> GetStadiumsAsync(bool useCache, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "useCache", useCache.ToString() },
            };

            var response = await _httpClientHelper.GetBaseAsync<BaseListResponse<IEnumerable<StadiumDto>>>("api/Stadium/GetStadiums", queryParams);
            return response;
        }

        public async Task<Result<BaseResponse<StadiumDto>>> GetStadiumByIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.GetBaseAsync<BaseResponse<StadiumDto>>($"api/Stadium/GetStadium?id={id}&useCache={useCache}");
        }

        public async Task<Result<BaseResponse<StadiumDto>>> CreateStadiumAsync(StadiumDto stadium, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.PostBaseAsync<StadiumDto, BaseResponse<StadiumDto>>("api/Stadium/AddStadium", stadium);
        }

        public async Task<Result<BaseResponse<StadiumDto>>> UpdateStadiumAsync(StadiumDto stadium, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.PutBaseAsync<StadiumDto, BaseResponse<StadiumDto>>("api/Stadium/EditStadium", stadium);
        }

        public async Task<Result<BaseResponse<StadiumDto>>> RecoverStadiumByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.GetBaseAsync<BaseResponse<StadiumDto>>($"api/Stadium/RecoverStadium?id={id}");
        }

        public async Task<Result<BaseResponse<StadiumDto>>> SoftDeleteStadiumByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.GetBaseAsync<BaseResponse<StadiumDto>>($"api/Stadium/SoftDeleteStadium?id={id}");
        }

        public async Task<Result<BaseResponse<StadiumDto>>> HardDeleteStadiumByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.GetBaseAsync<BaseResponse<StadiumDto>>($"api/Stadium/HardDeleteStadium?id={id}");
        }
        
        public async Task<Result<BaseResponse<StadiumDto>>> DeleteStadiumWithNoMatchesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.GetBaseAsync<BaseResponse<StadiumDto>>($"api/Stadium/DeleteStadiumWithNoMatches?id={id}");
        }
    }
}
