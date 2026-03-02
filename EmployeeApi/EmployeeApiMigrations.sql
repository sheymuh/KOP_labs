CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "EmployeePostCache" (
    "Id" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "IsDeprecated" boolean NOT NULL DEFAULT FALSE,
    CONSTRAINT "PK_EmployeePostCache" PRIMARY KEY ("Id")
);

CREATE TABLE "Employees" (
    "Id" uuid NOT NULL,
    "FIO" character varying(200) NOT NULL,
    "Autobiography" text,
    "PromotionDate" timestamp with time zone,
    "EmployeePostId" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL DEFAULT FALSE,
    CONSTRAINT "PK_Employees" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Employees_EmployeePostCache_EmployeePostId" FOREIGN KEY ("EmployeePostId") REFERENCES "EmployeePostCache" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_Employees_EmployeePostId" ON "Employees" ("EmployeePostId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260201103723_InitialCreate', '9.0.10');

COMMIT;

