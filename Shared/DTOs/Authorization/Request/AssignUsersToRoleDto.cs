namespace Shared.DTOs.Authorization.Request
{
    public class AssignUsersToRoleDto
    {
        public string RoleId { get; set; }
        public List<long> UserIds { get; set; }
    }
}
