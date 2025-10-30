using ComponentContract;

namespace TreeListComponent;

internal class CompanyTreeListComponent : IComponentContract
{
    private static readonly IComponentMetadata _metadata =
        new ComponentMetadata(
            id: "SubdivisionDirectory",
            title: "Подразделения",
            componentType: ComponentType.List,
            requiredAccess: AccessLevel.Basic);

    public IComponentMetadata Metadata => _metadata;

    public UserControl CreateControl(IHostServices host)
        => new CompanyTreeListControl(host);
}
