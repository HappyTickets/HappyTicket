using Shared.Common.General;
using Shared.DTOs.Identity.UserDTOs;

namespace Shared.DTOs.Authorization.Response
{
    public class RoleWithUsersDto
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public PaginatedList<ApplicationUserDTO> AssignedUsers { get; set; }
    }

}
