using Microsoft.EntityFrameworkCore.Migrations;

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

        private string createEmployeeSubMenu = Guid.NewGuid().ToString();
        private string listEmployeeSubMenu = Guid.NewGuid().ToString();
        private string profileUserSubMenu = Guid.NewGuid().ToString();
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SeedLocations(migrationBuilder);
            SeedRolesSQL(migrationBuilder);
            SeedUser(migrationBuilder);
            SeedUserRoles(migrationBuilder);
            SeedMenu(migrationBuilder);
            SeedSubMenu(migrationBuilder);
            SeedSubMenuRole(migrationBuilder);
        }

        private void SeedLocations(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [identity].[Location] VALUES
                ('{LocationId}', 'HQ');");
        }
        private void SeedRolesSQL(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"INSERT INTO [identity].[Role] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                VALUES ('{AdminRoleId}', 'Administrator', 'ADMINISTRATOR', null);");

            migrationBuilder.Sql($@"INSERT INTO [identity].[Role] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                VALUES ('{UserRoleId}', 'User', 'USER', null);");

            migrationBuilder.Sql($@"INSERT INTO [identity].[Role] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                VALUES ('{SuperDevRoleId}', 'SuperDeveloper', 'SUPERDEVELOPER', null);");
        }
        private void SeedUser(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [identity].[User] ([Id], [UserName], [LocationId], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
                VALUES ('{AdminId}', 'admin@test.com', '{LocationId}', 'ADMIN@TEST.COM', 'admin@test.com', 'ADMIN@TEST.COM', 0, 'AQAAAAIAAYagAAAAEAIoVtgbc8xCgaF/0Uor35PW8MmYnEIjPJLPBKQlW/1Q0YZGQnsGru3FZrws9lv9Bg==', 'YUPAFWNGZI2UC5FOITC7PX5J7XZTAZAA', '9337b27a-86df-425c-a68b-10e97e15d4ae', NULL, 0, 0, NULL, 1, 0)");

            migrationBuilder.Sql($@"
                INSERT INTO [identity].[User] ([Id], [UserName], [LocationId], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
                VALUES ('{UserId}', 'user@test.com', '{LocationId}', 'USER@TEST.COM', 'user@test.com', 'USER@TEST.COM', 0, 'AQAAAAIAAYagAAAAEAIoVtgbc8xCgaF/0Uor35PW8MmYnEIjPJLPBKQlW/1Q0YZGQnsGru3FZrws9lv9Bg==', 'YUPAFWNGZI2UC5FOITC7PX5J7XZTAZAA', '751920ce-2459-4f57-95de-2bf30c1205f5', NULL, 0, 0, NULL, 1, 0)");

            migrationBuilder.Sql($@"
                INSERT INTO [identity].[User] ([Id], [UserName], [LocationId], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
                VALUES ('{SuperDevId}', 'superdev@test.com', '{LocationId}', 'SUPERDEV@TEST.COM', 'superdev@test.com', 'SUPERDEV@TEST.COM', 0, 'AQAAAAIAAYagAAAAEAIoVtgbc8xCgaF/0Uor35PW8MmYnEIjPJLPBKQlW/1Q0YZGQnsGru3FZrws9lv9Bg==', 'YUPAFWNGZI2UC5FOITC7PX5J7XZTAZAA', '751920ce-2459-4f57-95de-2bf30c1205f5', NULL, 0, 0, NULL, 1, 0)");

        }
        private void SeedUserRoles(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [identity].[UserRoles] ([UserId], [RoleId])
                VALUES ('{UserId}', '{UserRoleId}');");

            migrationBuilder.Sql($@"
                INSERT INTO [identity].[UserRoles] ([UserId], [RoleId])
                VALUES ('{AdminId}', '{AdminRoleId}');");

            migrationBuilder.Sql($@"
                INSERT INTO [identity].[UserRoles] ([UserId], [RoleId])
                VALUES ('{SuperDevId}', '{SuperDevRoleId}');");
        }
        private void SeedMenu(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [identity].[Menu] (MenuId, DisplayName, sort) VALUES 
                ('{employeeMenu}', 'Employee', 1),  
                ('{userMenu}', 'User', 2),
                ('{departmentMenu}', 'Department', 3),
                ('{categoryMenu}', 'Category', 4),
                ('{subcategoryMenu}', 'Sub Category', 5),
                ('{auditMenu}', 'Audit', 6),
                ('{roleMenu}', 'Roles', 7),
                ('{permissionMenu}', 'Permission', 8),
                ('{menuMenu}', 'Menu', 9);");
        }
        private void SeedSubMenu(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [identity].[SubMenu] (SubMenuId, DisplayName, Controller, Method, MenuId) VALUES 
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
                    (NEWID(), 'Mapping', 'CategorySubCategoryMapping', 'Index', '{subcategoryMenu}');");
        }
        private void SeedSubMenuRole(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [identity].[SubMenuRoles] (SubMenuId, Id) VALUES 
                    ('{createEmployeeSubMenu}','{UserRoleId}'),
                    ('{listEmployeeSubMenu}','{UserRoleId}');");
        }
    }
}
