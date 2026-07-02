using System;
using MediNexus.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediNexus.Infrastructure.Migrations
{
    [DbContext(typeof(MediNexusDbContext))]
    [Migration("20260627190000_HCInicial_AddSignosVitalesTable")]
    /// <inheritdoc />
    public partial class HCInicial_AddSignosVitalesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SignosVitales_HCinicial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TensionArterial = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FrecuenciaCardiaca = table.Column<int>(type: "integer", nullable: true),
                    FrecuenciaRespiratoria = table.Column<int>(type: "integer", nullable: true),
                    Temperatura = table.Column<decimal>(type: "numeric(4,1)", nullable: true),
                    SaturacionOxigeno = table.Column<int>(type: "integer", nullable: true),
                    Glasgow = table.Column<int>(type: "integer", nullable: true),
                    Peso = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Talla = table.Column<decimal>(type: "numeric(4,2)", nullable: true),
                    IMC = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignosVitales_HCinicial", x => x.Id);
                });

            migrationBuilder.AddColumn<int>(
                name: "IdSignosVitalesHCInicial",
                table: "HCInicial",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HCInicial_IdSignosVitalesHCInicial",
                table: "HCInicial",
                column: "IdSignosVitalesHCInicial");

            migrationBuilder.AddForeignKey(
                name: "FK_HCInicial_SignosVitales_HCinicial_IdSignosVitalesHCInicial",
                table: "HCInicial",
                column: "IdSignosVitalesHCInicial",
                principalTable: "SignosVitales_HCinicial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HCInicial_SignosVitales_HCinicial_IdSignosVitalesHCInicial",
                table: "HCInicial");

            migrationBuilder.DropIndex(
                name: "IX_HCInicial_IdSignosVitalesHCInicial",
                table: "HCInicial");

            migrationBuilder.DropColumn(
                name: "IdSignosVitalesHCInicial",
                table: "HCInicial");

            migrationBuilder.DropTable(
                name: "SignosVitales_HCinicial");
        }
    }
}
