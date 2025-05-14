using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbIns.Migrations
{
    /// <inheritdoc />
    public partial class newFieldInAgrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Animals_IdAnimal",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_InsPersons_IdInsPerson",
                table: "Agreements");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdInsPerson",
                table: "Agreements",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdAnimal",
                table: "Agreements",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

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
                name: "FK_Agreements_Animals_IdAnimal",
                table: "Agreements",
                column: "IdAnimal",
                principalTable: "Animals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Apartments_IdApartment",
                table: "Agreements",
                column: "IdApartment",
                principalTable: "Apartments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_InsPersons_IdInsPerson",
                table: "Agreements",
                column: "IdInsPerson",
                principalTable: "InsPersons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Animals_IdAnimal",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Apartments_IdApartment",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_InsPersons_IdInsPerson",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_IdApartment",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "IdApartment",
                table: "Agreements");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdInsPerson",
                table: "Agreements",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IdAnimal",
                table: "Agreements",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Animals_IdAnimal",
                table: "Agreements",
                column: "IdAnimal",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_InsPersons_IdInsPerson",
                table: "Agreements",
                column: "IdInsPerson",
                principalTable: "InsPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
