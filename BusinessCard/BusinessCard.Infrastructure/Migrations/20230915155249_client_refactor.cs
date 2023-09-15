using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class client_refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDiscreet",
                schema: "dbo",
                table: "client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDiscreet",
                schema: "dbo",
                table: "client",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
