using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class booltoevent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "HasPayment",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInvitationOnly",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "HasPayment",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsInvitationOnly",
                table: "Events");

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
    }
}
