namespace ComponentContract;

public interface IHostServices
{
    ILicenseProvider License { get; }
    OrganisationDbContext DbContext { get; }
    object? GetService(Type serviceType);
    T? GetService<T>() where T : class;
}
