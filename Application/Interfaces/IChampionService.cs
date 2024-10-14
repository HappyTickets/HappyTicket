using Domain.Entities;
using LanguageExt.Common;
using Shared.DTOs.Champion;

namespace Application.Interfaces
{
    public interface IChampionService: IBaseService<ChampionO, ChampionDto>
    {
        Task<Result<ChampionDto>> CreateAsync(CreateOrUpdateChampionDto dto, CancellationToken cancellationToken = default);
        Task<Result<ChampionDto>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<ChampionDto>>> GetAllAsync(bool useCache = true, CancellationToken cancellationToken = default);
        Task<Result<ChampionDto>> GetByIdAsync(Guid id, bool useCache = true, CancellationToken cancellationToken = default);
        Task<Result<ChampionDto>> UpdateAsync(Guid id, CreateOrUpdateChampionDto dto, CancellationToken cancellationToken = default);
    }
}
