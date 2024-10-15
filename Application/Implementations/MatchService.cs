using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Shared.DTOs.MatchDtos;
using System.Linq.Expressions;

namespace Application.Implementations
{
    public class MatchService : BaseService<Match>, IMatchService
    {
        public MatchService(IUnitOfWork unitOfWork, ILogger<Match> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
        {
        }

        public async Task<IEnumerable<MatchDto>> GetMatchesAsync(CancellationToken cancellationToken = default)
        {
            Expression<Func<IQueryable<MatchDto>, IIncludableQueryable<MatchDto, object>>>[] includeProperties =
            {
                x => x.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Stadium).Include(m=>m.Champion)
            };

            return await GetAllAsync<MatchDto>(cancellationToken: cancellationToken, includeDTOProperties: includeProperties);
        }
        public async Task<IEnumerable<MatchDto>> GetActiveMatchesAsync(CancellationToken cancellationToken = default)
        {
            Expression<Func<IQueryable<MatchDto>, IIncludableQueryable<MatchDto, object>>>[] includeProperties =
            {
                 x => x.Include(m => m.TeamA)
                 .Include(m => m.TeamB)
                 .Include(m => m.Stadium)
                 .Include(m => m.Tickets)
                 .Include(m => m.Champion)

            };

            Expression<Func<MatchDto, bool>> matchFilter = m => m.Tickets.Any(t => t.DisplayForSale == true);

            var result = await FindAsync(matchFilter, cancellationToken: cancellationToken, includeDTOProperties: includeProperties);


            List<MatchDto> matches = result.ToList();

            foreach (var match in matches)
            {
                if (match.EventDate.HasValue && match.EventTime.HasValue)
                {
                    var matchDateTime = match.EventDate.Value.Add(match.EventTime.Value);
                    match.IsOver = DateTime.UtcNow.Subtract(matchDateTime).TotalMinutes > 120;
                }
            }

            var activeMatches = matches.Where(m => !m.IsOver).ToList();

            return activeMatches;

        }

        public async Task<MatchDto> GetMatchByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            Expression<Func<IQueryable<MatchDto>, IIncludableQueryable<MatchDto, object>>>[] includeProperties =
            {
                x => x.Include(m => m.TeamA)
                .Include(m => m.TeamB)
                .Include(m => m.Stadium)
                .Include(m => m.Champion)
            };

            var match = await GetByIdAsync(id, cancellationToken: cancellationToken, includeDTOProperties: includeProperties);

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


    }
}
