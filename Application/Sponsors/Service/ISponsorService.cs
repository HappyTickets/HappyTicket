using Application.Common.Interfaces.Services;
using Domain.Entities;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Sponsors;

namespace Application.Interfaces
{
    public interface ISponsorService : IBaseService<Sponsor>
    {
        Task<BaseResponse<long>> CreateAsync(CreateOrUpdateSponsorDto dto);
        Task<BaseResponse<Empty>> UpdateAsync(long id, CreateOrUpdateSponsorDto dto);
        Task<BaseResponse<Empty>> SoftDeleteAsync(long id);
        Task<BaseResponse<Empty>> HardDeleteAsync(long id);
    
        Task<BaseResponse<IEnumerable<SponsorDto>>> GetAllAsync();
        Task<BaseResponse<SponsorDto>> GetByIdAsync(long id);
    }
}
