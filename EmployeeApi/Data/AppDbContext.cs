using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Entities.Employee> Employees => Set<Entities.Employee>();

    public DbSet<Entities.EmployeePost> EmployeePostCache => Set<Entities.EmployeePost>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Entities.Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FIO).IsRequired().HasMaxLength(200);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(e => e.EmployeePost)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeePostId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Entities.EmployeePost>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.IsDeprecated).HasDefaultValue(false);
        });
    }
}
