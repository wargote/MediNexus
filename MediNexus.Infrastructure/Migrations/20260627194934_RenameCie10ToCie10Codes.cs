using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediNexus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCie10ToCie10Codes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop FK to Cie10/Cie10Codes if it exists (name may vary)
            migrationBuilder.Sql(@"
                DO $$ DECLARE v text;
                BEGIN
                    SELECT conname INTO v
                    FROM pg_constraint c
                    JOIN pg_class t ON t.oid = c.conrelid
                    JOIN pg_class r ON r.oid = c.confrelid
                    WHERE t.relname = 'AnalisisDiagnosticosPlan_HCinicial_Diagnosticos'
                      AND r.relname IN ('Cie10', 'Cie10Codes')
                      AND c.contype = 'f';
                    IF v IS NOT NULL THEN
                        EXECUTE format('ALTER TABLE ""AnalisisDiagnosticosPlan_HCinicial_Diagnosticos"" DROP CONSTRAINT %I', v);
                    END IF;
                END $$;");

            // Rename Cie10 to Cie10Codes only if Cie10 still exists
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM pg_class WHERE relname = 'Cie10' AND relkind = 'r') THEN
                        IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'PK_Cie10') THEN
                            ALTER TABLE ""Cie10"" RENAME CONSTRAINT ""PK_Cie10"" TO ""PK_Cie10Codes"";
                        END IF;
                        ALTER TABLE ""Cie10"" RENAME TO ""Cie10Codes"";
                    END IF;
                END $$;");

            // Rename index only if old name exists
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'IX_Cie10_Codigo') THEN
                        ALTER INDEX ""IX_Cie10_Codigo"" RENAME TO ""IX_Cie10Codes_Codigo"";
                    END IF;
                END $$;");

            // Drop Cie10Codes if it exists with wrong schema (e.g. lowercase "id" from an old migration)
            // so migration 200000 can recreate it correctly. Only drop it when Cie10 was NOT renamed
            // (i.e. the table came from elsewhere, not from our rename step above).
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM pg_class WHERE relname = 'Cie10Codes' AND relkind = 'r') THEN
                        IF NOT EXISTS (
                            SELECT 1 FROM information_schema.columns
                            WHERE table_name = 'Cie10Codes' AND column_name = 'Id'
                        ) THEN
                            DROP TABLE ""Cie10Codes"" CASCADE;
                        END IF;
                    END IF;
                END $$;");

            // Add FK to Cie10Codes only if both the table and the FK don't exist yet
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM pg_class WHERE relname = 'Cie10Codes' AND relkind = 'r') THEN
                        IF NOT EXISTS (
                            SELECT 1 FROM pg_constraint c
                            JOIN pg_class t ON t.oid = c.conrelid
                            JOIN pg_class r ON r.oid = c.confrelid
                            WHERE t.relname = 'AnalisisDiagnosticosPlan_HCinicial_Diagnosticos'
                              AND r.relname = 'Cie10Codes'
                              AND c.contype = 'f'
                        ) THEN
                            ALTER TABLE ""AnalisisDiagnosticosPlan_HCinicial_Diagnosticos""
                            ADD CONSTRAINT ""FK_AnalisisDiagnosticosPlan_HCinicial_Diagnosticos_Cie10Codes_~""
                            FOREIGN KEY (""IdCie10"") REFERENCES ""Cie10Codes"" (""Id"")
                            ON DELETE RESTRICT;
                        END IF;
                    END IF;
                END $$;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalisisDiagnosticosPlan_HCinicial_Diagnosticos_Cie10Codes_~",
                table: "AnalisisDiagnosticosPlan_HCinicial_Diagnosticos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cie10Codes",
                table: "Cie10Codes");

            migrationBuilder.RenameTable(
                name: "Cie10Codes",
                newName: "Cie10");

            migrationBuilder.RenameIndex(
                name: "IX_Cie10Codes_Codigo",
                table: "Cie10",
                newName: "IX_Cie10_Codigo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cie10",
                table: "Cie10",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalisisDiagnosticosPlan_HCinicial_Diagnosticos_IdCie10",
                table: "AnalisisDiagnosticosPlan_HCinicial_Diagnosticos",
                column: "IdCie10",
                principalTable: "Cie10",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
