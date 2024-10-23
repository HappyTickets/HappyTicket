using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs.Team;

namespace Client.Services.Interfaces
{
    public interface BIFavoriteTeamService
    {
        Task<Result<BaseResponse<TeamDto>>> AddFavoriteTeamAsync(string userId, Guid? teamId, Guid matchId, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<TeamDto>>> RemoveFavoriteTeamAsync(string userId, Guid matchId, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<TeamDto?>>> GetUserFavoriteTeamAsync(string userId, Guid matchId, bool useCache = false, CancellationToken cancellationToken = default);

    }
}
