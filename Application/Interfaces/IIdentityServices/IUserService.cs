using LanguageExt.Common;
using Shared.Common.General;
using Shared.DTOs.Identity.UserDTOs;

namespace Application.Interfaces.IIdentityServices
{
    public interface IUserService
    {
        Task<Result<PaginatedList<ApplicationUserDTO>>> GetAllUsersAsync(PaginationSearchModel queryParams, CancellationToken cancellationToken);
    }
}
