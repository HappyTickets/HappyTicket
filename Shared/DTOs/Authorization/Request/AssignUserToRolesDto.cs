namespace Shared.DTOs.Authorization.Request
{
    public class AssignUserToRolesDto
    {
        public List<string> Roles { get; set; }
        public string UserId { get; set; }
    }
}
