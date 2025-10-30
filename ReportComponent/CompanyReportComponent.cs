using ComponentContract;

namespace ReportComponent;

public class CompanyReportComponent : IComponentContract
{
    private static readonly IComponentMetadata _metadata =
        new ComponentMetadata(
            id: "SubdivisionReport",
            title: "Отчет по подразделениям",
            componentType: ComponentType.Report,
            requiredAccess: AccessLevel.Advanced);

    public IComponentMetadata Metadata => _metadata;

    public UserControl CreateControl(IHostServices host)
        => new CompanyReportControl(host);
}
