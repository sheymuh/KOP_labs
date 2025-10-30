using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ComponentContract;
using ComponentContract.Data;
using ComponentOprientedApp.Licensing;

namespace ComponentOprientedApp.Utils;

internal class HostServicesImpl : IHostServices
{
    private readonly ILicenseProvider _licenseProvider;
    private readonly CompanyDbContext _dbContext;

    public HostServicesImpl(AccessLevel currentAccessLevel, IConfiguration configuration)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();
        var connectionString = configuration.GetConnectionString("CompanyDb");
        optionsBuilder.UseNpgsql(connectionString);

        _dbContext = new CompanyDbContext(optionsBuilder.Options);
    }

    public ILicenseProvider License => _licenseProvider;

    public CompanyDbContext DbContext => _dbContext;

    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(ILicenseProvider))
            return _licenseProvider;
        if (serviceType == typeof(CompanyDbContext))
            return _dbContext;
        return null;
    }

    public T? GetService<T>() where T : class
    {
        var service = GetService(typeof(T));
        return service as T;
    }
}
