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
        var connectionString = configuration.GetConnectionString("OrganisationDb")
            ?? "Host=localhost;Port=5432;Database=organisation_db;Username=postgres;Password=admin123";

        optionsBuilder.UseNpgsql(connectionString);
        return new CompanyDbContext(optionsBuilder.Options);
    }
}

