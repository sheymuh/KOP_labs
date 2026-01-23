using static System.String;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace PluginLargeTextPdf;

internal class PdfBuilder
{
    public static void Build(string filePath, string header, List<string> paragraphs)
    {
        if (IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));
        if (IsNullOrWhiteSpace(header))
            throw new ArgumentNullException(nameof(header));
        if (paragraphs == null || paragraphs.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(paragraphs));

        var doc = new Document();
        var section = doc.AddSection();

        // Заголовок
        var title = section.AddParagraph(header);
        title.Format.Font.Size = 16;
        title.Format.Font.Bold = true;
        title.Format.SpaceAfter = "1cm";
        title.Format.Alignment = ParagraphAlignment.Center;

        // Основной текст
        foreach (var text in paragraphs)
        {
            if (IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(paragraphs), "Один из абзацев пуст.");

            var p = section.AddParagraph(text);
            p.Format.Font.Size = 12;
            p.Format.SpaceAfter = "0.4cm";
            p.Format.Alignment = ParagraphAlignment.Left;
        }

        var renderer = new PdfDocumentRenderer { Document = doc };
        renderer.RenderDocument();

        var dir = Path.GetDirectoryName(filePath);
        if (!IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        renderer.PdfDocument.Save(filePath);
    }
}
