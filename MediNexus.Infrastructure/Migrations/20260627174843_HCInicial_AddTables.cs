using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediNexus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HCInicial_AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subjetivo_HCinicial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MotivoConsulta = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    EnfermedadActual = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjetivo_HCinicial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HCInicial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdmissionId = table.Column<int>(type: "integer", nullable: false),
                    IdSubjetivoHCInicial = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HCInicial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HCInicial_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HCInicial_Subjetivo_HCinicial_IdSubjetivoHCInicial",
                        column: x => x.IdSubjetivoHCInicial,
                        principalTable: "Subjetivo_HCinicial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HCInicial_AdmissionId",
                table: "HCInicial",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_HCInicial_IdSubjetivoHCInicial",
                table: "HCInicial",
                column: "IdSubjetivoHCInicial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HCInicial");

            migrationBuilder.DropTable(
                name: "Subjetivo_HCinicial");
        }
    }
}
