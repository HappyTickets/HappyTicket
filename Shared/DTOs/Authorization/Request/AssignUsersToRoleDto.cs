namespace Shared.DTOs.Authorization.Request
{
    public class AssignUsersToRoleDto
    {
        public long RoleId { get; set; }
        public List<long> UserIds { get; set; }
    }
}
