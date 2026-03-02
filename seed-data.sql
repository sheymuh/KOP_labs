-- Ќаполнение таблицы EmployeePosts
INSERT INTO "EmployeePosts" ("Id", "Name", "IsDeprecated") VALUES
    ('11111111-1111-1111-1111-111111111111', 'директор', false),
    ('22222222-2222-2222-2222-222222222222', 'крутой работник', false),
    ('33333333-3333-3333-3333-333333333333', 'работник', false),
    ('44444444-4444-4444-4444-444444444444', 'устаревший работник', false),
ON CONFLICT ("Id") DO NOTHING;

-- Ќаполнение таблицы EmployeePostCache (копи€ из EmployeePosts)
-- EmployeeApi будет синхронизировать, но на начальный запуск нужно заполнить
INSERT INTO "EmployeePostCache" ("Id", "Name", "IsDeprecated")
SELECT "Id", "Name", "IsDeprecated" FROM "EmployeePosts"
ON CONFLICT ("Id") DO NOTHING;

-- Ќаполнение таблицы Employees (EmployeeApi)
DO $$ 
BEGIN
    -- Ќебольша€ пауза дл€ гарантии заполнени€ EmployeePostCache
    PERFORM pg_sleep(0.1);
    
    IF NOT EXISTS (SELECT 1 FROM "Employees" LIMIT 1) THEN
        INSERT INTO "Employees" ("Id", "FIO", "EmployeePostId", "Autobiography", "PromotionDate", "IsDeleted") VALUES
            ('cccccccc-cccc-cccc-cccc-cccccccccccc', 'серега', '11111111-1111-1111-1111-111111111111', 'бизнесмен', '2023-05-15', false),
            ('dddddddd-dddd-dddd-dddd-dddddddddddd', 'санек', '11111111-1111-1111-1111-111111111111', 'друг бизнесмена', '2024-02-20', false),

            ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'андрюха', '22222222-2222-2222-2222-222222222222', 'умеет, опыт есть', '2024-08-10', false),
            
            ('ffffffff-ffff-ffff-ffff-ffffffffffff', 'светка', '33333333-3333-3333-3333-333333333333', 'неплохо делает', '2024-11-05', false),
            ('11111111-2222-3333-4444-555555555555', 'пашка', '33333333-3333-3333-3333-333333333333', 'стараетс€', NULL, false),
            ('22222222-3333-4444-5555-666666666666', 'наташка', '33333333-3333-3333-3333-333333333333', 'что то может', NULL, false),
            
            -- ”даленный сотрудник
            ('66666666-7777-8888-9999-aaaaaaaaaaaa', 'олег', '22222222-2222-2222-2222-222222222222', 'легенда', '2023-03-10', true);
    END IF;
END $$;