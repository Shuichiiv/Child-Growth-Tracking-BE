using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataObjects_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReportProductRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductLists_Reports_ReportId",
                table: "ProductLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductLists_ReportId",
                table: "ProductLists");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "ProductLists");

            migrationBuilder.AddColumn<string>(
                name: "ProductType",
                table: "ProductLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ReportProducts",
                columns: table => new
                {
                    ReportProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportProducts", x => x.ReportProductId);
                    table.ForeignKey(
                        name: "FK_ReportProducts_ProductLists_ProductListId",
                        column: x => x.ProductListId,
                        principalTable: "ProductLists",
                        principalColumn: "ProductListId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportProducts_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportProducts_ProductListId",
                table: "ReportProducts",
                column: "ProductListId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportProducts_ReportId",
                table: "ReportProducts",
                column: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportProducts");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "ProductLists");

            migrationBuilder.AddColumn<Guid>(
                name: "ReportId",
                table: "ProductLists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductLists_ReportId",
                table: "ProductLists",
                column: "ReportId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLists_Reports_ReportId",
                table: "ProductLists",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "ReportId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
