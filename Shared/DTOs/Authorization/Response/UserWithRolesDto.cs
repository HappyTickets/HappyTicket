﻿namespace Shared.DTOs.Authorization.Response
{
    public class UserWithRolesDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public IEnumerable<string> TempRoles { get; set; }
        public IEnumerable<string> AssignedRoles { get; set; }

    }
}
