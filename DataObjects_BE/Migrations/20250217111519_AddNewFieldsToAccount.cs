using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataObjects_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Otp",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpCreatedAt",
                table: "Accounts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Otp",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "OtpCreatedAt",
                table: "Accounts");
        }
    }
}
