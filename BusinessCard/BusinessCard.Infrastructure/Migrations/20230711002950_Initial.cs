using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "kardibee");

            migrationBuilder.CreateTable(
                name: "card",
                schema: "kardibee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "client",
                schema: "kardibee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDiscreet = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_membertier_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalSchema: "kardibee",
                        principalTable: "membertier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "people",
                schema: "kardibee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSubsriptionOverride = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSuffix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocialMedia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberTierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_people", x => x.Id);
                    table.ForeignKey(
                        name: "FK_people_card_CardId",
                        column: x => x.CardId,
                        principalSchema: "kardibee",
                        principalTable: "card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_people_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "kardibee",
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_people_membertier_MemberTierId",
                        column: x => x.MemberTierId,
                        principalSchema: "kardibee",
                        principalTable: "membertier",
                        principalColumn: "Id");
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
                name: "IX_client_CompanyName",
                schema: "kardibee",
                table: "client",
                column: "CompanyName",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_people_CardId",
                schema: "kardibee",
                table: "people",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_people_ClientId",
                schema: "kardibee",
                table: "people",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_people_MemberTierId",
                schema: "kardibee",
                table: "people",
                column: "MemberTierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "people",
                schema: "kardibee");

            migrationBuilder.DropTable(
                name: "card",
                schema: "kardibee");

            migrationBuilder.DropTable(
                name: "client",
                schema: "kardibee");

            migrationBuilder.DropTable(
                name: "membertier",
                schema: "kardibee");
        }
    }
}
