namespace Shared.DTOs.Authorization.Request
{
    public class RemoveUsersFromRoleDto
    {
        public string RoleId { get; set; }
        public List<string> UserIds { get; set; }
    }
}
