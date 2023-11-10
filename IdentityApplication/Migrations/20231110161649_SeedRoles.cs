using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityApplication.Migrations
{
    public partial class SeedRoles : Migration
    {
        private string UserRoleId = Guid.NewGuid().ToString();
        private string AdminRoleId = Guid.NewGuid().ToString();
        private string AdminId = Guid.NewGuid().ToString();
        private string UserId = Guid.NewGuid().ToString();

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SeedRolesSQL(migrationBuilder);
            SeedUser(migrationBuilder);
            SeedUserRoles(migrationBuilder);
        }

        private void SeedRolesSQL(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                VALUES ('{AdminRoleId}', 'Administrator', 'ADMINISTRATOR', null);");

            migrationBuilder.Sql($@"INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                VALUES ('{UserRoleId}', 'User', 'USER', null);");
        }

        private void SeedUser(MigrationBuilder migrationBuilder)
        {
            var locationId = Guid.Parse("A3C602F9-CA4B-4810-855C-218A7A79A914");

            migrationBuilder.Sql($@"
                INSERT INTO [AspNetUsers] ([Id], [UserName], [LocationId], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
                VALUES ('{AdminId}', 'admin@test.com', '{locationId}', 'ADMIN@TEST.COM', 'admin@test.com', 'ADMIN@TEST.COM', 0, 'AQAAAAIAAYagAAAAEAIoVtgbc8xCgaF/0Uor35PW8MmYnEIjPJLPBKQlW/1Q0YZGQnsGru3FZrws9lv9Bg==', 'YUPAFWNGZI2UC5FOITC7PX5J7XZTAZAA', '9337b27a-86df-425c-a68b-10e97e15d4ae', NULL, 0, 0, NULL, 1, 0)");

            migrationBuilder.Sql($@"
                INSERT INTO [AspNetUsers] ([Id], [UserName], [LocationId], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
                VALUES ('{UserId}', 'user@test.com', '{locationId}', 'USER@TEST.COM', 'user@test.com', 'USER@TEST.COM', 0, 'AQAAAAIAAYagAAAAEAIoVtgbc8xCgaF/0Uor35PW8MmYnEIjPJLPBKQlW/1Q0YZGQnsGru3FZrws9lv9Bg==', 'YUPAFWNGZI2UC5FOITC7PX5J7XZTAZAA', '751920ce-2459-4f57-95de-2bf30c1205f5', NULL, 0, 0, NULL, 1, 0)");
        }

        private void SeedUserRoles(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
                VALUES ('{UserId}', '{UserRoleId}');");

            migrationBuilder.Sql($@"
                INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
                VALUES ('{AdminId}', '{AdminRoleId}');");
        }
    }
}
