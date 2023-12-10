using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class PermissionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Entity = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            SeedPermissions(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permission",
                schema: "Identity");
        }


        private void SeedPermissions(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [identity].[Permission] (Id, Value, Entity) VALUES 
                (NEWID(), 'Permissions.Employees.View', 'Employee'),
                (NEWID(), 'Permissions.Employees.Create', 'Employee'),
                (NEWID(), 'Permissions.Employees.Edit', 'Employee'),
                (NEWID(), 'Permissions.Employees.Delete', 'Employee');");
        }
    }
}
