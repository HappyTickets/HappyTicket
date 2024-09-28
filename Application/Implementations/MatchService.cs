using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Shared.DTOs.MatchDtos;
using Shared.ResourceFiles;
using System.Linq.Expressions;

namespace Application.Implementations
{
    public class MatchService : BaseService<Match, MatchDto>, IMatchService
    {
        public MatchService(IUnitOfWork unitOfWork, ILogger<Match> logger, IMemoryCache cache, IMapper mapper, IValidator<MatchDto> validator, IStringLocalizer<Resource> localizer) : base(unitOfWork, logger, cache, mapper, validator, localizer)
        {
        }

        public async Task<Result<IEnumerable<MatchDto>>> GetMatchesAsync(bool useCache, CancellationToken cancellationToken = default)
        {
            Expression<Func<IQueryable<MatchDto>, IIncludableQueryable<MatchDto, object>>>[] includeProperties =
            {
                x => x.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Stadium)
            };

            var result = await GetAllAsync(useCache, cancellationToken: cancellationToken, includeDTOProperties: includeProperties);

            if (result.IsSuccess)
            {
                List<MatchDto> matches = result.Match(
                    Succ: matches => matches.ToList(),
                    Fail: ex => new List<MatchDto>()
                );
                for (int i = 0; i < matches.Count(); i++)
                {
                    if (matches[i].EventDate.HasValue && matches[i].EventTime.HasValue)
                    {
                        var matchDateTime = matches[i].EventDate.Value.Add(matches[i].EventTime.Value);
                        if (DateTime.UtcNow.Subtract(matchDateTime).TotalMinutes > 120)
                        {
                            matches[i].IsOver = true;
                        }
                        else
                        {
                            matches[i].IsOver = false;
                        }
                    }
                }
                return matches;
            }
            else
            {
                return result;
            }
        }
        public async Task<Result<IEnumerable<MatchDto>>> GetActiveMatchesAsync(bool useCache, CancellationToken cancellationToken = default)
        {
            Expression<Func<IQueryable<MatchDto>, IIncludableQueryable<MatchDto, object>>>[] includeProperties =
            {
        x => x.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Stadium).Include(m => m.Tickets)
    };

            Expression<Func<MatchDto, bool>> matchFilter = m => m.Tickets.Any(t => t.DisplayForSale == true);

            var result = await FindAsync(matchFilter, useCache, cancellationToken: cancellationToken, includeDTOProperties: includeProperties);

            if (result.IsSuccess)
            {
                List<MatchDto> matches = result.Match(
                    Succ: matches => matches.ToList(),
                    Fail: ex => new List<MatchDto>()
                );

                foreach (var match in matches)
                {
                    if (match.EventDate.HasValue && match.EventTime.HasValue)
                    {
                        var matchDateTime = match.EventDate.Value.Add(match.EventTime.Value);
                        match.IsOver = DateTime.UtcNow.Subtract(matchDateTime).TotalMinutes > 120;
                    }
                }

                var activeMatches = matches.Where(m => !m.IsOver).ToList();

                return new Result<IEnumerable<MatchDto>>(activeMatches);
            }
            else
            {
                return result;
            }
        }

        public async Task<Result<MatchDto>> GetMatchByIdAsync(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
        {
            Expression<Func<IQueryable<MatchDto>, IIncludableQueryable<MatchDto, object>>>[] includeProperties =
            {
                x => x.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Stadium)
            };

            var result = await GetByIdAsync(id, useCache, cancellationToken: cancellationToken, includeDTOProperties: includeProperties);

            if (result.IsSuccess)
            {
                var match = result.Match(
                    Succ: matchDto => matchDto,
                    Fail: ex => null
                );

                if (match != null && match.EventDate.HasValue && match.EventTime.HasValue)
                {
                    var matchDateTime = match.EventDate.Value.Add(match.EventTime.Value);
                    if (DateTime.UtcNow.Subtract(matchDateTime).TotalMinutes > 120)
                    {
                        match.IsOver = true;
                    }
                    else
                    {
                        match.IsOver = false;
                    }
                }

                return match;
            }
            else
            {
                return result;
            }
        }
    }
}
