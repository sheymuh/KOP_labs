-- Создание таблиц для WebApi
CREATE TABLE IF NOT EXISTS "EmployeePosts" (
    "Id" uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    "Name" VARCHAR(100) NOT NULL,
    "IsDeprecated" BOOLEAN NOT NULL DEFAULT false
);

-- Создание таблиц для EmployeeApi
CREATE TABLE IF NOT EXISTS "EmployeePostCache" (
    "Id" uuid PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "IsDeprecated" BOOLEAN NOT NULL DEFAULT false
);

CREATE TABLE IF NOT EXISTS "Employees" (
    "Id" uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    "FIO" VARCHAR(200) NOT NULL,
    "Autobiography" TEXT,
    "PromotionDate" TIMESTAMP WITH TIME ZONE,
    "EmployeePostId" uuid NOT NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false,
    FOREIGN KEY ("EmployeePostId") REFERENCES "EmployeePostCache"("Id")
);

-- Создание индексов
CREATE INDEX IF NOT EXISTS "IX_Employees_EmployeePostId" ON "Employees" ("EmployeePostId");
CREATE INDEX IF NOT EXISTS "IX_Employees_IsDeleted" ON "Employees" ("IsDeleted");
CREATE INDEX IF NOT EXISTS "IX_EmployeePosts_IsDeprecated" ON "EmployeePosts" ("IsDeprecated");