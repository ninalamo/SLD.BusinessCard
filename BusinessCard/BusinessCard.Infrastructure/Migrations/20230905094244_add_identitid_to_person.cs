using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_identitid_to_person : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSubsriptionOverride",
                schema: "kardibee",
                table: "people",
                newName: "IsSubscriptionOverride");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                schema: "kardibee",
                table: "people",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                schema: "kardibee",
                table: "people");

            migrationBuilder.RenameColumn(
                name: "IsSubscriptionOverride",
                schema: "kardibee",
                table: "people",
                newName: "IsSubsriptionOverride");
        }
    }
}
