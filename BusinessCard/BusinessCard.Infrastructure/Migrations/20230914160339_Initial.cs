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
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "billingplan",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billingplan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "client",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDiscreet = table.Column<bool>(type: "bit", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBlackList = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "people",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSubscriptionOverride = table.Column<bool>(type: "bit", nullable: false),
                    IdentityUserId = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSuffix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocialMedia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_people_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscription",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ActualEndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentScheduleReminderInterval = table.Column<int>(type: "int", nullable: false),
                    PaymentScheduleInterval = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BillingPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_subscription_billingplan_BillingPlanId",
                        column: x => x.BillingPlanId,
                        principalSchema: "dbo",
                        principalTable: "billingplan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_subscription_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "client",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "card",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Uid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActivatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExpireDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RenewDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card", x => x.Id);
                    table.ForeignKey(
                        name: "FK_card_people_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "people",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cardsetting",
                schema: "dbo",
                columns: table => new
                {
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiresInMonths = table.Column<int>(type: "int", nullable: false, defaultValue: 12)
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

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "billingplan",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "Free Trial" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "Monthly" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "Yearly" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_billingplan_Name",
                schema: "dbo",
                table: "billingplan",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_card_PersonId",
                schema: "dbo",
                table: "card",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_card_Uid",
                schema: "dbo",
                table: "card",
                column: "Uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_client_Name",
                schema: "dbo",
                table: "client",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_people_ClientId",
                schema: "dbo",
                table: "people",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_people_Email",
                schema: "dbo",
                table: "people",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_people_PhoneNumber",
                schema: "dbo",
                table: "people",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscription_BillingPlanId",
                schema: "dbo",
                table: "subscription",
                column: "BillingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_subscription_ClientId",
                schema: "dbo",
                table: "subscription",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "card",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "cardsetting",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "people",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "subscription",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "billingplan",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "client",
                schema: "dbo");
        }
    }
}
