using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.DTOs.Stadium;
using Shared.DTOs.StadiumDTO;
using System.Net;

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
        public async ValueTask<BaseResponse<UpdateStadiumDto>> UpdateAsync([FromBody] UpdateStadiumDto stadiumDto)
        {
            var stadium = await GetByIdAsync<Stadium>(stadiumDto.Id);
            if (stadium == null)
            {
                return new BaseResponse<UpdateStadiumDto>
                {
                    Status = HttpStatusCode.BadRequest,
                    Title = "Stadium not found"
                };
            }
            _mapper.Map(stadiumDto, stadium);
            await UpdateAsync(stadium);
            return stadiumDto;
        }
        public async ValueTask<BaseResponse<IEnumerable<GetStadiumDto>>> GetStadiums()
        {
            var stadiums = await GetAllAsync<GetStadiumDto>();
            if (stadiums != null && stadiums.Any())
                return new BaseResponse<IEnumerable<GetStadiumDto>>(stadiums.ToList());
            return new BaseResponse<IEnumerable<GetStadiumDto>>
            {
                Status = HttpStatusCode.NotFound,
                Title = "No Stadiums Found",
                ErrorList = new List<ResponseError>
                {
                    new ResponseError
                    {
                        Title = "Data Not Found",
                        Message = "There are no stadiums available."
                    }
                }
            };
        }
        public async ValueTask<BaseResponse<GetStadiumDto>> GetStaduimByIdAsync(long stadiumId)
        {
            var match = await GetByIdAsync<GetStadiumDto>(stadiumId);
            if (match == null)
            {
                return new BaseResponse<GetStadiumDto>
                {
                    Status = HttpStatusCode.NotFound,
                    Title = "Match Not Found",
                    ErrorList = new List<ResponseError>
                {
                    new ResponseError
                    {
                        Title = "Data Not Found",
                        Message = $"Match with ID {stadiumId} does not exist"
                    }
                }
                };
            }
            return new BaseResponse<GetStadiumDto>(match);
        }

        public async ValueTask<BaseResponse<Unit>> DeleteStadiumAsync(long stadiumId, CancellationToken cancellationToken = default)
        {
            var stadium = await GetByIdAsync<Stadium>(stadiumId, cancellationToken);
            if (stadium == null)
            {
                return new BaseResponse<Unit>
                {
                    Status = HttpStatusCode.NotFound,
                    Title = "Stadium not found",
                    ErrorList = new List<ResponseError>
                    {
                        new ResponseError
                        {
                            Title = "Data Not Found",
                            Message = $"Stadium with ID {stadiumId} does not exist."
                        }
                    }
                };
            }
            // TODO Check
            //var matches = await GetMatchesByStadiumIdAsync(stadiumId, cancellationToken);
            await HardDeleteAsync(stadium);
            return new Unit();
        }
    }

}
