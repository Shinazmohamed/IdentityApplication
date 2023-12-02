using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class SubMenuRoleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SPNOV2023",
                table: "SPNOV2023");

            migrationBuilder.RenameTable(
                name: "SPNOV2023",
                newName: "SPDec2023");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SPDec2023",
                table: "SPDec2023",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SubMenuRole",
                columns: table => new
                {
                    SubMenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubMenuRole", x => new { x.SubMenuId, x.Id });
                    table.ForeignKey(
                        name: "FK_SubMenuRole_AspNetRoles_Id",
                        column: x => x.Id,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubMenuRole_SubMenu_SubMenuId",
                        column: x => x.SubMenuId,
                        principalTable: "SubMenu",
                        principalColumn: "SubMenuId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubMenuRole_Id",
                table: "SubMenuRole",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubMenuRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SPDec2023",
                table: "SPDec2023");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "SPDec2023",
                newName: "SPNOV2023");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SPNOV2023",
                table: "SPNOV2023",
                column: "Id");
        }
    }
}
