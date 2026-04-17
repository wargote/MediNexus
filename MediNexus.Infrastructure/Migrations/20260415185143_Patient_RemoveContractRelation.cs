using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediNexus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Patient_RemoveContractRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Contracts_ContractId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ContractId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Patients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Patients",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ContractId",
                table: "Patients",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Contracts_ContractId",
                table: "Patients",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
