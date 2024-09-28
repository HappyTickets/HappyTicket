using Domain.Entities;
using Domain.Entities.UserEntities;
using LanguageExt;
using LanguageExt.Common;
using Shared.DTOs;

namespace Application.Interfaces
{
    public interface ITeamService : IBaseService<Team, TeamDto>
    {
        Task<Result<TeamDto>> AddFavTeam(UserFavoriteTeamDto userFavoriteTeamDto, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveFavTeam(UserFavoriteTeamDto userFavoriteTeamDto, CancellationToken cancellationToken = default);
        Task<Result<TeamDto>> GetUserFavTeam(string userId,Guid matchId, bool useCache = true, CancellationToken cancellationToken = default);
    }
}
