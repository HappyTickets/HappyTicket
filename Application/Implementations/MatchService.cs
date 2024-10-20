using Application.Common.Implementations;
using Application.Common.Interfaces.Persistence;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.MatchDtos;

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
                    nameof(Match.Champion)
                };

            var result = await GetAllAsync<GetAllMatchesDto>(includes: includes);

            return result.ToList();
        }
        public async ValueTask<BaseResponse<GetMatchByIdDto>> GetByIdAsync(long id)
        {
            var match = await GetByIdAsync<GetMatchByIdDto>(id);
            return new BaseResponse<GetMatchByIdDto>(match);
        }
        public async ValueTask<BaseResponse<GetMatchByIdDto>> GetByIdAsync(long id, IEnumerable<string>? includes = null)
        {
            var match = await GetByIdAsync<GetMatchByIdDto>(id, includes: includes);
            return new BaseResponse<GetMatchByIdDto>(match);
        }
        public async ValueTask<BaseResponse<IEnumerable<FindActiveMatchesDto>>> FindActiveMatches()
        {
            var matches = await FindAsync<FindActiveMatchesDto>(m => m.IsActive);
            return new BaseResponse<IEnumerable<FindActiveMatchesDto>>(matches);
        }
        public async ValueTask<BaseResponse<PaginatedList<GetPaginatedMatchesDto>>> GetPaginatedAsync(PaginationSearchModel paginationParams)
        {
            var paginatedMatches = await GetPaginatedAsync<GetPaginatedMatchesDto>(paginationParams);
            return new BaseResponse<PaginatedList<GetPaginatedMatchesDto>>(paginatedMatches);
        }
        public async ValueTask<BaseResponse<long>> GetCountAsync(CancellationToken cancellationToken = default)
        {
            var count = await GetLongCountAsync(cancellationToken);
            return new(count);
        }
        public async ValueTask<BaseResponse<Unit>> CreateAsync(CreateMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await CreateAsync(dto, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }
        public async ValueTask<BaseResponse<Unit>> UpdateAsync(UpdateMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await UpdateAsync(dto, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }
        public async ValueTask<BaseResponse<Unit>> UpdateRangeAsync(IEnumerable<UpdateMatchDto> dtos, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await UpdateRangeAsync<UpdateMatchDto>(dtos, autoSave, cancellationToken: cancellationToken);
            return new();
        }
        public async ValueTask<BaseResponse<Unit>> SoftDeleteByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var match = await GetByIdAsync(id);
            await SoftDeleteByIdAsync(id, autoSave, cancellationToken);
            return new Unit();
        }
        public async ValueTask<BaseResponse<Unit>> HardDeleteByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var match = await GetByIdAsync(id);
            await HardDeleteByIdAsync(id, autoSave, cancellationToken: cancellationToken);
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
