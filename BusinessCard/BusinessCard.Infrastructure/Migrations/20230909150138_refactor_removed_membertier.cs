using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactor_removed_membertier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_membertier_SubscriptionId",
                schema: "kardibee",
                table: "client");

            migrationBuilder.DropForeignKey(
                name: "FK_people_membertier_SubscriptionId",
                schema: "kardibee",
                table: "people");

            migrationBuilder.DropTable(
                name: "membertier",
                schema: "kardibee");

            migrationBuilder.DropIndex(
                name: "IX_people_SubscriptionId",
                schema: "kardibee",
                table: "people");

            migrationBuilder.DropIndex(
                name: "IX_client_SubscriptionId",
                schema: "kardibee",
                table: "client");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                schema: "kardibee",
                table: "people");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                schema: "kardibee",
                table: "client");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "kardibee",
                table: "card",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "kardibee",
                table: "card");

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                schema: "kardibee",
                table: "people",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                schema: "kardibee",
                table: "client",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "membertier",
                schema: "kardibee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_membertier", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "kardibee",
                table: "membertier",
                columns: new[] { "Id", "Level", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), 1, "level_one" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), 2, "level_two" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), 3, "level_three" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), 4, "level_four" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), 5, "level_five" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), 6, "level_six" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), 7, "level_seven" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_people_SubscriptionId",
                schema: "kardibee",
                table: "people",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_client_SubscriptionId",
                schema: "kardibee",
                table: "client",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_membertier_Level",
                schema: "kardibee",
                table: "membertier",
                column: "Level",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_membertier_Name",
                schema: "kardibee",
                table: "membertier",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_client_membertier_SubscriptionId",
                schema: "kardibee",
                table: "client",
                column: "SubscriptionId",
                principalSchema: "kardibee",
                principalTable: "membertier",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_people_membertier_SubscriptionId",
                schema: "kardibee",
                table: "people",
                column: "SubscriptionId",
                principalSchema: "kardibee",
                principalTable: "membertier",
                principalColumn: "Id");
        }
    }
}
