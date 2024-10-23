//using Client.Services.Interfaces;
//using LanguageExt.Common;
//using Shared.Common;
//using Shared.DTOs.Champion;

//namespace Client.Services.Implementation
//{
//    public class ChampionService: IChampionService
//    {
//        private readonly IHttpClientHelper _httpClientHelper;

//        public ChampionService(IHttpClientHelper httpClientHelper)
//        {
//            _httpClientHelper = httpClientHelper;
//        }

//        public async Task<Result<BaseResponse<IEnumerable<ChampionDto>>>> GetChampionsAsync(bool useCache, CancellationToken cancellationToken = default)
//        {
//            return await _httpClientHelper.GetBaseAsync<BaseResponse<IEnumerable<ChampionDto>>>($"api/champion?useCache={useCache}");
//        }

//        public async Task<Result<BaseResponse<ChampionDto>>> GetChampionByIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default)
//        {
//            return await _httpClientHelper.GetBaseAsync<BaseResponse<ChampionDto>>($"api/champion/{id}?useCache={useCache}");
//        }

//        public async Task<Result<BaseResponse<ChampionDto>>> CreateChampionAsync(CreateOrUpdateChampionDto champion, CancellationToken cancellationToken = default)
//        {
//            return await _httpClientHelper.PostBaseAsync<CreateOrUpdateChampionDto, BaseResponse<ChampionDto>>("api/champion", champion);
//        }

//        public async Task<Result<BaseResponse<ChampionDto>>> UpdateChampionAsync(Guid id, CreateOrUpdateChampionDto champion, CancellationToken cancellationToken = default)
//        {
//            return await _httpClientHelper.PutBaseAsync<CreateOrUpdateChampionDto, BaseResponse<ChampionDto>>($"api/champion/{id}", champion);
//        }

//        public async Task<Result<BaseResponse<ChampionDto>>> DeleteChampionAsync(Guid id, CancellationToken cancellationToken = default)
//        {
//            return await _httpClientHelper.DeleteBaseAsync<BaseResponse<ChampionDto>>($"api/champion/{id}");
//        }

//    }
//}
