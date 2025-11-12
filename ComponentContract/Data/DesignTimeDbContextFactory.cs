using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ComponentContract.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CompanyDbContext>
{
    public CompanyDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();
        var connectionString = configuration.GetConnectionString("CompanyDb")
            ?? "Host=localhost;Port=5432;Database=company_db;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString);
        return new CompanyDbContext(optionsBuilder.Options);
    }
}

