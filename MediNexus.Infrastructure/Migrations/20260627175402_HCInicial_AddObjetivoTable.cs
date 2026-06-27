using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediNexus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HCInicial_AddObjetivoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdObjetivoHCInicial",
                table: "HCInicial",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Objetivo_HCinicial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CabezaCuello = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Torax = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Abdomen = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Extremidades = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SistemaNervioso = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OrganosSentidos = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Genitourinario = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Padres = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PersonalesMedicos = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OtrosFamiliares = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Alergicos = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Quirurgicos = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Toxicos = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Transfusiones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Habitos = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Traumas = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objetivo_HCinicial", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HCInicial_IdObjetivoHCInicial",
                table: "HCInicial",
                column: "IdObjetivoHCInicial");

            migrationBuilder.AddForeignKey(
                name: "FK_HCInicial_Objetivo_HCinicial_IdObjetivoHCInicial",
                table: "HCInicial",
                column: "IdObjetivoHCInicial",
                principalTable: "Objetivo_HCinicial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HCInicial_Objetivo_HCinicial_IdObjetivoHCInicial",
                table: "HCInicial");

            migrationBuilder.DropTable(
                name: "Objetivo_HCinicial");

            migrationBuilder.DropIndex(
                name: "IX_HCInicial_IdObjetivoHCInicial",
                table: "HCInicial");

            migrationBuilder.DropColumn(
                name: "IdObjetivoHCInicial",
                table: "HCInicial");
        }
    }
}
