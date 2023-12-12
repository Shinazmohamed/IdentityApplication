namespace IdentityApplication.Core.PermissionHelper
{
    public static class PermissionsModel
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }
        public static class Employee
        {
            public const string View = "Permissions.Employee.View";
            public const string Create = "Permissions.Employee.Create";
            public const string Edit = "Permissions.Employee.Edit";
            public const string Delete = "Permissions.Employee.Delete";
        }
        public static class Entity
        {
            public const string View = "Permissions.Entity.View";
            public const string Create = "Permissions.Entity.Create";
            public const string Edit = "Permissions.Entity.Edit";
            public const string Delete = "Permissions.Entity.Delete";
        }
        public static class Permission
        {
            public const string View = "Permissions.Permission.View";
            public const string Create = "Permissions.Permission.Create";
            public const string Edit = "Permissions.Permission.Edit";
            public const string Delete = "Permissions.Permission.Delete";
        }
    }
}
