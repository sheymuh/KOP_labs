using PluginsContracts;

namespace PluginLargeTextPdf;

internal class LargeTextPdf : IReportDocumentWithContextTextsContract
{
    public string DocumentFormat => "pdf";

    public async Task CreateDocumentAsync(string filePath, string header, List<string> paragraphs)
    {
        await Task.Run(() => PdfBuilder.Build(filePath, header, paragraphs));
    }
}
