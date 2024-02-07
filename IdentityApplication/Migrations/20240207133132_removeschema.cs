using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class removeschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserTokens",
                schema: "Identity",
                newName: "UserTokens");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "Identity",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                schema: "Identity",
                newName: "UserLogins");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "Identity",
                newName: "UserClaims");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "Identity",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "SubMenuRoles",
                schema: "Identity",
                newName: "SubMenuRoles");

            migrationBuilder.RenameTable(
                name: "SubMenu",
                schema: "Identity",
                newName: "SubMenu");

            migrationBuilder.RenameTable(
                name: "SubCategory",
                schema: "Identity",
                newName: "SubCategory");

            migrationBuilder.RenameTable(
                name: "SP_Table",
                schema: "Identity",
                newName: "SP_Table");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                schema: "Identity",
                newName: "RoleClaims");

            migrationBuilder.RenameTable(
                name: "Role",
                schema: "Identity",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "Permission",
                schema: "Identity",
                newName: "Permission");

            migrationBuilder.RenameTable(
                name: "Menu",
                schema: "Identity",
                newName: "Menu");

            migrationBuilder.RenameTable(
                name: "Location",
                schema: "Identity",
                newName: "Location");

            migrationBuilder.RenameTable(
                name: "Entity",
                schema: "Identity",
                newName: "Entity");

            migrationBuilder.RenameTable(
                name: "Department",
                schema: "Identity",
                newName: "Department");

            migrationBuilder.RenameTable(
                name: "Category",
                schema: "Identity",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "AuditLogs",
                schema: "Identity",
                newName: "AuditLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                newName: "UserTokens",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                newName: "UserLogins",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "UserClaims",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "User",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "SubMenuRoles",
                newName: "SubMenuRoles",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "SubMenu",
                newName: "SubMenu",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "SubCategory",
                newName: "SubCategory",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "SP_Table",
                newName: "SP_Table",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                newName: "RoleClaims",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Role",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "Permission",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "Menu",
                newName: "Menu",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "Location",
                newName: "Location",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "Entity",
                newName: "Entity",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Department",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Category",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "AuditLogs",
                newName: "AuditLogs",
                newSchema: "Identity");
        }
    }
}
