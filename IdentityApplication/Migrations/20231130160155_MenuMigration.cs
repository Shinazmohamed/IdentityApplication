using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class MenuMigration : Migration
    {
        /// <inheritdoc />
        /// 
        private string employeeMenu = Guid.NewGuid().ToString();
        private string userMenu = Guid.NewGuid().ToString();
        private string categoryMenu = Guid.NewGuid().ToString();
        private string subcategoryMenu = Guid.NewGuid().ToString();
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.MenuId);
                });

            migrationBuilder.CreateTable(
                name: "SubMenu",
                columns: table => new
                {
                    SubMenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubMenu", x => x.SubMenuId);
                    table.ForeignKey(
                        name: "FK_SubMenu_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "MenuId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubMenu_MenuId",
                table: "SubMenu",
                column: "MenuId");

            SeedMenu(migrationBuilder);
            SeedSubMenu(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubMenu");

            migrationBuilder.DropTable(
                name: "Menu");
        }

        private void SeedMenu(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [Menu] (MenuId, DisplayName) VALUES 
                ('{employeeMenu}', 'Employee'),  
                ('{userMenu}', 'User'),
                ('{categoryMenu}', 'Category'),
                ('{subcategoryMenu}', 'Sub Category');");
        }
        private void SeedSubMenu(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [SubMenu] (SubMenuId, DisplayName, Controller, Method, MenuId) VALUES 
                    (NEWID(), 'Create Employee', 'Employee', 'Create', '{employeeMenu}'),
                    (NEWID(), 'List Employee', 'Employee', 'List', '{employeeMenu}'),
                    (NEWID(), 'Register', 'User', 'Register', '{userMenu}'),
                    (NEWID(), 'List', 'User', 'Index', '{userMenu}'),
                    (NEWID(), 'Profile', 'User', 'Profile', '{userMenu}'),
                    (NEWID(), 'Create', 'Category', 'Index', '{categoryMenu}'),
                    (NEWID(), 'Create', 'SubCategory', 'Index', '{subcategoryMenu}'),
                    (NEWID(), 'Mapping', 'CategorySubCategoryMapping', 'Index', '{subcategoryMenu}');");
        }
    }
}
