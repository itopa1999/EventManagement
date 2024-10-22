using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class booltoreminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bee889c7-ef98-4c2e-adba-91f565a55c22");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f09e2248-ef84-408c-b7d5-b27afa3245eb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f278d014-2fdb-4901-b098-4c1fa89407ae");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "bee889c7-ef98-4c2e-adba-91f565a55c22", null, "Admin", "ADMIN" },
                    { "f09e2248-ef84-408c-b7d5-b27afa3245eb", null, "Organizer", "ORGANIZER" },
                    { "f278d014-2fdb-4901-b098-4c1fa89407ae", null, "Attendee", "ATTENDEE" }
                });
        }
    }
}
