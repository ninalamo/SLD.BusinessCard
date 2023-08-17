using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixed_configuration_for_membertier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_membertier_SubscriptionId",
                schema: "kardibee",
                table: "client");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                schema: "kardibee",
                table: "client",
                newName: "MemberTierId");

            migrationBuilder.RenameIndex(
                name: "IX_client_SubscriptionId",
                schema: "kardibee",
                table: "client",
                newName: "IX_client_MemberTierId");

            migrationBuilder.AddForeignKey(
                name: "FK_client_membertier_MemberTierId",
                schema: "kardibee",
                table: "client",
                column: "MemberTierId",
                principalSchema: "kardibee",
                principalTable: "membertier",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_membertier_MemberTierId",
                schema: "kardibee",
                table: "client");

            migrationBuilder.RenameColumn(
                name: "MemberTierId",
                schema: "kardibee",
                table: "client",
                newName: "SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_client_MemberTierId",
                schema: "kardibee",
                table: "client",
                newName: "IX_client_SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_client_membertier_SubscriptionId",
                schema: "kardibee",
                table: "client",
                column: "SubscriptionId",
                principalSchema: "kardibee",
                principalTable: "membertier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
