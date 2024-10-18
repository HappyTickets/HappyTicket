using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.MatchDtos;
using Shared.DTOs.Test.Request;
using Shared.DTOs.Test.Response;

namespace Application.Interfaces
{

    public interface IMatchService
    {
        // Retrieve all matches with their details
        ValueTask<BaseResponse<IEnumerable<GetMatchDto>>> GetAll();

        // Retrieve a match by its ID
        ValueTask<BaseResponse<GetMatchDto>> GetByIdAsync(long id);

        // Find active matches (you may need to define the predicate based on your logic)
        ValueTask<BaseResponse<IEnumerable<GetMatchDto>>> FindActiveMatches();

        // Get paginated matches based on search criteria
        ValueTask<BaseResponse<PaginatedList<GetMatchDto>>> GetPaginatedAsync(PaginationSearchModel paginationParams);

        ValueTask<BaseResponse<long>> GetCountAsync(CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> CreateAsyncTest(CreateTestMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> UpdateAsyncTest(UpdateTestMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> UpdateRangeAsyncTest(IEnumerable<UpdateTestMatchDto> dtos, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> SoftDeleteByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> HardDeleteByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        Task<BaseResponse<Unit>> RecoverByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default);

    }
}


















//using Domain.Entities;
//using LanguageExt.Common;
//using Shared.DTOs.MatchDtos;

//namespace Application.Interfaces
//{
//    public interface IMatchService : IBaseService<MatchO, MatchDto>
//    {
//        Task<Result<IEnumerable<MatchDto>>> GetMatchesAsync(bool useCache, CancellationToken cancellationToken = default);
//        Task<Result<IEnumerable<MatchDto>>> GetActiveMatchesAsync(bool useCache, CancellationToken cancellationToken = default);
//        Task<Result<MatchDto>> GetMatchByIdAsync(Guid id, bool useCache = true, CancellationToken cancellationToken = default);
//    }
//}
