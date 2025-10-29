namespace ComponentContract;

public interface ILicenseProvider
{
    AccessLevel CurrentLevel { get; }
    bool IsExpired { get; }
}
