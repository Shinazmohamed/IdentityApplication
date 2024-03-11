using Microsoft.EntityFrameworkCore.Migrations;
using NuGet.Configuration;

#nullable disable

namespace IdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        private string UserRoleId = Guid.NewGuid().ToString();
        private string AdminRoleId = Guid.NewGuid().ToString();
        private string SuperDevRoleId = Guid.NewGuid().ToString();

        private string AdminId = Guid.NewGuid().ToString();
        private string UserId = Guid.NewGuid().ToString();
        private string SuperDevId = Guid.NewGuid().ToString();

        private string LocationId = Guid.NewGuid().ToString();

        private string employeeMenu = Guid.NewGuid().ToString();
        private string userMenu = Guid.NewGuid().ToString();
        private string departmentMenu = Guid.NewGuid().ToString();
        private string categoryMenu = Guid.NewGuid().ToString();
        private string subcategoryMenu = Guid.NewGuid().ToString();
        private string menuMenu = Guid.NewGuid().ToString();
        private string auditMenu = Guid.NewGuid().ToString();
        private string roleMenu = Guid.NewGuid().ToString();
        private string permissionMenu = Guid.NewGuid().ToString();
        private string settingsMenu = Guid.NewGuid().ToString();

        private string createEmployeeSubMenu = Guid.NewGuid().ToString();
        private string listEmployeeSubMenu = Guid.NewGuid().ToString();
        private string profileUserSubMenu = Guid.NewGuid().ToString();

        private string employeePermission = Guid.NewGuid().ToString();
        private string entityPermission = Guid.NewGuid().ToString();
        private string permissionPermission = Guid.NewGuid().ToString();
        private string categoryPermission = Guid.NewGuid().ToString();
        private string departmentPermission = Guid.NewGuid().ToString();
        private string rolePermission = Guid.NewGuid().ToString();
        private string subcategoryPermission = Guid.NewGuid().ToString();
        private string userPermission = Guid.NewGuid().ToString();
        private string auditPermission = Guid.NewGuid().ToString();
        private string subMenuPermission = Guid.NewGuid().ToString();
        private string menuPermission = Guid.NewGuid().ToString();

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SeedLocations(migrationBuilder);
            SeedRolesSQL(migrationBuilder);
            SeedUser(migrationBuilder);
            SeedUserRoles(migrationBuilder);
            SeedMenu(migrationBuilder);
            SeedSubMenu(migrationBuilder);
            SeedSubMenuRole(migrationBuilder);
            SeedEntities(migrationBuilder);
            SeedEntityPermissions(migrationBuilder);
        }

        private void SeedLocations(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Location] VALUES
                ('{LocationId}', 'HQ');");
        }
        private void SeedRolesSQL(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"INSERT INTO [dbo].[Role] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                VALUES ('{AdminRoleId}', 'Administrator', 'ADMINISTRATOR', null);");

            migrationBuilder.Sql($@"INSERT INTO [dbo].[Role] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                VALUES ('{UserRoleId}', 'User', 'USER', null);");

            migrationBuilder.Sql($@"INSERT INTO [dbo].[Role] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                VALUES ('{SuperDevRoleId}', 'SuperDeveloper', 'SUPERDEVELOPER', null);");
        }
        private void SeedUser(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[User] ([Id], [UserName], [LocationId], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
                VALUES ('{AdminId}', 'admin@test.com', '{LocationId}', 'ADMIN@TEST.COM', 'admin@test.com', 'ADMIN@TEST.COM', 0, 'AQAAAAIAAYagAAAAEAIoVtgbc8xCgaF/0Uor35PW8MmYnEIjPJLPBKQlW/1Q0YZGQnsGru3FZrws9lv9Bg==', 'YUPAFWNGZI2UC5FOITC7PX5J7XZTAZAA', '9337b27a-86df-425c-a68b-10e97e15d4ae', NULL, 0, 0, NULL, 1, 0)");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[User] ([Id], [UserName], [LocationId], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
                VALUES ('{UserId}', 'user@test.com', '{LocationId}', 'USER@TEST.COM', 'user@test.com', 'USER@TEST.COM', 0, 'AQAAAAIAAYagAAAAEAIoVtgbc8xCgaF/0Uor35PW8MmYnEIjPJLPBKQlW/1Q0YZGQnsGru3FZrws9lv9Bg==', 'YUPAFWNGZI2UC5FOITC7PX5J7XZTAZAA', '751920ce-2459-4f57-95de-2bf30c1205f5', NULL, 0, 0, NULL, 1, 0)");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[User] ([Id], [UserName], [LocationId], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
                VALUES ('{SuperDevId}', 'superdev@test.com', '{LocationId}', 'SUPERDEV@TEST.COM', 'superdev@test.com', 'SUPERDEV@TEST.COM', 0, 'AQAAAAIAAYagAAAAEAIoVtgbc8xCgaF/0Uor35PW8MmYnEIjPJLPBKQlW/1Q0YZGQnsGru3FZrws9lv9Bg==', 'YUPAFWNGZI2UC5FOITC7PX5J7XZTAZAA', '751920ce-2459-4f57-95de-2bf30c1205f5', NULL, 0, 0, NULL, 0, 0)");

        }
        private void SeedUserRoles(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId])
                VALUES ('{UserId}', '{UserRoleId}');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId])
                VALUES ('{AdminId}', '{AdminRoleId}');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId])
                VALUES ('{SuperDevId}', '{SuperDevRoleId}');");
        }
        private void SeedMenu(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Menu] (MenuId, DisplayName, sort) VALUES 
                ('{employeeMenu}', 'Employee', 1),  
                ('{userMenu}', 'User', 2),
                ('{departmentMenu}', 'Department', 3),
                ('{categoryMenu}', 'Category', 4),
                ('{subcategoryMenu}', 'Sub Category', 5),
                ('{auditMenu}', 'Audit', 6),
                ('{roleMenu}', 'Roles', 7),
                ('{permissionMenu}', 'Permission', 8),
                ('{menuMenu}', 'Menu', 9),
                ('{settingsMenu}', 'Settings', 99);");
        }
        private void SeedSubMenu(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[SubMenu] (SubMenuId, DisplayName, Controller, Method, MenuId) VALUES 
                    ('{createEmployeeSubMenu}', 'Create Employee', 'Employee', 'Create', '{employeeMenu}'),
                    ('{listEmployeeSubMenu}', 'List Employee', 'Employee', 'List', '{employeeMenu}'),
                    (NEWID(), 'Register', 'User', 'Register', '{userMenu}'),
                    (NEWID(), 'List', 'User', 'Index', '{userMenu}'),
                    ('{profileUserSubMenu}', 'Profile', 'User', 'Profile', '{userMenu}'),
                    (NEWID(), 'Create', 'Category', 'Index', '{categoryMenu}'),
                    (NEWID(), 'Mapping', 'CategoryDepartmentMapping', 'Index', '{categoryMenu}'),
                    (NEWID(), 'Create', 'SubCategory', 'Index', '{subcategoryMenu}'),
                    (NEWID(), 'Create menu', 'SubMenu', 'Index', '{menuMenu}'),
                    (NEWID(), 'View logs', 'Audit', 'Index', '{auditMenu}'),
                    (NEWID(), 'Create', 'Department', 'Index', '{departmentMenu}'),
                    (NEWID(), 'View', 'Roles', 'Index', '{roleMenu}'),
                    (NEWID(), 'View', 'Permission', 'Index', '{permissionMenu}'),
                    (NEWID(), 'Logout', 'User', 'Logout', '{settingsMenu}'),
                    (NEWID(), 'Mapping', 'CategorySubCategoryMapping', 'Index', '{subcategoryMenu}');");
        }
        private void SeedSubMenuRole(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[SubMenuRoles] (SubMenuId, Id) VALUES 
                    ('{createEmployeeSubMenu}','{UserRoleId}'),
                    ('{listEmployeeSubMenu}','{UserRoleId}');");
        }
        private void SeedEntities(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Entity] ([EntityId], [Name]) VALUES 
                ('{employeePermission}', 'Employee'),
                ('{entityPermission}', 'Entity'),
                ('{permissionPermission}', 'Permission'),
                ('{categoryPermission}', 'Category'),
                ('{departmentPermission}', 'Department'),
                ('{rolePermission}', 'Role'),
                ('{subcategoryPermission}', 'Sub Category'),
                ('{userPermission}', 'User'),
                ('{auditPermission}', 'Audit'),
                ('{subMenuPermission}', 'Sub Menu'),
                ('{menuPermission}', 'Menu');");
        }
        private void SeedEntityPermissions(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{employeePermission}', NEWID(), 'Permissions.Employee.Create'),
                ('{employeePermission}', NEWID(), 'Permissions.Employee.View'),
                ('{employeePermission}', NEWID(), 'Permissions.Employee.Edit'),
                ('{employeePermission}', NEWID(), 'Permissions.Employee.Delete');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{entityPermission}', NEWID(), 'Permissions.Entity.Create'),
                ('{entityPermission}', NEWID(), 'Permissions.Entity.View'),
                ('{entityPermission}', NEWID(), 'Permissions.Entity.Edit'),
                ('{entityPermission}', NEWID(), 'Permissions.Entity.Delete');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{permissionPermission}', NEWID(), 'Permissions.Permission.Create'),
                ('{permissionPermission}', NEWID(), 'Permissions.Permission.View'),
                ('{permissionPermission}', NEWID(), 'Permissions.Permission.Edit'),
                ('{permissionPermission}', NEWID(), 'Permissions.Permission.Delete');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{categoryPermission}', NEWID(), 'Permissions.Category.Create'),
                ('{categoryPermission}', NEWID(), 'Permissions.Category.View'),
                ('{categoryPermission}', NEWID(), 'Permissions.Category.Edit'),
                ('{categoryPermission}', NEWID(), 'Permissions.Category.Delete');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{departmentPermission}', NEWID(), 'Permissions.Department.Create'),
                ('{departmentPermission}', NEWID(), 'Permissions.Department.View'),
                ('{departmentPermission}', NEWID(), 'Permissions.Department.Edit'),
                ('{departmentPermission}', NEWID(), 'Permissions.Department.Delete');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{rolePermission}', NEWID(), 'Permissions.Role.Create'),
                ('{rolePermission}', NEWID(), 'Permissions.Role.View'),
                ('{rolePermission}', NEWID(), 'Permissions.Role.Edit'),
                ('{rolePermission}', NEWID(), 'Permissions.Role.Delete');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{subcategoryPermission}', NEWID(), 'Permissions.SubCategory.Create'),
                ('{subcategoryPermission}', NEWID(), 'Permissions.SubCategory.View'),
                ('{subcategoryPermission}', NEWID(), 'Permissions.SubCategory.Edit'),
                ('{subcategoryPermission}', NEWID(), 'Permissions.SubCategory.Delete');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{userPermission}', NEWID(), 'Permissions.User.Create'),
                ('{userPermission}', NEWID(), 'Permissions.User.View'),
                ('{userPermission}', NEWID(), 'Permissions.User.Edit'),
                ('{userPermission}', NEWID(), 'Permissions.User.Delete'),
                ('{userPermission}', NEWID(), 'Permissions.User.ResetPassword'),
                ('{userPermission}', NEWID(), 'Permissions.User.Profile'),
                ('{userPermission}', NEWID(), 'Permissions.User.Register');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{auditPermission}', NEWID(), 'Permissions.Audit.View');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{subMenuPermission}', NEWID(), 'Permissions.SubMenu.Create'),
                ('{subMenuPermission}', NEWID(), 'Permissions.SubMenu.View'),
                ('{subMenuPermission}', NEWID(), 'Permissions.SubMenu.Edit'),
                ('{subMenuPermission}', NEWID(), 'Permissions.SubMenu.Delete');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{menuPermission}', NEWID(), 'Permissions.Menu.Create'),
                ('{menuPermission}', NEWID(), 'Permissions.Menu.View'),
                ('{menuPermission}', NEWID(), 'Permissions.Menu.Edit'),
                ('{menuPermission}', NEWID(), 'Permissions.Menu.Delete');");
        }
    }
}
