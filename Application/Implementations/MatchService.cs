using Application.Common.Implementations;
using Application.Common.Interfaces.Persistence;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using LanguageExt;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.MatchDtos;
using Shared.DTOs.Team;
using System.Linq.Expressions;
using System.Net;

namespace Application.Implementations
{
    public class MatchService : BaseService<Match>, IMatchService
    {
        public MatchService(IUnitOfWork unitOfWork, ILogger<Match> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
        {
        }

        public async ValueTask<BaseResponse<IEnumerable<GetAllMatchesDto>>> GetAll()
        {
            var includes = new List<string>
                {
                    nameof(Match.Stadium),
                    nameof(Match.Champion),
                    $"{nameof(Match.MatchTeams)}.{nameof(MatchTeam.Team)}"

                };

            var result = await GetAllAsync<GetAllMatchesDto>(includes: includes);


            return result.ToList();
        }
        public async ValueTask<BaseResponse<GetMatchByIdDto>> GetMatchByIdAsync(long id)
        {
            var includes = new List<string>
                {
                    nameof(Match.Stadium),
                    nameof(Match.Champion),
                    $"{nameof(Match.MatchTeams)}.{nameof(MatchTeam.Team)}"

                };

            var match = await GetByIdAsync<GetMatchByIdDto>(id, includes: includes);
            return new BaseResponse<GetMatchByIdDto>(match);

        }

        public async ValueTask<BaseResponse<IEnumerable<FindActiveMatchesDto>>> FindActiveMatches()
        {
            var includes = new List<string>
                {
                    nameof(Match.Stadium),
                    nameof(Match.Champion),
                    $"{nameof(Match.MatchTeams)}.{nameof(MatchTeam.Team)}"

                };
            // Get the current date and time (local or UTC based on your needs)
            var currentDateTime = DateTime.Now; // Use DateTime.UtcNow if your app works with UTC

            // Fetch matches that are not finished and have valid EventDate and EventTime
            var matches = await FindAsync<FindActiveMatchesDto>(
                x => x.EventDate != null && x.EventTime != null);

            // Perform the time calculation on the client side
            var activeMatches = matches
                .AsEnumerable()  // This moves the computation to the client-side
                .Where(x =>
                {
                    var matchStartTime = x.EventDate.Value.Add(x.EventTime.Value);
                    return matchStartTime <= currentDateTime; // Match has started or is about to start
                });

            return new BaseResponse<IEnumerable<FindActiveMatchesDto>>(activeMatches);

        }
        public async ValueTask<BaseResponse<PaginatedList<GetPaginatedMatchesDto>>> GetPaginatedAsync(PaginationSearchModel paginationParams)
        {
            var paginatedMatches = await GetPaginatedAsync<GetPaginatedMatchesDto>(null, paginationParams);
            return new BaseResponse<PaginatedList<GetPaginatedMatchesDto>>(paginatedMatches);
        }
        public async ValueTask<BaseResponse<long>> GetCountAsync(CancellationToken cancellationToken = default)
        {
            var count = await GetLongCountAsync(cancellationToken);
            return new(count);
        }
        public async ValueTask<BaseResponse<Unit>> CreateMatchAsync(CreateMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var match = _mapper.Map<Match>(dto);

            var teamA = new MatchTeam
            {
                TeamId = dto.TeamAId,
                IsHomeTeam = true,
                MatchId = match.Id
            };

            var teamB = new MatchTeam
            {
                TeamId = dto.TeamBId,
                IsHomeTeam = false,
                MatchId = match.Id
            };

            match.MatchTeams.Add(teamA);
            match.MatchTeams.Add(teamB);

            _unitOfWork.Repository<Match>().Create(match);

            if (autoSave)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }


            await CreateAsync(dto, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }
        public async ValueTask<BaseResponse<Unit>> UpdateMatchAsync(UpdateMatchDto dto, long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            // Retrieve the match by its ID
            var match = await GetByIdAsync<Match>(id);

            // Check if the match exists
            if (match == null)
            {
                return new BaseResponse<Unit>
                {
                    Status = HttpStatusCode.NotFound,
                    Title = "Match Not Found",
                    ErrorList = new List<ResponseError>
            {
                new ResponseError("Match Not Found", "The requested match could not be found.")
            }
                };
            }

            // Update or add Team A (Home Team)
            var teamA = match.MatchTeams.FirstOrDefault(mt => mt.IsHomeTeam == true);
            if (teamA != null)
            {
                // Update existing Team A
                teamA.TeamId = dto.TeamAId;
            }
            else
            {
                // Add new Team A if it doesn't exist
                teamA = new MatchTeam
                {
                    TeamId = dto.TeamAId,
                    IsHomeTeam = true,
                    MatchId = match.Id
                };
                match.MatchTeams.Add(teamA);
            }

            // Update or add Team B (Away Team)
            var teamB = match.MatchTeams.FirstOrDefault(mt => mt.IsHomeTeam == false);
            if (teamB != null)
            {
                // Update existing Team B
                teamB.TeamId = dto.TeamBId;
            }
            else
            {
                // Add new Team B if it doesn't exist
                teamB = new MatchTeam
                {
                    TeamId = dto.TeamBId,
                    IsHomeTeam = false,
                    MatchId = match.Id
                };
                match.MatchTeams.Add(teamB);
            }

            // Update the match entity in the repository
            _unitOfWork.Repository<Match>().Update(match);

            // Save changes if autoSave is true
            if (autoSave)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await UpdateAsync(dto, autoSave, cancellationToken: cancellationToken);
            return new BaseResponse<Unit>();
        }
        public async ValueTask<BaseResponse<Unit>> UpdateRangeAsync(IEnumerable<UpdateMatchDto> dtos, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await UpdateRangeAsync<UpdateMatchDto>(dtos, autoSave, cancellationToken: cancellationToken);
            return new();
        }
        public async ValueTask<BaseResponse<Unit>> SoftDeleteByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            //var match = await GetByIdAsync(id);
            //if (match == null)
            //{
            //    return new BaseResponse<LanguageExt.Unit>
            //    {
            //        Status = HttpStatusCode.NotFound,
            //        Title = "Match Not Found",
            //        ErrorList = new List<ResponseError>
            //{
            //    new ResponseError("Null Data", "The requested data could not be found.")
            //}
            //    };
            //}
            //await SoftDeleteByIdAsync(id, autoSave, cancellationToken);
            return new Unit();
        }
        public async ValueTask<BaseResponse<Unit>> HardDeleteByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            //var match = await GetByIdAsync(id);
            //if (match == null)
            //{
            //    return new BaseResponse<LanguageExt.Unit>
            //    {
            //        Status = HttpStatusCode.NotFound,
            //        Title = "Match Not Found",
            //        ErrorList = new List<ResponseError>
            //{
            //    new ResponseError("Null Data", "The requested data could not be found.")
            //}
            //    };
            //}
            //await HardDeleteByIdAsync(id, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }
        public async Task<BaseResponse<Unit>> RecoverByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var match = await RecoverByIdAsync(id, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }

    }
}


















