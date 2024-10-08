namespace Shared.Common.ApiRoutes
{
    public static class ApiRoutes
    {

        public const string API = "api";
        public static class User
        {
            public const string GetAll = "GetAll";
        }
        public static class Authorization
        {
            public const string Base = API + "/Authorization/";

            public const string CreateRole = "CreateRole";
            public const string EditRole = "EditRole";
            public const string DeleteRole = "DeleteRole/{id}";
            public const string RolesList = "RolesList";
            public const string GetRoleById = "Role-by-id/{id}";

            public const string AssignUserToRoles = "assign-user-to-roles";
            public const string AssignUsersToRole = "assign-users-to-role";
            public const string RemoveUsersFromRole = "remove-users-from-role";
            public const string GetUserWithRoles = "get-user-with-roles/{userId}";
            public const string GetUsersWithRoles = "get-users-with-roles";
            public const string GetRoleWithUsers = "get-role-with-users/{roleId}";
        }

    }
}
