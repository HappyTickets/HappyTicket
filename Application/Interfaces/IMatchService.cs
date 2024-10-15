using Domain.Entities;
using Shared.DTOs.MatchDtos;

namespace Application.Interfaces
{
    public interface IMatchService : IBaseService<Match>
    {
        Task<IEnumerable<MatchDto>> GetMatchesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<MatchDto>> GetActiveMatchesAsync(CancellationToken cancellationToken = default);
        Task<MatchDto> GetMatchByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
