using Client.Services.Interfaces;
using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs.Sponsors;

namespace Client.Services.Implementation
{
    public class SponsorService : ISponsorService
    {
        private readonly IHttpClientHelper _httpClientHelper;

        public SponsorService(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }

        public async Task<Result<BaseResponse<IEnumerable<SponsorDto>>>> GetSponsorsAsync(bool useCache, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.GetBaseAsync<BaseResponse<IEnumerable<SponsorDto>>>($"api/sponsor?useCache={useCache}");
        }

        public async Task<Result<BaseResponse<SponsorDto>>> GetSponsorByIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.GetBaseAsync<BaseResponse<SponsorDto>>($"api/sponsor/{id}?useCache={useCache}");
        }

        public async Task<Result<BaseResponse<SponsorDto>>> CreateSponsorAsync(SponsorDto sponsor, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.PostBaseAsync<SponsorDto, BaseResponse<SponsorDto>>("api/sponsor", sponsor);
        }

        public async Task<Result<BaseResponse<SponsorDto>>> UpdateSponsorAsync(SponsorDto sponsor, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.PutBaseAsync<SponsorDto, BaseResponse<SponsorDto>>("api/sponsor", sponsor);
        }

        public async Task<Result<BaseResponse<SponsorDto>>> DeleteSponsorAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.DeleteBaseAsync<BaseResponse<SponsorDto>>($"api/sponsor/{id}");
        }
    }
}
