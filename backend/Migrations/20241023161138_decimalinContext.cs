using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class decimalinContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "30f4de15-73e5-470a-89c6-b3a0101a0c89");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95e7a324-d99f-4c1c-80bd-08e7d4bac90e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb2c27b3-e4c9-480f-9938-36037c45a967");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "06ac76da-40e7-4899-9e35-9a8ae7edf8c1", null, "Attendee", "ATTENDEE" },
                    { "1bd6151d-84d3-4f2c-a81d-0bb08bd32603", null, "Organizer", "ORGANIZER" },
                    { "cf2f13c4-3421-4f1f-b744-f2a172cf4568", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06ac76da-40e7-4899-9e35-9a8ae7edf8c1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1bd6151d-84d3-4f2c-a81d-0bb08bd32603");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf2f13c4-3421-4f1f-b744-f2a172cf4568");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "30f4de15-73e5-470a-89c6-b3a0101a0c89", null, "Attendee", "ATTENDEE" },
                    { "95e7a324-d99f-4c1c-80bd-08e7d4bac90e", null, "Organizer", "ORGANIZER" },
                    { "fb2c27b3-e4c9-480f-9938-36037c45a967", null, "Admin", "ADMIN" }
                });
        }
    }
}
