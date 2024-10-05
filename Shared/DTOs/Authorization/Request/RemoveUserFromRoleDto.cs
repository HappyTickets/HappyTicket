namespace Shared.DTOs.Authorization.Request
{
    public class RemoveUserFromRoleDto
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
    }
}
