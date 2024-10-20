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
        ValueTask<BaseResponse<IEnumerable<GetAllMatchesDto>>> GetAll();

        // Retrieve a match by its ID
        ValueTask<BaseResponse<GetMatchByIdDto>> GetByIdAsync(long id);

        // Find active matches (you may need to define the predicate based on your logic)
        ValueTask<BaseResponse<IEnumerable<FindActiveMatchesDto>>> FindActiveMatches();

        // Get paginated matches based on search criteria
        ValueTask<BaseResponse<PaginatedList<GetPaginatedMatchesDto>>> GetPaginatedAsync(PaginationSearchModel paginationParams);

        ValueTask<BaseResponse<long>> GetCountAsync(CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> CreateMatchAsync(CreateMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> UpdateAsync(UpdateMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> UpdateRangeAsync(IEnumerable<UpdateMatchDto> dtos, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> SoftDeleteByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<Unit>> HardDeleteByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        Task<BaseResponse<Unit>> RecoverByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default);

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
