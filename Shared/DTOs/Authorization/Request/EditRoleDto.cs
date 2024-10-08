namespace Shared.DTOs.Authorization.Request
{
    public class EditRoleDto
    {
        public string RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
    }
}
