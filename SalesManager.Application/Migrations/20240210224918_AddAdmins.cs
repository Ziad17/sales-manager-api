using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SalesManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                schema: "SalesManager",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                schema: "SalesManager",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Admins",
                schema: "SalesManager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSuperAdmin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_Users_Id",
                        column: x => x.Id,
                        principalSchema: "SalesManager",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "SalesManager",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("60091d58-d6bd-471e-8d7a-5fb7afd84385"), null, "Admin", null },
                    { new Guid("a55eef24-3edb-41a0-8329-1462dee9aeb9"), null, "Driver", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins",
                schema: "SalesManager");

            migrationBuilder.DeleteData(
                schema: "SalesManager",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("60091d58-d6bd-471e-8d7a-5fb7afd84385"));

            migrationBuilder.DeleteData(
                schema: "SalesManager",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a55eef24-3edb-41a0-8329-1462dee9aeb9"));

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                schema: "SalesManager",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                schema: "SalesManager",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
