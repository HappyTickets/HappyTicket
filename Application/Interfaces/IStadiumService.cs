using Domain.Entities;
using LanguageExt.Common;
using Shared.DTOs;

namespace Application.Interfaces
{
    public interface IStadiumService : IBaseService<Stadium, StadiumDto>
    {
        Task<Result<StadiumDto>> DeleteStadiumWithNoMatchesAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
