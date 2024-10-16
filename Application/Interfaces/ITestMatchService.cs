using LanguageExt;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.MatchDtos;

namespace Application.Interfaces
{
    public interface ITestMatchService
    {
        // Retrieve all matches with their details
        ValueTask<BaseResponse<IEnumerable<MatchDto>>> GetAll();

        // Retrieve a match by its ID
        ValueTask<BaseResponse<MatchDto>> GetByIdAsync(long id);

        // Find active matches (you may need to define the predicate based on your logic)
        ValueTask<BaseResponse<IEnumerable<MatchDto>>> FindActiveMatches();

        // Get paginated matches based on search criteria
        ValueTask<BaseResponse<PaginatedList<MatchDto>>> GetPaginatedAsync(PaginationSearchModel paginationParams);

        ValueTask<BaseResponse<long>> GetCountAsync(CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> CreateAsync_(MatchCreateOrUpdateDto dto, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> UpdateAsync_(MatchCreateOrUpdateDto dto, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> SoftDeleteByIdAsync_(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> HardDeleteByIdAsync_(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        Task<BaseResponse<Unit>> RecoverByIdAsync_(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        //Task<BaseResponse<Unit>> RecoverRangeAsync_(Expression<Func<Match, bool>> predicate, bool autoSave = true, CancellationToken cancellationToken = default);
    }


}
