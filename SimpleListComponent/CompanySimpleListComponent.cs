using ComponentContract;

namespace SimpleListComponent;

public sealed class CompanySimpleListComponent : IComponentContract
{
    private static readonly IComponentMetadata _metadata =
        new ComponentMetadata(
            id: "SimpleListComponent",
            title: "Справочник должностей сотрудников",
            componentType: ComponentType.List,
            requiredAccess: AccessLevel.Minimal);

    public IComponentMetadata Metadata => _metadata;

    public UserControl CreateControl(IHostServices hostServices)
        => new CompanySimpleListControl(hostServices);
}
