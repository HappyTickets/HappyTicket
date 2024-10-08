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
using Shared.DTOs.Team;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.ResourceFiles;

namespace Application.Implementations
{
    public class TeamService : BaseService<Team, TeamDto>, ITeamService
    {
        private readonly IValidator<CreateOrUpdateTeamDto> _createOrUpdateValidator;

        public TeamService(
            IUnitOfWork unitOfWork,
            ILogger<Team> logger,
            IMemoryCache cache,
            IMapper mapper,
            IValidator<TeamDto> validator,
            IStringLocalizer<Resource> localizer,
            IValidator<CreateOrUpdateTeamDto> createOrUpdateValidator) :
            base(unitOfWork, logger, cache, mapper, validator, localizer)
        {
            _createOrUpdateValidator = createOrUpdateValidator;
        }

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
                async team =>
                {
                    await _unitOfWork.SaveChangesAsync();
                    return _mapper.Map<TeamDto>(team).ToResult();
                },
                async ex => await ex.ToResultAsync<TeamDto>());
        }

        public async Task<Result<TeamDto>> CreateAsync(CreateOrUpdateTeamDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = _createOrUpdateValidator.Validate(dto);
                if (!validationResult.IsValid)
                    return new(new ValidationException(validationResult.Errors));

                var teamRepo = _unitOfWork.Repository<Team>();

                var team = _mapper.Map<Team>(dto);
                if (dto.SponsorsIds != null)
                {
                    team.TeamSponsors = dto.SponsorsIds.Select(id => new TeamSponsor
                    {
                        SponsorId = id
                    }).ToArray();
                }

                var result = await teamRepo.CreateAsync(team);
                return await result.Match(
                    async team =>
                    {
                        await _unitOfWork.SaveChangesAsync();
                        return _mapper.Map<TeamDto>(team).ToResult();
                    },
                    async ex => await ex.ToResultAsync<TeamDto>());
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<Result<TeamDto>> UpdateAsync(Guid id, CreateOrUpdateTeamDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = _createOrUpdateValidator.Validate(dto);
                if (!validationResult.IsValid)
                    return new(new ValidationException(validationResult.Errors));

                var teamRepo = _unitOfWork.Repository<Team>();
                var teamSponsorRepo = _unitOfWork.Repository<TeamSponsor>();
                var teamResult = await teamRepo.GetByIdAsync(id, cancellationToken);
                return await teamResult.Match(
                    async team =>
                    {
                        _mapper.Map(dto, team);
                        var result = teamRepo.Update(team);
                        return await result.Match(
                            async team =>
                            {
                                teamSponsorRepo.HardDeleteRange(cs => cs.TeamId == id);
                                if (dto.SponsorsIds != null)
                                {
                                    var teamSponsors = dto.SponsorsIds.Select(i => new TeamSponsor
                                    {
                                        TeamId = id,
                                        SponsorId = i
                                    });
                                    await teamSponsorRepo.CreateRangeAsync(teamSponsors);
                                }
                                await _unitOfWork.SaveChangesAsync();
                                return _mapper.Map<TeamDto>(team).ToResult();
                            },
                            async ex => await ex.ToResultAsync<TeamDto>());
                    },
                    async ex => await ex.ToResultAsync<TeamDto>()
                    );
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }


        public async Task<Result<TeamDto>> GetByIdAsync(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.GetByIdAsync(id: id, useCache: useCache, cancellationToken: cancellationToken, includeDTOProperties: q => q.Include(c => c.TeamSponsors).ThenInclude(cs => cs.Sponsor));
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<Result<IEnumerable<TeamDto>>> GetAllAsync(bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.GetAllAsync(useCache: useCache, cancellationToken: cancellationToken, includeDTOProperties: q => q.Include(c => c.TeamSponsors).ThenInclude(cs => cs.Sponsor));
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }
    }

}
