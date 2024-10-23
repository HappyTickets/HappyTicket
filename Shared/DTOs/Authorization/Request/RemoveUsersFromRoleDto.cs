namespace Shared.DTOs.Authorization.Request
{
    public class RemoveUsersFromRoleDto
    {
        public long RoleId { get; set; }
        public List<long> UserIds { get; set; }
    }
}
