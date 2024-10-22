using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class booltoreminders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ada9a56-87ff-4df0-906c-f392b1ec3149");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c076dcae-8c29-4bc1-b7c6-88b5fed94415");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcbac89a-e8b1-4d68-a490-5584e1ba475c");

            migrationBuilder.DropColumn(
                name: "HasSent",
                table: "Invitations");

            migrationBuilder.AddColumn<bool>(
                name: "HasSent",
                table: "Reminders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7c8f7705-d510-4428-a571-a87d01d3a591", null, "Admin", "ADMIN" },
                    { "ca5e5e82-58c7-46e3-8846-e22866d8dc74", null, "Organizer", "ORGANIZER" },
                    { "e506c86c-4d72-4a4a-9a35-841981f7dada", null, "Attendee", "ATTENDEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c8f7705-d510-4428-a571-a87d01d3a591");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca5e5e82-58c7-46e3-8846-e22866d8dc74");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e506c86c-4d72-4a4a-9a35-841981f7dada");

            migrationBuilder.DropColumn(
                name: "HasSent",
                table: "Reminders");

            migrationBuilder.AddColumn<bool>(
                name: "HasSent",
                table: "Invitations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8ada9a56-87ff-4df0-906c-f392b1ec3149", null, "Attendee", "ATTENDEE" },
                    { "c076dcae-8c29-4bc1-b7c6-88b5fed94415", null, "Admin", "ADMIN" },
                    { "dcbac89a-e8b1-4d68-a490-5584e1ba475c", null, "Organizer", "ORGANIZER" }
                });
        }
    }
}
