using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class StaffDataMigration : Migration
    {
        private string staffPermission = Guid.NewGuid().ToString();
        private string createEmployeeMonthPermission = Guid.NewGuid().ToString();
        private string viewEmployeeMonthPermission = Guid.NewGuid().ToString();
        private string staffMenu = Guid.NewGuid().ToString();
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "LocationId",
                table: "Staffs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            SeedEntities(migrationBuilder);

            SeedPermission(migrationBuilder);

            SeedMenu(migrationBuilder);

            SeedSubMenu(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "LocationId",
                table: "Staffs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        private void SeedPermission(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{staffPermission}', NEWID(), 'Permissions.Staff.Create'),
                ('{staffPermission}', NEWID(), 'Permissions.Staff.View'),
                ('{staffPermission}', NEWID(), 'Permissions.Staff.Delete');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{createEmployeeMonthPermission}', NEWID(), 'Permissions.CreateEmployee.CurrentMonth'),
                ('{createEmployeeMonthPermission}', NEWID(), 'Permissions.CreateEmployee.PreviousMonth');");

            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Permission] ([EntityId], [Id], [value]) VALUES 
                ('{viewEmployeeMonthPermission}', NEWID(), 'Permissions.ViewEmployee.CurrentMonth'),
                ('{viewEmployeeMonthPermission}', NEWID(), 'Permissions.ViewEmployee.PreviousMonth');");
        }

        private void SeedEntities(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Entity] ([EntityId], [Name]) VALUES 
                ('{staffPermission}', 'Staff'),
                ('{createEmployeeMonthPermission}', 'CreateEmployee'),
                ('{viewEmployeeMonthPermission}', 'ViewEmployee');");
        }

        private void SeedMenu(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Menu] (MenuId, DisplayName, sort) VALUES 
                ('{staffMenu}', 'Staff', 11);");
        }

        private void SeedSubMenu(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[SubMenu] (SubMenuId, DisplayName, Controller, Method, MenuId) VALUES 
                    (NEWID(), 'Manage Staff', 'Staff', 'Index', '{staffMenu}');");
        }
    }
}
