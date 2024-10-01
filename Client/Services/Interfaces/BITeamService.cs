using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs;

namespace Client.Services.Interfaces
{
    public interface BITeamService
    {
        Task<Result<BaseListResponse<IEnumerable<TeamDto>>>> GetTeamsAsync(bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<TeamDto>>> GetTeamByIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<TeamDto>>> CreateTeamAsync(TeamDto team, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<TeamDto>>> UpdateTeamAsync(TeamDto team, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<TeamDto>>> RecoverTeamByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<TeamDto>>> SoftDeleteTeamByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<TeamDto>>> HardDeleteTeamByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<TeamDto>>> DeleteTeamWithNoMatchesAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
