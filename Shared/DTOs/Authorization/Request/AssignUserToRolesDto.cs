namespace Shared.DTOs.Authorization.Request
{
    public class AssignUserToRolesDto
    {
        public List<string> Roles { get; set; }
        public long UserId { get; set; }
    }
}
