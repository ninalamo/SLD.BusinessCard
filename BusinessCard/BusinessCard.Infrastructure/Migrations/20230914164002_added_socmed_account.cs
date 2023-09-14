using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_socmed_account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "socialmedia",
                schema: "dbo",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Facebook = table.Column<string>(type: "nvarchar(56)", maxLength: 56, nullable: false),
                    Instagram = table.Column<string>(type: "nvarchar(56)", maxLength: 56, nullable: false),
                    LinkedIn = table.Column<string>(type: "nvarchar(56)", maxLength: 56, nullable: false),
                    Pinterest = table.Column<string>(type: "nvarchar(56)", maxLength: 56, nullable: false),
                    Twitter = table.Column<string>(type: "nvarchar(56)", maxLength: 56, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socialmedia", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_socialmedia_people_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "people",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "socialmedia",
                schema: "dbo");
        }
    }
}
