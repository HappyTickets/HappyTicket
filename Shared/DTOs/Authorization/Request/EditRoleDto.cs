using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Authorization.Request
{
    public class EditRoleDto
    {
        public string RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
}
