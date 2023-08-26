using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changed_person_subscriptoion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_people_card_CardId",
                schema: "kardibee",
                table: "people");

            migrationBuilder.DropForeignKey(
                name: "FK_people_membertier_MemberTierId",
                schema: "kardibee",
                table: "people");

            migrationBuilder.RenameColumn(
                name: "MemberTierId",
                schema: "kardibee",
                table: "people",
                newName: "_subscription");

            migrationBuilder.RenameIndex(
                name: "IX_people_MemberTierId",
                schema: "kardibee",
                table: "people",
                newName: "IX_people__subscription");

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                schema: "kardibee",
                table: "people",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_people_card_CardId",
                schema: "kardibee",
                table: "people",
                column: "CardId",
                principalSchema: "kardibee",
                principalTable: "card",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_people_membertier__subscription",
                schema: "kardibee",
                table: "people",
                column: "_subscription",
                principalSchema: "kardibee",
                principalTable: "membertier",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_people_card_CardId",
                schema: "kardibee",
                table: "people");

            migrationBuilder.DropForeignKey(
                name: "FK_people_membertier__subscription",
                schema: "kardibee",
                table: "people");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                schema: "kardibee",
                table: "people");

            migrationBuilder.RenameColumn(
                name: "_subscription",
                schema: "kardibee",
                table: "people",
                newName: "MemberTierId");

            migrationBuilder.RenameIndex(
                name: "IX_people__subscription",
                schema: "kardibee",
                table: "people",
                newName: "IX_people_MemberTierId");

            migrationBuilder.AddForeignKey(
                name: "FK_people_card_CardId",
                schema: "kardibee",
                table: "people",
                column: "CardId",
                principalSchema: "kardibee",
                principalTable: "card",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_people_membertier_MemberTierId",
                schema: "kardibee",
                table: "people",
                column: "MemberTierId",
                principalSchema: "kardibee",
                principalTable: "membertier",
                principalColumn: "Id");
        }
    }
}
