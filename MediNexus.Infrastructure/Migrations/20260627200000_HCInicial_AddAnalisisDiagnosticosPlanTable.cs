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
    [Migration("20260627200000_HCInicial_AddAnalisisDiagnosticosPlanTable")]
    /// <inheritdoc />
    public partial class HCInicial_AddAnalisisDiagnosticosPlanTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cie10Codes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cie10Codes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnalisisDiagnosticosPlan_HCinicial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Analisis = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Plan = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalisisDiagnosticosPlan_HCinicial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnalisisDiagnosticosPlan_HCinicial_Diagnosticos",
                columns: table => new
                {
                    IdAnalisisDiagnosticosPlan = table.Column<int>(type: "integer", nullable: false),
                    IdCie10 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalisisDiagnosticosPlan_HCinicial_Diagnosticos",
                        x => new { x.IdAnalisisDiagnosticosPlan, x.IdCie10 });

                    table.ForeignKey(
                        name: "FK_AnalisisDiagnosticosPlan_HCinicial_Diagnosticos_IdAnalisis",
                        column: x => x.IdAnalisisDiagnosticosPlan,
                        principalTable: "AnalisisDiagnosticosPlan_HCinicial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_AnalisisDiagnosticosPlan_HCinicial_Diagnosticos_IdCie10",
                        column: x => x.IdCie10,
                        principalTable: "Cie10Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<int>(
                name: "IdAnalisisDiagnosticosPlanHCInicial",
                table: "HCInicial",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cie10Codes_Codigo",
                table: "Cie10Codes",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalisisDiagnosticosPlan_HCinicial_Diagnosticos_IdCie10",
                table: "AnalisisDiagnosticosPlan_HCinicial_Diagnosticos",
                column: "IdCie10");

            migrationBuilder.CreateIndex(
                name: "IX_HCInicial_IdAnalisisDiagnosticosPlanHCInicial",
                table: "HCInicial",
                column: "IdAnalisisDiagnosticosPlanHCInicial");

            migrationBuilder.AddForeignKey(
                name: "FK_HCInicial_AnalisisDiagnosticosPlan_HCinicial_IdAnalisis",
                table: "HCInicial",
                column: "IdAnalisisDiagnosticosPlanHCInicial",
                principalTable: "AnalisisDiagnosticosPlan_HCinicial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HCInicial_AnalisisDiagnosticosPlan_HCinicial_IdAnalisis",
                table: "HCInicial");

            migrationBuilder.DropIndex(
                name: "IX_HCInicial_IdAnalisisDiagnosticosPlanHCInicial",
                table: "HCInicial");

            migrationBuilder.DropColumn(
                name: "IdAnalisisDiagnosticosPlanHCInicial",
                table: "HCInicial");

            migrationBuilder.DropTable(
                name: "AnalisisDiagnosticosPlan_HCinicial_Diagnosticos");

            migrationBuilder.DropTable(
                name: "AnalisisDiagnosticosPlan_HCinicial");

            migrationBuilder.DropTable(
                name: "Cie10Codes");
        }
    }
}
