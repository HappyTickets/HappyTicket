using Application.Common.Implementations;
using Application.Common.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Test.Request;
using Shared.DTOs.Test.Response;


namespace Application.Tests.Matches.Service
{
    public class TestMatchService : BaseService<Match>, ITestMatchService
    {
        public TestMatchService(IUnitOfWork unitOfWork, ILogger<Match> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
        {
        }
        /*************************************************************************
         
            create a suitable dto for different operations.

            pass localized ErrorMessages into baseResponse

            return  the result direct. thank to implict operator in BaseRespnse

            if u need any help just call Anas 😁

         **********************************************************************************/
        public async ValueTask<BaseResponse<IEnumerable<GetMatchDto>>> GetAll()
        {
            // Define the includes for the related properties you want to load
            var includes = new List<string>
                {
                    nameof(Match.Stadium),
                    nameof(Match.Champion)
                };

            // Call GetAllAsync to fetch the data with the specified includes
            var res = await GetAllAsync<GetMatchDto>(includes: includes);

            // Use the implicit conversion to return a successful BaseResponse
            return res.ToList();
        }
        public async ValueTask<BaseResponse<GetMatchDto>> GetByIdAsync(long id)
        {
            var match = await GetByIdAsync<GetMatchDto>(id);
            return new BaseResponse<GetMatchDto>(match);
        }

        public async ValueTask<BaseResponse<GetMatchDto>> GetByIdAsync(long id, IEnumerable<string>? includes = null)
        {
            var match = await GetByIdAsync<GetMatchDto>(id, includes: includes);
            return new BaseResponse<GetMatchDto>(match);
        }

        public async ValueTask<BaseResponse<IEnumerable<GetMatchDto>>> FindActiveMatches()
        {

            var matches = await FindAsync<GetMatchDto>(m => m.IsActive);
            return new BaseResponse<IEnumerable<GetMatchDto>>(matches);
        }

        public async ValueTask<BaseResponse<PaginatedList<GetMatchDto>>> GetPaginatedAsync(PaginationParams paginationParams)
        {
            var paginatedMatches = await GetPaginatedAsync<GetMatchDto>(paginationParams);
            return new BaseResponse<PaginatedList<GetMatchDto>>(paginatedMatches);
        }
        public async ValueTask<BaseResponse<long>> GetCountAsync(CancellationToken cancellationToken = default)
        {
            var count = await GetLongCountAsync(cancellationToken);
            return new(count);
        }
        // use CreateMatchDto
        public async ValueTask<BaseResponse<Unit>> CreateAsyncTest(CreateTestMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await CreateAsync(dto, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }
        // use UpdateMatchDto
        public async ValueTask<BaseResponse<Unit>> UpdateAsyncTest(UpdateTestMatchDto dto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await UpdateAsync(dto, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }

        public async ValueTask<BaseResponse<Unit>> UpdateRangeAsyncTest(IEnumerable<UpdateTestMatchDto> dtos, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await UpdateRangeAsync(dtos, autoSave, cancellationToken: cancellationToken);
            return new();
        }
        public async ValueTask<BaseResponse<Unit>> SoftDeleteByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await SoftDeleteByIdAsync(id, autoSave, cancellationToken);
            return new Unit();
        }

        public async ValueTask<BaseResponse<Unit>> HardDeleteByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await HardDeleteByIdAsync(id, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }

        public async Task<BaseResponse<Unit>> RecoverByIdAsyncTest(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var match = await RecoverByIdAsync(id, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }



        //public async Task<BaseResponse<Unit>> RecoverRangeAsync_(Expression<Func<Match, bool>> predicate, bool autoSave = true, CancellationToken cancellationToken = default)
        //{
        //    return await RecoverRangeAsync(predicate, autoSave, cancellationToken: cancellationToken);
        //}

    }
}
