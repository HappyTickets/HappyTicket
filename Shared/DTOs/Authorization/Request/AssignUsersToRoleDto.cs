namespace Shared.DTOs.Authorization.Request
{
    public class AssignUsersToRoleDto
    {
        public string Role { get; set; }
        public List<string> UserIds { get; set; }
    }
}
