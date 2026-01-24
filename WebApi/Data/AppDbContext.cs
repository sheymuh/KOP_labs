using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<EmployeePost> EmployeePosts => Set<EmployeePost>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // EmployeePost
        modelBuilder.Entity<EmployeePost>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(100);
            entity.Property(e => e.IsDeprecated)
              .HasDefaultValue(false);
        });
    }
}
