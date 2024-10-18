using Shared.Common;
using Shared.DTOs.Stadium;

namespace Application.Interfaces
{
    public interface IStadiumService
    {
        ValueTask<BaseResponse<IEnumerable<GetStadiumDto>>> GetStadiums();
        ValueTask<BaseResponse<GetStadiumDto>> GetStaduimById(long stadiumId);
    }
}
