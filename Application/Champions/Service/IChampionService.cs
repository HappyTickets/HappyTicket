using Shared.Common;
using Shared.DTOs.Champion;

namespace Application.Interfaces
{
    public interface IChampionService
    {
        ValueTask<BaseResponse<ChampionDto>> CreateAsync(CreateOrUpdateChampionDto dto, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<ChampionDto>> UpdateAsync(CreateOrUpdateChampionDto dto, CancellationToken cancellationToken = default);
        //Task<Result<ChampionDto>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        //Task<Result<IEnumerable<ChampionDto>>> GetAllAsync(bool useCache = true, CancellationToken cancellationToken = default);
        //Task<Result<ChampionDto>> GetByIdAsync(Guid id, bool useCache = true, CancellationToken cancellationToken = default);
    }
}
