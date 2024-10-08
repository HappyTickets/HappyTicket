using Domain.Entities;
using LanguageExt;
using LanguageExt.Common;
using Shared.DTOs;
using Shared.DTOs.Team;

namespace Application.Interfaces
{
    public interface ITeamService : IBaseService<Team, TeamDto>
    {
        Task<Result<TeamDto>> AddFavTeam(UserFavoriteTeamDto userFavoriteTeamDto, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveFavTeam(UserFavoriteTeamDto userFavoriteTeamDto, CancellationToken cancellationToken = default);
        Task<Result<TeamDto>> GetUserFavTeam(string userId, Guid matchId, bool useCache = true, CancellationToken cancellationToken = default);
        Task<Result<TeamDto>> DeleteTeamWithNoMatchesAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<TeamDto>> CreateAsync(CreateOrUpdateTeamDto dto, CancellationToken cancellationToken = default);
        Task<Result<TeamDto>> UpdateAsync(Guid id, CreateOrUpdateTeamDto dto, CancellationToken cancellationToken = default);
        Task<Result<TeamDto>> GetByIdAsync(Guid id, bool useCache = true, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<TeamDto>>> GetAllAsync(bool useCache = true, CancellationToken cancellationToken = default);
    }
}
