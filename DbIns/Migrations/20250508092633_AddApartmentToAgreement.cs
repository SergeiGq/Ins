using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbIns.Migrations
{
    /// <inheritdoc />
    public partial class AddApartmentToAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Homes_IdHome",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_IdHome",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "IdHome",
                table: "Agreements");

            migrationBuilder.AddColumn<Guid>(
                name: "IdApartment",
                table: "Agreements",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_IdApartment",
                table: "Agreements",
                column: "IdApartment");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Apartments_IdApartment",
                table: "Agreements",
                column: "IdApartment",
                principalTable: "Apartments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Apartments_IdApartment",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_IdApartment",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "IdApartment",
                table: "Agreements");

            migrationBuilder.AddColumn<Guid>(
                name: "IdHome",
                table: "Agreements",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_IdHome",
                table: "Agreements",
                column: "IdHome");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Homes_IdHome",
                table: "Agreements",
                column: "IdHome",
                principalTable: "Homes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
