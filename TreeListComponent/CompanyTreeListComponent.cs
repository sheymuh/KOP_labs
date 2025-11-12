using ComponentContract;
using System.Reflection;

namespace TreeListComponent;

internal class CompanyTreeListComponent : IComponentContract
{
    private static readonly IComponentMetadata _metadata =
        new ComponentMetadata(
            id: "TreeListComponent",
            title: "Сотрудники",
            componentType: ComponentType.List,
            requiredAccess: AccessLevel.Basic);

    public IComponentMetadata Metadata => _metadata;

    public UserControl CreateControl(IHostServices host)
    {
        try
        {
            // Проверяем загрузку сборки
            var assembly = Assembly.Load("ControlsLibraryNet90");
            Console.WriteLine($"Assembly loaded from: {assembly.Location}");

            return new CompanyTreeListControl(host);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading assembly: {ex}");
            throw;
        }
    }
}
