using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ticketsType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Events",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TicketType",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "016a5075-f3cb-44ce-89cf-73d1a10a4e49", null, "Organizer", "ORGANIZER" },
                    { "87f23869-7902-4a1c-b5b5-af71d247cea0", null, "Admin", "ADMIN" },
                    { "b54fdded-2108-4232-aa2b-8e5815d3d5a1", null, "Attendee", "ATTENDEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "016a5075-f3cb-44ce-89cf-73d1a10a4e49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87f23869-7902-4a1c-b5b5-af71d247cea0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b54fdded-2108-4232-aa2b-8e5815d3d5a1");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TicketType",
                table: "Events");

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
    }
}
