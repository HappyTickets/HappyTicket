using LanguageExt;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Test.Request;
using Shared.DTOs.Test.Response;

namespace Application.Tests.Matches.Service
{
    public interface ITestMatchService
    {
        // Retrieve all matches with their details
        ValueTask<BaseResponse<IEnumerable<GetMatchDto>>> GetAll();

        // Retrieve a match by its ID
        ValueTask<BaseResponse<GetMatchDto>> GetByIdAsync(long id);

        // Find active matches (you may need to define the predicate based on your logic)
        ValueTask<BaseResponse<IEnumerable<GetMatchDto>>> FindActiveMatches();

        // Get paginated matches based on search criteria
        ValueTask<BaseResponse<PaginatedList<GetMatchDto>>> GetPaginatedAsync(PaginationParams paginationParams);

        ValueTask<BaseResponse<long>> GetCountAsync(CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> CreateAsyncTest(CreateTestMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> UpdateAsyncTest(UpdateTestMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> UpdateRangeAsyncTest(IEnumerable<UpdateTestMatchDto> dtos, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> SoftDeleteByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> HardDeleteByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        Task<BaseResponse<Unit>> RecoverByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        //Task<BaseResponse<Unit>> RecoverRangeAsync_(Expression<Func<Match, bool>> predicate, bool autoSave = true, CancellationToken cancellationToken = default);
    }


}
