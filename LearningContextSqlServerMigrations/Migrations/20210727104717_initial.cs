using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LearningContextSqlServerMigrations.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Chapter = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ExamRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestQuestionMarking = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFinish = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mark = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    TestItemId = table.Column<int>(type: "int", nullable: false),
                    PersonintId = table.Column<int>(name: "Person<int>Id", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tests_Person_Person<int>Id",
                        column: x => x.PersonintId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QUestionSqls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mark = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AnswerType = table.Column<int>(type: "int", nullable: false),
                    AnswerExplain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestItemQUestionSqlintId = table.Column<int>(name: "TestItem<QUestionSql, int>Id", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QUestionSqls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QUestionSqls_TestItems_TestItem<QUestionSql, int>Id",
                        column: x => x.TestItemQUestionSqlintId,
                        principalTable: "TestItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QUestionSqlId = table.Column<int>(type: "int", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    IsAnswered = table.Column<bool>(type: "bit", nullable: false),
                    IsMarked = table.Column<bool>(type: "bit", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestQUestionSqlintId = table.Column<int>(name: "Test<QUestionSql, int>Id", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_QUestionSqls_QUestionSqlId",
                        column: x => x.QUestionSqlId,
                        principalTable: "QUestionSqls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_Tests_Test<QUestionSql, int>Id",
                        column: x => x.TestQUestionSqlintId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTrue = table.Column<bool>(type: "bit", nullable: false),
                    QUestionSqlId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionOptions_QUestionSqls_QUestionSqlId",
                        column: x => x.QUestionSqlId,
                        principalTable: "QUestionSqls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Supplements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RotateContent = table.Column<int>(type: "int", nullable: false),
                    OriginalContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalcontentType = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    QUestionSqlId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplements_QUestionSqls_QUestionSqlId",
                        column: x => x.QUestionSqlId,
                        principalTable: "QUestionSqls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnswareOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTrue = table.Column<bool>(type: "bit", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false),
                    AnswerintId = table.Column<int>(name: "Answer<int>Id", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswareOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswareOptions_Answers_Answer<int>Id",
                        column: x => x.AnswerintId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Address", "Email", "IdNumber", "Name", "Password", "Phone" },
                values: new object[] { 1, "Gilon", "Yos@gmail.com", "135792468", "yosef Levy", "12345@12345", "+97249984222" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Address", "Email", "IdNumber", "Name", "Password", "Phone" },
                values: new object[] { 2, "Gilon", "Yoni@gmail.com", "246813579", "Yoni Levy", "12345@12345", "+97249984220" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Address", "Email", "IdNumber", "Name", "Password", "Phone" },
                values: new object[] { 4, "Gilon", "Tal@gmail.com", "1502626", "Tal Levy", "12345@12345", "+97249984226" });

            migrationBuilder.CreateIndex(
                name: "IX_AnswareOptions_Answer<int>Id",
                table: "AnswareOptions",
                column: "Answer<int>Id");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QUestionSqlId",
                table: "Answers",
                column: "QUestionSqlId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Test<QUestionSql, int>Id",
                table: "Answers",
                column: "Test<QUestionSql, int>Id");

            migrationBuilder.CreateIndex(
                name: "IX_Person_IdNumber",
                table: "Person",
                column: "IdNumber",
                unique: true,
                filter: "[IdNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QUestionSqlId",
                table: "QuestionOptions",
                column: "QUestionSqlId");

            migrationBuilder.CreateIndex(
                name: "IX_QUestionSqls_TestItem<QUestionSql, int>Id",
                table: "QUestionSqls",
                column: "TestItem<QUestionSql, int>Id");

            migrationBuilder.CreateIndex(
                name: "IX_Supplements_QUestionSqlId",
                table: "Supplements",
                column: "QUestionSqlId");

            migrationBuilder.CreateIndex(
                name: "IX_TestItems_Category_Chapter_Subject_Version",
                table: "TestItems",
                columns: new[] { "Category", "Chapter", "Subject", "Version" },
                unique: true,
                filter: "[Category] IS NOT NULL AND [Chapter] IS NOT NULL AND [Subject] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_Person<int>Id",
                table: "Tests",
                column: "Person<int>Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswareOptions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "Supplements");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "QUestionSqls");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "TestItems");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
