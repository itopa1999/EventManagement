using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12e31bec-de04-49e0-8b38-b9b26d04245e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "858b3586-9a45-46ae-ae2e-98e1f99562ff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0efc2a8-fdd0-4876-afb9-b02714274d62");

            migrationBuilder.CreateTable(
                name: "AccessToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessTok = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessToken", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "22d4e2e3-d3d2-4265-8c02-adacde364cc5", null, "Admin", "ADMIN" },
                    { "3989dd21-835c-4e1e-a4c9-6aad76a9a684", null, "Organizer", "ORGANIZER" },
                    { "a67f82ef-edec-4a8a-a0f6-e36d3be968e1", null, "Attendee", "ATTENDEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessToken");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22d4e2e3-d3d2-4265-8c02-adacde364cc5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3989dd21-835c-4e1e-a4c9-6aad76a9a684");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a67f82ef-edec-4a8a-a0f6-e36d3be968e1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "12e31bec-de04-49e0-8b38-b9b26d04245e", null, "Attendee", "ATTENDEE" },
                    { "858b3586-9a45-46ae-ae2e-98e1f99562ff", null, "Admin", "ADMIN" },
                    { "d0efc2a8-fdd0-4876-afb9-b02714274d62", null, "Organizer", "ORGANIZER" }
                });
        }
    }
}
