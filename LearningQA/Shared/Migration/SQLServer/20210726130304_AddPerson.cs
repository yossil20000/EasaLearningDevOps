using Microsoft.EntityFrameworkCore.Migrations;

namespace LearningQA.Shared.Migration.SQLServer
{
    public partial class AddPerson : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Address", "Email", "IdNumber", "Name", "Password", "Phone" },
                values: new object[] { 1, "Gilon", "Yos@gmail.com", "135792468", "yosef Levy", "12345@12345", "+97249984222" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Address", "Email", "IdNumber", "Name", "Password", "Phone" },
                values: new object[] { 2, "Gilon", "Yoni@gmail.com", "246813579", "Yoni Levy", "12345@12345", "+97249984220" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
