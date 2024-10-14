//using Application.Interfaces;
//using Application.Interfaces.Persistence;
//using AutoMapper;
//using Domain.Entities;
//using FluentValidation;
//using LanguageExt.Common;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Localization;
//using Microsoft.Extensions.Logging;
//using Shared.DTOs;
//using Shared.Extensions;
//using Shared.ResourceFiles;

//namespace Application.Implementations
//{
//    public class StadiumService : BaseService<StadiumO, StadiumDto>, IStadiumService
//    {
//        public StadiumService(IUnitOfWork unitOfWork, ILogger<StadiumO> logger, IMemoryCache cache, IMapper mapper, IValidator<StadiumDto> validator, IStringLocalizer<Resource> localizer) : base(unitOfWork, logger, cache, mapper, validator, localizer) { }
//        public async Task<Result<StadiumDto>> DeleteStadiumWithNoMatchesAsync(Guid id, CancellationToken cancellationToken = default)
//        {
//            var matchesRepo = _unitOfWork.Repository<MatchO>();
//            var stadiumRepo = _unitOfWork.Repository<StadiumO>();

//            if (await matchesRepo.Query().AnyAsync(m => m.StadiumId == id))
//                return new(new Exception(Resource.Stadium_With_Match_Deletion_Failure));

//            var result = await stadiumRepo.HardDeleteByIdAsync(id, cancellationToken);
//            return await result.Match(
//                async stadium =>
//                {
//                    await _unitOfWork.SaveChangesAsync();
//                    return _mapper.Map<StadiumDto>(stadium).ToResult();
//                },
//                async ex => await ex.ToResultAsync<StadiumDto>());
//        }
//    }

//}
