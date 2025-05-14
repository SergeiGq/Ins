using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbIns.Migrations
{
    /// <inheritdoc />
    public partial class changefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsPersons_LicenseAutos_IdLicenseAuto",
                table: "InsPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_InsPersons_Transports_IdTransport",
                table: "InsPersons");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdTransport",
                table: "InsPersons",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdLicenseAuto",
                table: "InsPersons",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_InsPersons_LicenseAutos_IdLicenseAuto",
                table: "InsPersons",
                column: "IdLicenseAuto",
                principalTable: "LicenseAutos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InsPersons_Transports_IdTransport",
                table: "InsPersons",
                column: "IdTransport",
                principalTable: "Transports",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsPersons_LicenseAutos_IdLicenseAuto",
                table: "InsPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_InsPersons_Transports_IdTransport",
                table: "InsPersons");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdTransport",
                table: "InsPersons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IdLicenseAuto",
                table: "InsPersons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InsPersons_LicenseAutos_IdLicenseAuto",
                table: "InsPersons",
                column: "IdLicenseAuto",
                principalTable: "LicenseAutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InsPersons_Transports_IdTransport",
                table: "InsPersons",
                column: "IdTransport",
                principalTable: "Transports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
