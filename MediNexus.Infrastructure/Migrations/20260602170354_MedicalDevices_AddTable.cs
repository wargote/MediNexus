using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediNexus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MedicalDevices_AddTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElementTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElementUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementUsages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ElementName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    ElementTypeId = table.Column<int>(type: "integer", nullable: false),
                    ElementUsageId = table.Column<int>(type: "integer", nullable: false),
                    RipsCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsReusable = table.Column<bool>(type: "boolean", nullable: false),
                    IsInvasive = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalDevices_ElementTypes_ElementTypeId",
                        column: x => x.ElementTypeId,
                        principalTable: "ElementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalDevices_ElementUsages_ElementUsageId",
                        column: x => x.ElementUsageId,
                        principalTable: "ElementUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElementTypes_Name",
                table: "ElementTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ElementUsages_Name",
                table: "ElementUsages",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalDevices_ElementName",
                table: "MedicalDevices",
                column: "ElementName");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalDevices_ElementTypeId",
                table: "MedicalDevices",
                column: "ElementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalDevices_ElementUsageId",
                table: "MedicalDevices",
                column: "ElementUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalDevices_RipsCode",
                table: "MedicalDevices",
                column: "RipsCode");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalDevices");

            migrationBuilder.DropTable(
                name: "ElementTypes");

            migrationBuilder.DropTable(
                name: "ElementUsages");
        }
    }
}
