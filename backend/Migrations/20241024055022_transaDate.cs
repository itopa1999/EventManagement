using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class transaDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "TicketType",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AttendeeId",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "TicketType",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AttendeeId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
