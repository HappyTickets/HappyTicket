namespace Shared.DTOs.Authorization.Request
{
    public class AddRoleDto
    {
        public required string RoleName { get; set; }
        public required string RoleDescription { get; set; }
    }
}
