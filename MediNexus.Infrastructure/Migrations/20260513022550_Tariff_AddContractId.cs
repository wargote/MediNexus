using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediNexus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Tariff_AddContractId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Tariffs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tariffs_ContractId",
                table: "Tariffs",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tariffs_Contracts_ContractId",
                table: "Tariffs",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tariffs_Contracts_ContractId",
                table: "Tariffs");

            migrationBuilder.DropIndex(
                name: "IX_Tariffs_ContractId",
                table: "Tariffs");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Tariffs");
        }
    }
}
