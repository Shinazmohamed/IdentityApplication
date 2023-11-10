using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SPOct2023",
                table: "SPOct2023");

            migrationBuilder.RenameTable(
                name: "SPOct2023",
                newName: "SPDec2023");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SPDec2023",
                table: "SPDec2023",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SPDec2023",
                table: "SPDec2023");

            migrationBuilder.RenameTable(
                name: "SPDec2023",
                newName: "SPOct2023");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SPOct2023",
                table: "SPOct2023",
                column: "Id");
        }
    }
}
