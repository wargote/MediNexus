using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediNexus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20260114_PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.AlterColumn<string>(
                name: "Route",
                table: "NavigationSubMenus",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NavigationSubMenus",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NavigationModules",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Route",
                table: "NavigationMenus",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NavigationMenus",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "NavigationMenus",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TariffScheduleDetails",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodeRef = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    PrToQx = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Factors = table.Column<decimal>(type: "numeric(10,4)", nullable: true),
                    TariffScheduleId = table.Column<int>(type: "integer", nullable: false),
                    QxGroupId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffScheduleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TariffScheduleDetails_QxGroups_QxGroupId",
                        column: x => x.QxGroupId,
                        principalSchema: "public",
                        principalTable: "QxGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TariffScheduleDetails_TariffSchedules_TariffScheduleId",
                        column: x => x.TariffScheduleId,
                        principalSchema: "public",
                        principalTable: "TariffSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UX_QxGroups_ReferenceCodification",
                schema: "public",
                table: "QxGroups",
                column: "ReferenceCodification",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TariffScheduleDetails_CodeRef",
                schema: "public",
                table: "TariffScheduleDetails",
                column: "CodeRef");

            migrationBuilder.CreateIndex(
                name: "IX_TariffScheduleDetails_QxGroupId",
                schema: "public",
                table: "TariffScheduleDetails",
                column: "QxGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffScheduleDetails_TariffScheduleId",
                schema: "public",
                table: "TariffScheduleDetails",
                column: "TariffScheduleId");

            migrationBuilder.CreateIndex(
                name: "UX_TariffScheduleDetails_TariffScheduleId_CodeRef",
                schema: "public",
                table: "TariffScheduleDetails",
                columns: new[] { "TariffScheduleId", "CodeRef" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_TariffSchedules_Name",
                schema: "public",
                table: "TariffSchedules",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TariffScheduleDetails",
                schema: "public");

            migrationBuilder.AlterColumn<string>(
                name: "Route",
                table: "NavigationSubMenus",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NavigationSubMenus",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NavigationModules",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Route",
                table: "NavigationMenus",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NavigationMenus",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "NavigationMenus",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
