using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediNexus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdmissionsFks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    TriageId = table.Column<int>(type: "integer", nullable: false),
                    InsurerId = table.Column<int>(type: "integer", nullable: false),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    CareModalityId = table.Column<int>(type: "integer", nullable: false),
                    CareReasonId = table.Column<int>(type: "integer", nullable: false),
                    ServiceClassificationId = table.Column<int>(type: "integer", nullable: false),
                    ServiceGroupId = table.Column<int>(type: "integer", nullable: false),
                    AdmissionTypeId = table.Column<int>(type: "integer", nullable: false),
                    CareScopeId = table.Column<int>(type: "integer", nullable: false),
                    CarePurposeId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admissions_AdmissionTypes_AdmissionTypeId",
                        column: x => x.AdmissionTypeId,
                        principalTable: "AdmissionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_CareModalities_CareModalityId",
                        column: x => x.CareModalityId,
                        principalTable: "CareModalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_CarePurposes_CarePurposeId",
                        column: x => x.CarePurposeId,
                        principalTable: "CarePurposes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_CareReasons_CareReasonId",
                        column: x => x.CareReasonId,
                        principalTable: "CareReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_CareScopes_CareScopeId",
                        column: x => x.CareScopeId,
                        principalTable: "CareScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Contracts_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Insurers_InsurerId",
                        column: x => x.InsurerId,
                        principalTable: "Insurers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_ServiceClassifications_ServiceClassificationId",
                        column: x => x.ServiceClassificationId,
                        principalTable: "ServiceClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_ServiceGroups_ServiceGroupId",
                        column: x => x.ServiceGroupId,
                        principalTable: "ServiceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Triages_TriageId",
                        column: x => x.TriageId,
                        principalTable: "Triages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_AdmissionTypeId",
                table: "Admissions",
                column: "AdmissionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_CareModalityId",
                table: "Admissions",
                column: "CareModalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_CarePurposeId",
                table: "Admissions",
                column: "CarePurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_CareReasonId",
                table: "Admissions",
                column: "CareReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_CareScopeId",
                table: "Admissions",
                column: "CareScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_ConvenioId",
                table: "Admissions",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_InsurerId",
                table: "Admissions",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_PatientId",
                table: "Admissions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_ServiceClassificationId",
                table: "Admissions",
                column: "ServiceClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_ServiceGroupId",
                table: "Admissions",
                column: "ServiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_TriageId",
                table: "Admissions",
                column: "TriageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admissions");
        }
    }
}
