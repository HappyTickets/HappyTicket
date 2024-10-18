using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.DTOs.Stadium;

namespace Application.Implementations
{
    public class StadiumService : BaseService<Stadium>, IStadiumService
    {
        public StadiumService(IUnitOfWork unitOfWork, ILogger<Stadium> logger, IMapper mapper) : base(unitOfWork, logger, mapper) { }
        public async ValueTask<BaseResponse<IEnumerable<GetStadiumDto>>> GetStadiums()
        {
            var stadiums = await GetAllAsync<GetStadiumDto>();
            if (stadiums != null && stadiums.Any())
                return new BaseResponse<IEnumerable<GetStadiumDto>>(stadiums.ToList());
            return new BaseResponse<IEnumerable<GetStadiumDto>>();
        }
    }

}
