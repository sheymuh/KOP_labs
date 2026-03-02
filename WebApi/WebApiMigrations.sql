CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "EmployeePosts" (
    "Id" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "IsDeprecated" boolean NOT NULL DEFAULT FALSE,
    CONSTRAINT "PK_EmployeePosts" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260201103348_InitialCreate', '9.0.10');

COMMIT;

