using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Authorization.Request
{
    public class AddRoleDto
    {
        [Required]
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
    }
}
