using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbIns.Migrations
{
    /// <inheritdoc />
    public partial class newTableHome1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Apartments_IdApartment",
                table: "Agreements");

            migrationBuilder.RenameColumn(
                name: "IdApartment",
                table: "Agreements",
                newName: "IdHome");

            migrationBuilder.RenameIndex(
                name: "IX_Agreements_IdApartment",
                table: "Agreements",
                newName: "IX_Agreements_IdHome");

            migrationBuilder.CreateTable(
                name: "Homes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdAddress = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homes_Addresses_IdAddress",
                        column: x => x.IdAddress,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homes_IdAddress",
                table: "Homes",
                column: "IdAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Homes_IdHome",
                table: "Agreements",
                column: "IdHome",
                principalTable: "Homes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Homes_IdHome",
                table: "Agreements");

            migrationBuilder.DropTable(
                name: "Homes");

            migrationBuilder.RenameColumn(
                name: "IdHome",
                table: "Agreements",
                newName: "IdApartment");

            migrationBuilder.RenameIndex(
                name: "IX_Agreements_IdHome",
                table: "Agreements",
                newName: "IX_Agreements_IdApartment");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Apartments_IdApartment",
                table: "Agreements",
                column: "IdApartment",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
