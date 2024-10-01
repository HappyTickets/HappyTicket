using LanguageExt.Common;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.MatchDtos;

namespace Client.Services.Interfaces
{
    public interface BIMatchService
    {
        Task<Result<BaseListResponse<IEnumerable<MatchDto>>>> GetMatchesAsync(bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseListResponse<IEnumerable<MatchDto>>>> GetActiveMatches(bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<MatchDto>>> GetMatchByIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<MatchCommandDto>>> CreateMatchAsync(MatchCommandDto match, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<MatchDto>>> UpdateMatchAsync(MatchDto match, CancellationToken cancellationToken = default);
        Task<bool> DeleteMatchAsync(Guid Id,CancellationToken cancellationToken = default);
        //Task<ApiResponse> RecoverMatchByIdAsync(Guid id, CancellationToken cancellationToken = default);
        //Task<ApiResponse> SoftDeleteMatchByIdAsync(Guid id, CancellationToken cancellationToken = default);
        //Task<ApiResponse> HardDeleteMatchByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
