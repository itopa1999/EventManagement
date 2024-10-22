using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class state : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "48a952c5-e0f9-4638-8e38-adc6ec0d2f4f", null, "Admin", "ADMIN" },
                    { "e9c2c398-4d19-49d2-a709-24681b0d2a2a", null, "Organizer", "ORGANIZER" },
                    { "eedec756-2bb8-457e-9bc9-c329a8ada1d7", null, "Attendee", "ATTENDEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48a952c5-e0f9-4638-8e38-adc6ec0d2f4f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9c2c398-4d19-49d2-a709-24681b0d2a2a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eedec756-2bb8-457e-9bc9-c329a8ada1d7");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Events");

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
    }
}
