using Application.Common.Implementations;
using Application.Common.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.DTOs.Stadium;
using Shared.DTOs.StadiumDTO;
using System.Net;

namespace Application.Stadiums.Service
{
    public class StadiumService : BaseService<Stadium>, IStadiumService
    {
        public StadiumService(IUnitOfWork _unitOfWork, ILogger<Stadium> logger, IMapper mapper) : base(_unitOfWork, logger, mapper) { }

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
            // Validator For Check if the stadium is involved in any match
            var matches = await GetMatchesByStadiumIdAsync(stadiumId, cancellationToken);
            if (matches != null && matches.Any())
            {
                return new BaseResponse<Unit>
                {
                    Status = HttpStatusCode.Conflict,
                    Title = "Cannot Delete Stadium",
                    ErrorList = new List<ResponseError>
                    {
                        new ResponseError
                        {
                            Title = "Conflict",
                            Message = "Cannot delete a stadium involved in a match."
                        }
                    }
                };
            }
            await HardDeleteAsync(stadium);
            return new BaseResponse<Unit>
            {
                Status = HttpStatusCode.OK,
                Title = "Stadium Deleted",
                Data = new Unit(),
                ErrorList = null
            };
        }

        private async Task<IEnumerable<Match>> GetMatchesByStadiumIdAsync(long stadiumId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Match>().ListAsync(result => result.StadiumId == stadiumId, null, cancellationToken);
        }
    }

}
