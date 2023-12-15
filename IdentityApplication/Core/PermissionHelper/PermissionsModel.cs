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
        public static class EmployeePermission
        {
            public const string View = "Permissions.Employee.View";
            public const string Create = "Permissions.Employee.Create";
            public const string Edit = "Permissions.Employee.Edit";
            public const string Delete = "Permissions.Employee.Delete";
        }
        public static class EntityPermission
        {
            public const string View = "Permissions.Entity.View";
            public const string Create = "Permissions.Entity.Create";
            public const string Edit = "Permissions.Entity.Edit";
            public const string Delete = "Permissions.Entity.Delete";
        }
        public static class PermissionPermission
        {
            public const string View = "Permissions.Permission.View";
            public const string Create = "Permissions.Permission.Create";
            public const string Edit = "Permissions.Permission.Edit";
            public const string Delete = "Permissions.Permission.Delete";
        }
        public static class CategoryPermission
        {
            public const string View = "Permissions.Category.View";
            public const string Create = "Permissions.Category.Create";
            public const string Edit = "Permissions.Category.Edit";
            public const string Delete = "Permissions.Category.Delete";
        }
        public static class DepartmentPermission
        {
            public const string View = "Permissions.Department.View";
            public const string Create = "Permissions.Department.Create";
            public const string Edit = "Permissions.Department.Edit";
            public const string Delete = "Permissions.Department.Delete";
        }
        public static class RolePermission
        {
            public const string View = "Permissions.Role.View";
            public const string Create = "Permissions.Role.Create";
            public const string Edit = "Permissions.Role.Edit";
            public const string Delete = "Permissions.Role.Delete";
        }
        public static class SubCategoryPermission
        {
            public const string View = "Permissions.SubCategory.View";
            public const string Create = "Permissions.SubCategory.Create";
            public const string Edit = "Permissions.SubCategory.Edit";
            public const string Delete = "Permissions.SubCategory.Delete";
        }
        public static class UserPermission
        {
            public const string View = "Permissions.User.View";
            public const string Create = "Permissions.User.Create";
            public const string Edit = "Permissions.User.Edit";
            public const string Delete = "Permissions.User.Delete";
            public const string ResetPassword = "Permissions.User.ResetPassword";
            public const string Profile = "Permissions.User.Profile";
            public const string Register = "Permissions.User.Register";
        }
        public static class AuditPermission
        {
            public const string View = "Permissions.Audit.View";
        }
        public static class SubMenuPermission
        {
            public const string View = "Permissions.SubMenu.View";
            public const string Create = "Permissions.SubMenu.Create";
            public const string Edit = "Permissions.SubMenu.Edit";
            public const string Delete = "Permissions.SubMenu.Delete";
        }
        public static class MenuPermission
        {
            public const string View = "Permissions.Menu.View";
            public const string Create = "Permissions.Menu.Create";
            public const string Edit = "Permissions.Menu.Edit";
            public const string Delete = "Permissions.Menu.Delete";
        }
    }
}
