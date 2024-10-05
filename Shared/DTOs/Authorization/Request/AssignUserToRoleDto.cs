namespace Shared.DTOs.Authorization.Request
{
    public class AssignUserToRoleDto
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
    }
}
