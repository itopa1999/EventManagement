using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class BlockBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09c6f900-be35-495d-8408-1db9d6c985c6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f1a0a19-498b-4168-9595-8c7e091be687");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d7f34907-b481-4283-9719-534c99ad8be0");

            migrationBuilder.AddColumn<bool>(
                name: "IsBlock",
                table: "Wallets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlock",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlock",
                table: "Attendees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlock",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "93c8b955-d7c5-4af4-9ecc-76de944f2f8b", null, "Organizer", "ORGANIZER" },
                    { "b6ab04cc-44d3-45a8-91af-579fb78bdc7a", null, "Admin", "ADMIN" },
                    { "d6053f05-6bff-4346-a56b-c9a5b302ac6a", null, "Attendee", "ATTENDEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93c8b955-d7c5-4af4-9ecc-76de944f2f8b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6ab04cc-44d3-45a8-91af-579fb78bdc7a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d6053f05-6bff-4346-a56b-c9a5b302ac6a");

            migrationBuilder.DropColumn(
                name: "IsBlock",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "IsBlock",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsBlock",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "IsBlock",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09c6f900-be35-495d-8408-1db9d6c985c6", null, "Admin", "ADMIN" },
                    { "8f1a0a19-498b-4168-9595-8c7e091be687", null, "Attendee", "ATTENDEE" },
                    { "d7f34907-b481-4283-9719-534c99ad8be0", null, "Organizer", "ORGANIZER" }
                });
        }
    }
}
