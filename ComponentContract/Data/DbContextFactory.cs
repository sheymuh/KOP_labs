using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ComponentContract.Data;

public static class DbContextFactory
{
    private static IConfiguration? _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public static CompanyDbContext CreateDbContext()
    {
        if (_configuration == null)
            throw new InvalidOperationException("DbContextFactory not initialized. Call Initialize() first.");

        var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();
        var connectionString = _configuration.GetConnectionString("CompanyDb")
            ?? throw new InvalidOperationException("Connection string 'CompanyDb' not found.");

        optionsBuilder.UseNpgsql(connectionString);
        return new CompanyDbContext(optionsBuilder.Options);
    }
}
