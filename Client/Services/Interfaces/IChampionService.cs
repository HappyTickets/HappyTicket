using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs.Champion;

namespace Client.Services.Interfaces
{
    public interface IChampionService
    {
        Task<Result<BaseResponse<ChampionDto>>> CreateChampionAsync(CreateOrUpdateChampionDto champion, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<ChampionDto>>> DeleteChampionAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<ChampionDto>>> GetChampionByIdAsync(Guid id, bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<IEnumerable<ChampionDto>>>> GetChampionsAsync(bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<ChampionDto>>> UpdateChampionAsync(Guid id, CreateOrUpdateChampionDto champion, CancellationToken cancellationToken = default);
    }
}
