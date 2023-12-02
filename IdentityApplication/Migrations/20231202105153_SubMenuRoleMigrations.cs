using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class SubMenuRoleMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubMenuRole_AspNetRoles_Id",
                table: "SubMenuRole");

            migrationBuilder.DropForeignKey(
                name: "FK_SubMenuRole_SubMenu_SubMenuId",
                table: "SubMenuRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubMenuRole",
                table: "SubMenuRole");

            migrationBuilder.DropIndex(
                name: "IX_SubMenuRole_Id",
                table: "SubMenuRole");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "SubMenuRole",
                newName: "SubMenuRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubMenuRoles",
                table: "SubMenuRoles",
                columns: new[] { "SubMenuId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_SubMenuRoles_SubMenu_SubMenuId",
                table: "SubMenuRoles",
                column: "SubMenuId",
                principalTable: "SubMenu",
                principalColumn: "SubMenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubMenuRoles_SubMenu_SubMenuId",
                table: "SubMenuRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubMenuRoles",
                table: "SubMenuRoles");

            migrationBuilder.RenameTable(
                name: "SubMenuRoles",
                newName: "SubMenuRole");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubMenuRole",
                table: "SubMenuRole",
                columns: new[] { "SubMenuId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_SubMenuRole_Id",
                table: "SubMenuRole",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubMenuRole_AspNetRoles_Id",
                table: "SubMenuRole",
                column: "Id",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubMenuRole_SubMenu_SubMenuId",
                table: "SubMenuRole",
                column: "SubMenuId",
                principalTable: "SubMenu",
                principalColumn: "SubMenuId");
        }
    }
}
