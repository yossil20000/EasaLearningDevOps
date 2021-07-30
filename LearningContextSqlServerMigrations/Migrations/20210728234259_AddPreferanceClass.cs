using Microsoft.EntityFrameworkCore.Migrations;

namespace LearningContextSqlServerMigrations.Migrations
{
    public partial class AddPreferanceClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreferanceId",
                table: "Person",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Preferance<int>",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Theme = table.Column<int>(type: "int", nullable: false),
                    HUE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferance<int>", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Person_PreferanceId",
                table: "Person",
                column: "PreferanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Preferance<int>_PreferanceId",
                table: "Person",
                column: "PreferanceId",
                principalTable: "Preferance<int>",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Preferance<int>_PreferanceId",
                table: "Person");

            migrationBuilder.DropTable(
                name: "Preferance<int>");

            migrationBuilder.DropIndex(
                name: "IX_Person_PreferanceId",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "PreferanceId",
                table: "Person");
        }
    }
}
