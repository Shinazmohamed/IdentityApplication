using System;
using Microsoft.EntityFrameworkCore.Migrations;
using static IdentityApplication.Core.PermissionHelper.PermissionsModel;

#nullable disable

namespace IdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class TeamMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "Staffs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_TeamId",
                table: "Staffs",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Teams_TeamId",
                table: "Staffs",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.SetNull);

            SeedTeams(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Teams_TeamId",
                table: "Staffs");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_TeamId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Staffs");
        }


        private void SeedTeams(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [dbo].[Teams] ([TeamId], [TeamName]) VALUES 
                (NEWID(), 'Sales Team'),
                (NEWID(), 'Fit_On Team'),
                (NEWID(), 'Alteration Team');");
        }
    }
}
