using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataObjects_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddResetPasswordColumnsToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpiration",
                table: "Accounts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpiration",
                table: "Accounts");
        }
    }
}
