using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.DTOs.Stadium;
using Shared.DTOs.StadiumDTO;

namespace Application.Implementations
{
    public class StadiumService : BaseService<Stadium>, IStadiumService
    {
        public StadiumService(IUnitOfWork unitOfWork, ILogger<Stadium> logger, IMapper mapper) : base(unitOfWork, logger, mapper) { }

        public async ValueTask<BaseResponse<GetStadiumDto>> CreateStadiumAsync(CreateStadiumDto createStadiumDto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var stadium = _mapper.Map<Stadium>(createStadiumDto);
            await CreateAsync(createStadiumDto, autoSave, cancellationToken: cancellationToken);
            var stadiumDtoResult = _mapper.Map<GetStadiumDto>(stadium);
            return stadiumDtoResult;
        }

        public async ValueTask<BaseResponse<IEnumerable<GetStadiumDto>>> GetStadiums()
        {
            var stadiums = await GetAllAsync<GetStadiumDto>();
            if (stadiums != null && stadiums.Any())
                return new BaseResponse<IEnumerable<GetStadiumDto>>(stadiums.ToList());
            return new BaseResponse<IEnumerable<GetStadiumDto>>();
        }
        public async ValueTask<BaseResponse<GetStadiumDto>> GetStaduimByIdAsync(long stadiumId)
        {
            var match = await GetByIdAsync<GetStadiumDto>(stadiumId);
            return new BaseResponse<GetStadiumDto>(match);
        }
    }

}
