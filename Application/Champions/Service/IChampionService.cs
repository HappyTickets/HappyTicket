using Shared.Common;
using Shared.DTOs.Champion;
using Shared.DTOs.ChampionDtos;

namespace Application.Interfaces
{
    public interface IChampionService
    {
        ValueTask<BaseResponse<CreateChampionshipDto>> CreateChampionAsync(CreateChampionshipDto dto, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<UpdateChampionshipDto>> UpdateAsync(UpdateChampionshipDto updateChampionshipDto, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<IEnumerable<GetChampionshipDto>>> GetAllChampionshipsAsync(CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<GetChampionshipDto>> GetChampionshipByIdAsync(long championshipId);
        ValueTask<BaseResponse<Object?>> DeleteChampionAsync(long championshipId, CancellationToken cancellationToken = default);
    }
}
