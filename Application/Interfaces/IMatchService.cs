using Domain.Entities;
using LanguageExt.Common;
using Shared.DTOs.MatchDtos;

namespace Application.Interfaces
{
    public interface IMatchService : IBaseService<Match, MatchDto>
    {
        Task<Result<IEnumerable<MatchDto>>> GetMatchesAsync(bool useCache, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<MatchDto>>> GetActiveMatchesAsync(bool useCache, CancellationToken cancellationToken = default);
        Task<Result<MatchDto>> GetMatchByIdAsync(Guid id, bool useCache = true, CancellationToken cancellationToken = default);
    }
}
