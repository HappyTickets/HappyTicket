﻿using Shared.Common;
using Shared.DTOs.Stadium;
using Shared.DTOs.StadiumDTO;

namespace Application.Interfaces
{
    public interface IStadiumService
    {
        ValueTask<BaseResponse<GetStadiumDto>> CreateStadiumAsync(CreateStadiumDto createStadiumDto, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<BaseResponse<IEnumerable<GetStadiumDto>>> GetStadiums();
        ValueTask<BaseResponse<GetStadiumDto>> GetStaduimByIdAsync(long stadiumId);
    }
}
