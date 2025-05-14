using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbIns.Migrations
{
    /// <inheritdoc />
    public partial class new11tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "Clients",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "IdAddress",
                table: "Clients",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Passport",
                table: "Clients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Clients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TypeAnimal = table.Column<string>(type: "text", nullable: false),
                    Passport = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Desc = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LicenseAutos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseAutos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CityNumber = table.Column<string>(type: "text", nullable: false),
                    VIN = table.Column<string>(type: "text", nullable: false),
                    Mark = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NameStreet = table.Column<string>(type: "text", nullable: false),
                    NumberHome = table.Column<int>(type: "integer", nullable: false),
                    Entrance = table.Column<int>(type: "integer", nullable: false),
                    Apartment = table.Column<int>(type: "integer", nullable: false),
                    IdCity = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Cities_IdCity",
                        column: x => x.IdCity,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdAddress = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apartments_Addresses_IdAddress",
                        column: x => x.IdAddress,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsPersons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FIO = table.Column<string>(type: "text", nullable: false),
                    IdAddress = table.Column<Guid>(type: "uuid", nullable: false),
                    Passport = table.Column<string>(type: "text", nullable: false),
                    IdTransport = table.Column<Guid>(type: "uuid", nullable: false),
                    IdLicenseAuto = table.Column<Guid>(type: "uuid", nullable: false),
                    Birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsPersons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsPersons_Addresses_IdAddress",
                        column: x => x.IdAddress,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsPersons_LicenseAutos_IdLicenseAuto",
                        column: x => x.IdLicenseAuto,
                        principalTable: "LicenseAutos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsPersons_Transports_IdTransport",
                        column: x => x.IdTransport,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agreements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdClient = table.Column<Guid>(type: "uuid", nullable: false),
                    IdInsPerson = table.Column<Guid>(type: "uuid", nullable: false),
                    IdAnimal = table.Column<Guid>(type: "uuid", nullable: false),
                    IdInsCompany = table.Column<Guid>(type: "uuid", nullable: false),
                    IdInsType = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agreements_Animals_IdAnimal",
                        column: x => x.IdAnimal,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agreements_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agreements_InsCompanies_IdInsCompany",
                        column: x => x.IdInsCompany,
                        principalTable: "InsCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agreements_InsPersons_IdInsPerson",
                        column: x => x.IdInsPerson,
                        principalTable: "InsPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agreements_InsTypes_IdInsType",
                        column: x => x.IdInsType,
                        principalTable: "InsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_IdAddress",
                table: "Clients",
                column: "IdAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_IdCity",
                table: "Addresses",
                column: "IdCity");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_IdAnimal",
                table: "Agreements",
                column: "IdAnimal");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_IdClient",
                table: "Agreements",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_IdInsCompany",
                table: "Agreements",
                column: "IdInsCompany");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_IdInsPerson",
                table: "Agreements",
                column: "IdInsPerson");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_IdInsType",
                table: "Agreements",
                column: "IdInsType");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_IdAddress",
                table: "Apartments",
                column: "IdAddress");

            migrationBuilder.CreateIndex(
                name: "IX_InsPersons_IdAddress",
                table: "InsPersons",
                column: "IdAddress");

            migrationBuilder.CreateIndex(
                name: "IX_InsPersons_IdLicenseAuto",
                table: "InsPersons",
                column: "IdLicenseAuto");

            migrationBuilder.CreateIndex(
                name: "IX_InsPersons_IdTransport",
                table: "InsPersons",
                column: "IdTransport");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Addresses_IdAddress",
                table: "Clients",
                column: "IdAddress",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Addresses_IdAddress",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "Agreements");

            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "InsCompanies");

            migrationBuilder.DropTable(
                name: "InsPersons");

            migrationBuilder.DropTable(
                name: "InsTypes");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "LicenseAutos");

            migrationBuilder.DropTable(
                name: "Transports");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Clients_IdAddress",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IdAddress",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Passport",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Clients");
        }
    }
}
