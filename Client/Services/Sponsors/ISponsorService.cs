using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Sponsors;

namespace Client.Services.Sponsors
{
    public interface ISponsorService
    {
        Task<BaseResponse<long>> CreateAsync(CreateOrUpdateSponsorDto sponsor);
        Task<BaseResponse<IEnumerable<SponsorDto>>> GetAllAsync();
        Task<BaseResponse<SponsorDto>> GetByIdAsync(long id);
        Task<BaseResponse<Empty>> HardDeleteAsync(long id);
        Task<BaseResponse<Empty>> SoftDeleteAsync(long id);
        Task<BaseResponse<Empty>> UpdateAsync(long id, CreateOrUpdateSponsorDto dto);
    }
}
