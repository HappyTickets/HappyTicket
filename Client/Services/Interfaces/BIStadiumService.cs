using LanguageExt.Common;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs;

namespace Client.Services.Interfaces
{
    public interface BIStadiumService
    {
        Task<Result<BaseListResponse<IEnumerable<StadiumDto>>>> GetStadiumsAsync(bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<StadiumDto>>> GetStadiumByIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<StadiumDto>>> CreateStadiumAsync(StadiumDto stadium, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<StadiumDto>>> UpdateStadiumAsync(StadiumDto stadium, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<StadiumDto>>> RecoverStadiumByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<StadiumDto>>> SoftDeleteStadiumByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<StadiumDto>>> HardDeleteStadiumByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
