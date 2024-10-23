using Client.Services._HttpClientFacade;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Sponsors;

namespace Client.Services.Sponsors
{
    public class SponsorService : ISponsorService
    {
        private readonly IHttpClientFacade _httpClientFacade;

        public SponsorService(IHttpClientFacade httpClientFacade)
        {
            _httpClientFacade = httpClientFacade;
        }

        public async Task<BaseResponse<IEnumerable<SponsorDto>>> GetAllAsync()
            => await _httpClientFacade.GetAsync<BaseResponse<IEnumerable<SponsorDto>>>($"api/sponsors/get-all");
        
        public async Task<BaseResponse<SponsorDto>> GetByIdAsync(long id)
            => await _httpClientFacade.GetAsync<BaseResponse<SponsorDto>>($"api/sponsors/get-by-id?id={id}");
        
        public async Task<BaseResponse<long>> CreateAsync(CreateOrUpdateSponsorDto sponsor)
            => await _httpClientFacade.PostAsync<BaseResponse<long>>("api/sponsors/create", sponsor);

        public async Task<BaseResponse<Empty>> UpdateAsync(long id, CreateOrUpdateSponsorDto dto)
            => await _httpClientFacade.PutAsync<BaseResponse<Empty>>($"api/sponsors/update?id={id}", dto);

        public async Task<BaseResponse<Empty>> HardDeleteAsync(long id)
            => await _httpClientFacade.DeleteAsync<BaseResponse<Empty>>($"api/sponsors/hard-delete?id={id}");
        
        public async Task<BaseResponse<Empty>> SoftDeleteAsync(long id)
         => await _httpClientFacade.DeleteAsync<BaseResponse<Empty>>($"api/sponsors/soft-delete?id={id}");
        
    }
}
