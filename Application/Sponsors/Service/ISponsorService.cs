using Application.Common.Interfaces.Services;
using Domain.Entities;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Sponsors;

namespace Application.Interfaces
{
    public interface ISponsorService : IBaseService<Sponsor>
    {
        Task<BaseResponse<long>> CreateAsync(CreateOrUpdateSponsorDto dto, CancellationToken cancellationToken = default);
        Task<BaseResponse<Empty>> UpdateAsync(long id, CreateOrUpdateSponsorDto dto, CancellationToken cancellationToken = default);
        Task<BaseResponse<Empty>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<BaseResponse<Empty>> HardDeleteAsync(long id, CancellationToken cancellationToken = default);
    
        Task<BaseResponse<IEnumerable<SponsorDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<BaseResponse<SponsorDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    }
}
