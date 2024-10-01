using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.UserEntities;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.ResourceFiles;

namespace Application.Implementations
{
    public class TeamService : BaseService<Team, TeamDto>, ITeamService
    {
        public TeamService(
            IUnitOfWork unitOfWork,
            ILogger<Team> logger,
            IMemoryCache cache,
            IMapper mapper,
            IValidator<TeamDto> validator,
            IStringLocalizer<Resource> localizer) :
            base(unitOfWork, logger, cache, mapper, validator, localizer)
        { }

        public async Task<Result<TeamDto?>> GetUserFavTeam(string userId, Guid matchId, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityResult = await _unitOfWork.Repository<UserFavoriteTeam>()
                    .FindAsync(x => x.UserId == userId && x.MatchId == matchId, cancellationToken, x => x.Include(y => y.Team));

                var favoriteTeam = entityResult.Match(
                    succ => succ.FirstOrDefault()?.Team,
                    fail => null);

                if (favoriteTeam == null)
                {
                    return new Result<TeamDto?>(new NotFoundException(new List<ErrorInfo>
                    {
                        new() { Title = Resource.NotFound, Message = Resource.FavoriteTeam_NotFound_Message.ToString().Replace("{user}", userId).Replace("{match}", matchId.ToString()) }
                    }));
                }

                var favoriteTeamDto = _mapper.Map<TeamDto>(favoriteTeam);
                return new(favoriteTeamDto);
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<Result<TeamDto>> AddFavTeam(UserFavoriteTeamDto userFavoriteTeamDto, CancellationToken cancellationToken = default)
        {
            try
            {
                UserFavoriteTeam userFavoriteTeam = _mapper.Map<UserFavoriteTeam>(userFavoriteTeamDto);
                var entityResult = await _unitOfWork.Repository<UserFavoriteTeam>().CreateAsync(userFavoriteTeam, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var teamResult = await _unitOfWork.Repository<Team>().GetByIdAsync(entityResult.Match(Succ => Succ.TeamId, Fail => Guid.Empty), cancellationToken);
                var teamDto = _mapper.Map<TeamDto>(teamResult);

                return new(teamDto);
            }
            catch (Exception ex)
            {
                return new Result<TeamDto>(ex);
            }
        }

        public async Task<Result<Unit>> RemoveFavTeam(UserFavoriteTeamDto userFavoriteTeamDto, CancellationToken cancellationToken = default)
        {
            try
            {
                UserFavoriteTeam userFavoriteTeam = _mapper.Map<UserFavoriteTeam>(userFavoriteTeamDto);

                var entityResult = await _unitOfWork.Repository<UserFavoriteTeam>()
                    .FindAsync(x => x.UserId == userFavoriteTeam.UserId && x.MatchId == userFavoriteTeam.MatchId, cancellationToken);

                return await entityResult.Match(
                    async succ =>
                    {
                        var favoriteTeam = succ.FirstOrDefault();

                        if (favoriteTeam == null)
                        {
                            return new Result<Unit>(new NotFoundException(new List<ErrorInfo>
                            {
                                new() { Title = Resource.NotFound, Message = Resource.FavoriteTeam_NotFound_Message.ToString().Replace("{user}", userFavoriteTeam.UserId).Replace("{match}", userFavoriteTeam.MatchId.ToString()) }
                            }));
                        }

                        return await _unitOfWork.Repository<UserFavoriteTeam>().HardDelete(favoriteTeam).MapAsync(
                            async updateSucc =>
                            {
                                return await _unitOfWork.SaveChangesAsync(cancellationToken).Map(succ => new Unit());
                            });
                    },
                    async fail => await fail.ToResultAsync<Unit>());
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<Result<TeamDto>> DeleteTeamWithNoMatchesAsync(Guid id, CancellationToken cancellationToken = default) 
        {
            var matchesRepo = _unitOfWork.Repository<Match>();
            var teamsRepo = _unitOfWork.Repository<Team>();

            if (await matchesRepo.Query().AnyAsync(m => m.TeamAId == id || m.TeamBId == id))
                return new(new Exception(Resource.Team_With_Match_Deletion_Failure));

            var result = await teamsRepo.HardDeleteByIdAsync(id, cancellationToken);
            return await result.Match(
                async team => {
                    await _unitOfWork.SaveChangesAsync();
                    return _mapper.Map<TeamDto>(team).ToResult();
                },
                async ex => await ex.ToResultAsync<TeamDto>());
        }
    }

}
