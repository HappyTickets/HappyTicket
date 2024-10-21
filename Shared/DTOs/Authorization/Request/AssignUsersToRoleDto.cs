namespace Shared.DTOs.Authorization.Request
{
    public class AssignUsersToRoleDto
    {
        public string RoleId { get; set; }
        public List<string> UserIds { get; set; }
    }
}
