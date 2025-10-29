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

        // Настройка Employee
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FIO).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Autobiography).HasMaxLength(500);
            entity.Property(e => e.PromotionDate).HasColumnType("timestamp without time zone");

            // Связь с должностью работника
            entity.HasOne(e => e.EmployeePost)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeePostId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Иерархия работников
            entity.HasOne(e => e.Parent)
                  .WithMany(e => e.Children)
                  .HasForeignKey(e => e.ParentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Настройка EmployeePost
        modelBuilder.Entity<EmployeePost>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Настройка типов данных для PostgreSQL
        modelBuilder.Entity<Employee>()
            .Property(e => e.PromotionDate)
            .HasColumnType("timestamp without time zone");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>()
            .HaveColumnType("timestamp without time zone");

        configurationBuilder.Properties<DateTime?>()
            .HaveColumnType("timestamp without time zone");
    }
}
