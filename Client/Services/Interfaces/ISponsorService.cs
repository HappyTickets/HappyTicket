using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs;

namespace Client.Services.Interfaces
{
    public interface ISponsorService
    {
        Task<Result<BaseResponse<SponsorDto>>> CreateSponsorAsync(SponsorDto sponsor, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<SponsorDto>>> DeleteSponsorAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<SponsorDto>>> GetSponsorByIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<IEnumerable<SponsorDto>>>> GetSponsorsAsync(bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<SponsorDto>>> UpdateSponsorAsync(SponsorDto sponsor, CancellationToken cancellationToken = default);
    }
}
