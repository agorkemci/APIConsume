using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToDoApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDone = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Description", "DueDate", "Priority", "Title" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "Configure GitHub Actions workflow for automated build, test, and container deployment.", new DateTime(2026, 9, 24, 0, 0, 0, 0, DateTimeKind.Local), 2, "Set up CI/CD pipeline" });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Description", "DueDate", "IsDone", "Priority", "Title" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), "Add token-based authorization with refresh token rotation and role-based policies.", new DateTime(2026, 9, 21, 0, 0, 0, 0, DateTimeKind.Local), true, 2, "Implement JWT authentication" });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Description", "DueDate", "Priority", "Title" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Cover CRUD operations and query specifications using xUnit and Moq.", new DateTime(2026, 9, 27, 0, 0, 0, 0, DateTimeKind.Local), 1, "Write unit tests for repository layer" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Inspect SQL execution plans, add missing indexes, and apply AsNoTracking where appropriate.", new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Local), 1, "Optimize EF Core queries" }
                });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Description", "DueDate", "Title" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), "Document all public endpoints with request/response schemas using Swagger/OpenAPI.", new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Local), "Update API documentation" });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_DueDate",
                table: "Todos",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_IsDone",
                table: "Todos",
                column: "IsDone");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_Priority",
                table: "Todos",
                column: "Priority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