//using Application.Interfaces;
//using Application.Interfaces.Persistence;
//using AutoMapper;
//using Domain.Entities;
//using FluentValidation;
//using LanguageExt.Common;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Query;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Localization;
//using Microsoft.Extensions.Logging;
//using Shared.DTOs.MatchDtos;
//using Shared.ResourceFiles;
//using System.Linq.Expressions;

//namespace Application.Implementations
//{
//    public class MatchService : BaseService<MatchO, MatchDto>, IMatchService
//    {
//        public MatchService(IUnitOfWork unitOfWork, ILogger<MatchO> logger, IMemoryCache cache, IMapper mapper, IValidator<MatchDto> validator, IStringLocalizer<Resource> localizer) : base(unitOfWork, logger, cache, mapper, validator, localizer)
//        {
//        }

//        public async Task<Result<IEnumerable<MatchDto>>> GetMatchesAsync(bool useCache, CancellationToken cancellationToken = default)
//        {
//            Expression<Func<IQueryable<MatchDto>, IIncludableQueryable<MatchDto, object>>>[] includeProperties =
//            {
//                x => x.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Stadium).Include(m=>m.Champion)
//            };

//            var result = await GetAllAsync(useCache, cancellationToken: cancellationToken, includeDTOProperties: includeProperties);

