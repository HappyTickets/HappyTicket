using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.MatchDtos;
using System.Linq.Expressions;


namespace Application.Implementations
{
    public class TestMatchService : BaseService<Match>, ITestMatchService
    {
        public TestMatchService(IUnitOfWork unitOfWork, ILogger<Match> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
        {
        }

        public async ValueTask<BaseResponse<IEnumerable<MatchDto>>> GetAll()
        {
            // Define the includes for the related properties you want to load
            var includes = new List<Expression<Func<Match, object>>>
                {
                    match => match.Stadium
                };

            // Call GetAllAsync to fetch the data with the specified includes
            var res = await GetAllAsync<MatchDto>(includes: includes);

            // Use the implicit conversion to return a successful BaseResponse
            return res.ToList();
        }
        public async ValueTask<BaseResponse<MatchDto>> GetByIdAsync(long id)
        {
            var match = await GetByIdAsync<MatchDto>(id);
            return new BaseResponse<MatchDto>(match);
        }

        public async ValueTask<BaseResponse<MatchDto>> GetByIdAsync(long id, IEnumerable<Expression<Func<Match, object>>>? includes = null)
        {
            var match = await GetByIdAsync<MatchDto>(id, includes: includes);
            return new BaseResponse<MatchDto>(match);
        }

        public async ValueTask<BaseResponse<IEnumerable<MatchDto>>> FindActiveMatches()
        {

            var matches = await FindAsync<MatchDto>(m => m.IsActive);
            return new BaseResponse<IEnumerable<MatchDto>>(matches);
        }

        public async ValueTask<BaseResponse<PaginatedList<MatchDto>>> GetPaginatedAsync(PaginationSearchModel paginationParams)
        {
            var paginatedMatches = await GetPaginatedAsync<MatchDto>(paginationParams);
            return new BaseResponse<PaginatedList<MatchDto>>(paginatedMatches);
        }
        public async ValueTask<BaseResponse<long>> GetCountAsync(CancellationToken cancellationToken = default)
        {
            var count = await GetLongCountAsync(cancellationToken);
            return new(count);
        }
        // use CreateMatchDto
        public async ValueTask<BaseResponse<Unit>> CreateAsync_(MatchCreateOrUpdateDto dto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await CreateAsync(dto, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }
        // use UpdateMatchDto
        public async ValueTask<BaseResponse<Unit>> UpdateAsync_(MatchCreateOrUpdateDto dto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await UpdateAsync(dto, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }

        public async ValueTask<BaseResponse<Unit>> SoftDeleteByIdAsync_(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await SoftDeleteByIdAsync(id, autoSave, cancellationToken);
            return new Unit();
        }

        public async ValueTask<BaseResponse<Unit>> HardDeleteByIdAsync_(long id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await HardDeleteByIdAsync(id, autoSave, cancellationToken: cancellationToken);
            return new Unit();
        }

        public async Task<BaseResponse<Unit>> RecoverByIdAsync_(long id, bool autoSave = true, CancellationToken cancellationToken = default)
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
