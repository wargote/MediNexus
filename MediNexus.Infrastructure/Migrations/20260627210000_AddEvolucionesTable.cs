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
    [Migration("20260627210000_AddEvolucionesTable")]
    /// <inheritdoc />
    public partial class AddEvolucionesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evoluciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdmissionId = table.Column<int>(type: "integer", nullable: false),
                    MotivoConsulta = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TensionArterial = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FrecuenciaCardiaca = table.Column<int>(type: "integer", nullable: true),
                    FrecuenciaRespiratoria = table.Column<int>(type: "integer", nullable: true),
                    Temperatura = table.Column<decimal>(type: "numeric(4,1)", nullable: true),
                    SaturacionOxigeno = table.Column<int>(type: "integer", nullable: true),
                    Glasgow = table.Column<int>(type: "integer", nullable: true),
                    Peso = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Talla = table.Column<decimal>(type: "numeric(4,2)", nullable: true),
                    IMC = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Plan = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evoluciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evoluciones_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evoluciones_AdmissionId",
                table: "Evoluciones",
                column: "AdmissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Evoluciones");
        }
    }
}
