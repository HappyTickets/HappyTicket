using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Test.Request;
using Shared.DTOs.Test.Response;
using Shared.Exceptions;
using System.Linq.Expressions;

namespace Application.Implementations
{
    public class MatchService : BaseService<Match>, IMatchService
    {
        public MatchService(IUnitOfWork unitOfWork, ILogger<Match> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
        {
        }

        public async ValueTask<BaseResponse<IEnumerable<GetMatchDto>>> GetAll()
        {
            var includes = new List<Expression<Func<Match, object>>>
                {
                    match => match.Stadium,
                    match => match.Champion
                };

            var result = await GetAllAsync<GetMatchDto>(includes: includes);

            return result.ToList();
        }

        public async ValueTask<BaseResponse<GetMatchDto>> GetByIdAsync(long id)
        {
            var match = await GetByIdAsync<GetMatchDto>(id);
            return new BaseResponse<GetMatchDto>(match);
        }
        public async ValueTask<BaseResponse<GetMatchDto>> GetByIdAsync(long id, IEnumerable<Expression<Func<Match, object>>>? includes = null)
        {
            var match = await GetByIdAsync<GetMatchDto>(id, includes: includes);
            return new BaseResponse<GetMatchDto>(match);
        }
        public async ValueTask<BaseResponse<IEnumerable<GetMatchDto>>> FindActiveMatches()
        {
            var matches = await FindAsync<GetMatchDto>(m => m.IsActive);
            return new BaseResponse<IEnumerable<GetMatchDto>>(matches);
        }
        public async ValueTask<BaseResponse<PaginatedList<GetMatchDto>>> GetPaginatedAsync(PaginationSearchModel paginationParams)
        {
            var paginatedMatches = await GetPaginatedAsync<GetMatchDto>(paginationParams);
            return new BaseResponse<PaginatedList<GetMatchDto>>(paginatedMatches);
        }
        public async ValueTask<BaseResponse<long>> GetCountAsync(CancellationToken cancellationToken = default)
        {
            var count = await GetLongCountAsync(cancellationToken);
            return new(count);
        }
        public async ValueTask<BaseResponse<Unit>> CreateAsyncTest(CreateTestMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await CreateAsync(dto, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }
        public async ValueTask<BaseResponse<Unit>> UpdateAsyncTest(UpdateTestMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await UpdateAsync(dto, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }
        public async ValueTask<BaseResponse<Unit>> UpdateRangeAsyncTest(IEnumerable<UpdateTestMatchDto> dtos, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await UpdateRangeAsync<UpdateTestMatchDto>(dtos, autoSave, cancellationToken: cancellationToken);
            return new();
        }
        public async ValueTask<BaseResponse<Unit>> SoftDeleteByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var match = await GetByIdAsync(id);


            await SoftDeleteByIdAsync(id, autoSave, cancellationToken);
            return new Unit();


        }

        public async ValueTask<BaseResponse<Unit>> HardDeleteByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var match = await GetByIdAsync(id);


            await HardDeleteByIdAsync(id, autoSave, cancellationToken: cancellationToken);
            return new Unit();


        }

        public async Task<BaseResponse<Unit>> RecoverByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default)
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
