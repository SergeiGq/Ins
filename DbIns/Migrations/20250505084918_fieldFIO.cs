using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbIns.Migrations
{
    /// <inheritdoc />
    public partial class fieldFIO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FIO",
                table: "Clients",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FIO",
                table: "Clients");
        }
    }
}
