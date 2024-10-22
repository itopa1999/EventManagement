using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class eventype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5779b47f-6199-4ebc-8515-72f034daea0b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c25db6d3-46fc-4ff0-be0a-b9caa25c42d2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6ec5f06-6847-4ed9-924e-6eca913e1241");

            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "065e59c9-9c37-42f3-8123-714fafccaadc", null, "Organizer", "ORGANIZER" },
                    { "147e1437-b162-418a-8181-fe8d03cd75b8", null, "Attendee", "ATTENDEE" },
                    { "5abb3fa5-40d3-499f-a11c-027cec6fe2f3", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "065e59c9-9c37-42f3-8123-714fafccaadc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "147e1437-b162-418a-8181-fe8d03cd75b8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5abb3fa5-40d3-499f-a11c-027cec6fe2f3");

            migrationBuilder.DropColumn(
                name: "EventType",
                table: "Events");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5779b47f-6199-4ebc-8515-72f034daea0b", null, "Admin", "ADMIN" },
                    { "c25db6d3-46fc-4ff0-be0a-b9caa25c42d2", null, "Organizer", "ORGANIZER" },
                    { "e6ec5f06-6847-4ed9-924e-6eca913e1241", null, "Attendee", "ATTENDEE" }
                });
        }
    }
}
