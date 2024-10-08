namespace Shared.DTOs.Authorization.Response
{
    public class RoleDto
    {
        public string RoleId { get; set; }
        public required string RoleName { get; set; }
        public required string RoleDescription { get; set; }
    }
}
