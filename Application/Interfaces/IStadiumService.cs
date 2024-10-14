using Domain.Entities;
using LanguageExt.Common;
using Shared.DTOs;

namespace Application.Interfaces
{
    public interface IStadiumService : IBaseService<StadiumO, StadiumDto>
    {
        Task<Result<StadiumDto>> DeleteStadiumWithNoMatchesAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