//            if (result.IsSuccess)
//            {
//                List<MatchDto> matches = result.Match(
//                    Succ: matches => matches.ToList(),
//                    Fail: ex => new List<MatchDto>()
//                );
//                for (int i = 0; i < matches.Count(); i++)
//                {
//                    if (matches[i].EventDate.HasValue && matches[i].EventTime.HasValue)
//                    {
//                        var matchDateTime = matches[i].EventDate.Value.Add(matches[i].EventTime.Value);
//                        if (DateTime.UtcNow.Subtract(matchDateTime).TotalMinutes > 120)
//                        {
//                            matches[i].IsOver = true;
//                        }
//                        else
//                        {
//                            matches[i].IsOver = false;
//                        }
//                    }
//                }
//                return matches;
//            }
//            else
//            {
//                return result;
//            }
//        }
//        public async Task<Result<IEnumerable<MatchDto>>> GetActiveMatchesAsync(bool useCache, CancellationToken cancellationToken = default)
//        {
//            Expression<Func<IQueryable<MatchDto>, IIncludableQueryable<MatchDto, object>>>[] includeProperties =
//            {
//                 x => x.Include(m => m.TeamA)
//                 .Include(m => m.TeamB)
//                 .Include(m => m.Stadium)
//                 .Include(m => m.Tickets)
//                 .Include(m => m.Champion)

//            };

//            Expression<Func<MatchDto, bool>> matchFilter = m => m.Tickets.Any(t => t.DisplayForSale == true);

//            var result = await FindAsync(matchFilter, useCache, cancellationToken: cancellationToken, includeDTOProperties: includeProperties);

//            if (result.IsSuccess)
//            {
//                List<MatchDto> matches = result.Match(
//                    Succ: matches => matches.ToList(),
//                    Fail: ex => new List<MatchDto>()
//                );

//                foreach (var match in matches)
//                {
//                    if (match.EventDate.HasValue && match.EventTime.HasValue)
//                    {
//                        var matchDateTime = match.EventDate.Value.Add(match.EventTime.Value);
//                        match.IsOver = DateTime.UtcNow.Subtract(matchDateTime).TotalMinutes > 120;
//                    }
//                }

//                var activeMatches = matches.Where(m => !m.IsOver).ToList();

//                return new Result<IEnumerable<MatchDto>>(activeMatches);
//            }
//            else
//            {
//                return result;
//            }
//        }

//        public async Task<Result<MatchDto>> GetMatchByIdAsync(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
//        {
//            Expression<Func<IQueryable<MatchDto>, IIncludableQueryable<MatchDto, object>>>[] includeProperties =
//            {
//                x => x.Include(m => m.TeamA)
//                .Include(m => m.TeamB)
//                .Include(m => m.Stadium)
//                .Include(m => m.Champion)
//            };

//            var result = await GetByIdAsync(id, useCache, cancellationToken: cancellationToken, includeDTOProperties: includeProperties);

//            if (result.IsSuccess)
//            {
//                var match = result.Match(
//                    Succ: matchDto => matchDto,
//                    Fail: ex => null
//                );

//                if (match != null && match.EventDate.HasValue && match.EventTime.HasValue)
//                {
//                    var matchDateTime = match.EventDate.Value.Add(match.EventTime.Value);
//                    if (DateTime.UtcNow.Subtract(matchDateTime).TotalMinutes > 120)
//                    {
//                        match.IsOver = true;
//                    }
//                    else
//                    {
//                        match.IsOver = false;
//                    }
//                }

//                return match;
//            }
//            else
//            {
//                return result;
//            }
//        }
//    }
//}
