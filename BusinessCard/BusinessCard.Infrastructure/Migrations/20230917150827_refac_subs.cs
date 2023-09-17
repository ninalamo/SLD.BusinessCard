using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refac_subs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cardsetting",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_subscription_ClientId",
                schema: "dbo",
                table: "subscription");

            migrationBuilder.AddColumn<int>(
                name: "CardExpiryInMonths",
                schema: "dbo",
                table: "subscription",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "subscription",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                schema: "dbo",
                table: "subscription",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_subscription_ClientId_Level",
                schema: "dbo",
                table: "subscription",
                columns: new[] { "ClientId", "Level" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_subscription_ClientId_Level",
                schema: "dbo",
                table: "subscription");

            migrationBuilder.DropColumn(
                name: "CardExpiryInMonths",
                schema: "dbo",
                table: "subscription");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "subscription");

            migrationBuilder.DropColumn(
                name: "Level",
                schema: "dbo",
                table: "subscription");

            migrationBuilder.CreateTable(
                name: "cardsetting",
                schema: "dbo",
                columns: table => new
                {
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiresInMonths = table.Column<int>(type: "int", nullable: false, defaultValue: 12),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cardsetting", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_cardsetting_subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalSchema: "dbo",
                        principalTable: "subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_subscription_ClientId",
                schema: "dbo",
                table: "subscription",
                column: "ClientId");
        }
    }
}
