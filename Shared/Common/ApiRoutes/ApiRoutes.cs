namespace Shared.Common.ApiRoutes
{
    public static class ApiRoutes
    {
        public static class User
        {
            public const string GetAll = "GetAll";
        }
        public static class Authorization
        {
            public const string Roles = "/Roles";
            public const string Create = Roles + "/Create";
            public const string Edit = Roles + "/Edit";
            public const string Delete = Roles + "/Delete/{id}";
            public const string RolesList = Roles + "/RoleList";
            public const string GetRoleById = Roles + "/RoleById/{id}";
            public const string ManageUserRoles = Roles + "/ManageUserRoles/{userId}";
            public const string UpdateUserRoles = Roles + "/UpdateUserRoles";
        }
    }
}
