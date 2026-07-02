-- ============================================================
-- Script: Tabla CIE-10 Códigos (Cie10Codes)
-- Sistema: MediNexus
-- Fecha:   2026-06-18
-- ============================================================

-- 1. Creación de la tabla
CREATE TABLE IF NOT EXISTS public."Cie10Codes" (
    "COD_4"                              VARCHAR(4)   NOT NULL,
    "DESCRIPCION_CODIGO_CUATRO_CARACTERES" VARCHAR(500) NOT NULL,
    CONSTRAINT "PK_Cie10Codes" PRIMARY KEY ("COD_4")
);

-- 2. Índice sobre la descripción para acelerar búsquedas de texto
CREATE INDEX IF NOT EXISTS "IX_Cie10Codes_Descripcion"
    ON public."Cie10Codes" ("DESCRIPCION_CODIGO_CUATRO_CARACTERES");

-- 3. Comentarios de columna
COMMENT ON TABLE  public."Cie10Codes"
    IS 'Repositorio maestro de códigos CIE-10 de cuatro caracteres para consulta y validación de diagnósticos.';
COMMENT ON COLUMN public."Cie10Codes"."COD_4"
    IS 'Código CIE-10 de cuatro caracteres. Clave primaria natural (ej. A001, J180).';
COMMENT ON COLUMN public."Cie10Codes"."DESCRIPCION_CODIGO_CUATRO_CARACTERES"
    IS 'Descripción asociada al código CIE-10 de cuatro caracteres.';

-- 4. Datos de ejemplo (opcional - puede eliminar si carga datos desde un bulk insert)
INSERT INTO public."Cie10Codes" ("COD_4", "DESCRIPCION_CODIGO_CUATRO_CARACTERES")
VALUES
    ('A000', 'Cólera debida a Vibrio cholerae 01, biotipo cholerae'),
    ('A001', 'Cólera debida a Vibrio cholerae 01, biotipo El Tor'),
    ('A009', 'Cólera, no especificada'),
    ('J180', 'Neumonía debida a bronconeumonía, no especificada'),
    ('J189', 'Neumonía, no especificada'),
    ('Z000', 'Examen médico general'),
    ('E119', 'Diabetes mellitus tipo 2, sin complicaciones'),
    ('I109', 'Hipertensión esencial, sin otra especificación')
ON CONFLICT ("COD_4") DO NOTHING;
