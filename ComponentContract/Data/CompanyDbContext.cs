using Microsoft.EntityFrameworkCore;
using ComponentContract.Entities;

namespace ComponentContract.Data;

public class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeePost> EmployeePosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Указание имен таблиц
        modelBuilder.Entity<Employee>().ToTable("employee");
        modelBuilder.Entity<EmployeePost>().ToTable("employee_post");

        // Настройка EmployeePost с указанием имен столбцов
        modelBuilder.Entity<EmployeePost>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                  .HasColumnName("id"); // явно указываем имя столбца

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100)
                  .HasColumnName("name"); // явно указываем имя столбца

            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Настройка Employee с указанием имен столбцов
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                  .HasColumnName("id");

            entity.Property(e => e.FIO)
                  .IsRequired()
                  .HasMaxLength(200)
                  .HasColumnName("fio");

            entity.Property(e => e.Autobiography)
                  .HasMaxLength(500)
                  .HasColumnName("autobiography");

            entity.Property(e => e.EmployeePostId)
                  .HasColumnName("employee_post_id");

            entity.Property(e => e.PromotionDate)
                  .HasColumnType("timestamp without time zone")
                  .HasColumnName("promotion_date");

            // Связь с должностью работника
            entity.HasOne(e => e.EmployeePost)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeePostId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Настройка EmployeePost
        modelBuilder.Entity<EmployeePost>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                  .HasColumnName("id");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100)
                  .HasColumnName("name");
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Настройка типов данных для PostgreSQL
        modelBuilder.Entity<Employee>()
            .Property(e => e.PromotionDate)
            .HasColumnName("promotion_date")
            .HasColumnType("timestamp without time zone");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>()
            .HaveColumnType("timestamp without time zone");

        configurationBuilder.Properties<DateTime?>()
            .HaveColumnType("timestamp without time zone");
    }
    public async Task<bool> TestDatabaseConnection()
    {
        try
        {
            var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=company_db;Username=postgres;Password=postgres");

            using var context = new CompanyDbContext(optionsBuilder.Options);

            // Простая проверка - попытка выполнить запрос
            var canConnect = await context.Database.CanConnectAsync();

            if (canConnect)
            {
                Console.WriteLine("Подключение к базе данных успешно!");
                return true;
            }
            else
            {
                Console.WriteLine("Не удалось подключиться к базе данных");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка подключения: {ex.Message}");
            return false;
        }

    }
}
