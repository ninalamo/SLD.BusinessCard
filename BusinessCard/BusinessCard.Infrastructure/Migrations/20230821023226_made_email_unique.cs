using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class made_email_unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "kardibee",
                table: "people",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "kardibee",
                table: "people",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_people_Email",
                schema: "kardibee",
                table: "people",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_people_PhoneNumber",
                schema: "kardibee",
                table: "people",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_people_Email",
                schema: "kardibee",
                table: "people");

            migrationBuilder.DropIndex(
                name: "IX_people_PhoneNumber",
                schema: "kardibee",
                table: "people");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "kardibee",
                table: "people",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "kardibee",
                table: "people",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
