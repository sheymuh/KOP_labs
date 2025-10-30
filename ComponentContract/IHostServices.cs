using ComponentContract.Data;

namespace ComponentContract;

public interface IHostServices
{
    ILicenseProvider License { get; }
    CompanyDbContext DbContext { get; }
    object? GetService(Type serviceType);
    T? GetService<T>() where T : class;
}
