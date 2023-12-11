using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class EntityMigration : Migration
    {

        private string EmployeeEntityID = Guid.NewGuid().ToString();
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entity",
                schema: "Identity",
                table: "Permission");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                schema: "Identity",
                table: "Permission",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Entity",
                schema: "Identity",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity", x => x.EntityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_EntityId",
                schema: "Identity",
                table: "Permission",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Entity_EntityId",
                schema: "Identity",
                table: "Permission",
                column: "EntityId",
                principalSchema: "Identity",
                principalTable: "Entity",
                principalColumn: "EntityId",
                onDelete: ReferentialAction.Cascade);

            SeedEntity(migrationBuilder);
            SeedPermission(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Entity_EntityId",
                schema: "Identity",
                table: "Permission");

            migrationBuilder.DropTable(
                name: "Entity",
                schema: "Identity");

            migrationBuilder.DropIndex(
                name: "IX_Permission_EntityId",
                schema: "Identity",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "EntityId",
                schema: "Identity",
                table: "Permission");

            migrationBuilder.AddColumn<string>(
                name: "Entity",
                schema: "Identity",
                table: "Permission",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        private void SeedEntity(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [identity].[Entity] (EntityId, Name) VALUES 
                    ('{EmployeeEntityID}','Employee');");
        }

        private void SeedPermission(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO [identity].[Permission] (Id, Value, EntityId) VALUES 
                    (NEWID(), 'Permissions.Category.Edit', '{EmployeeEntityID}');");
        }
    }
}
