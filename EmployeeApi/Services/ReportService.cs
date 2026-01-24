using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SkiaSharp;
using Drawing = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using Word = DocumentFormat.OpenXml.Wordprocessing;


namespace EmployeeApi.Services;

public static class ReportService
{
    public static byte[] GeneratePdfReport(
        IEnumerable<Entities.Employee> employees,
        bool includeDeleted = false
    )
    {
        var promoted = employees
            .Where(s => s.PromotionDate != null && (includeDeleted || !s.IsDeleted))
            .ToList();
        QuestPDF.Settings.License = LicenseType.Community;
        using var ms = new MemoryStream();

        Document
            .Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Header()
                        .Text("Сотрудники, проходившие квалификацию")
                        .SemiBold()
                        .FontSize(16);
                    page.Content()
                        .Column(col =>
                        {
                            foreach (var e in promoted)
                            {
                                col.Item().Text($"{e.FIO} — {e.Autobiography ?? "Автобиография не указана"}");
                            }
                        });
                });
            })
            .GeneratePdf(ms);

        return ms.ToArray();
    }

    public static byte[] GenerateWordReport(
        IEnumerable<Entities.Employee> employees,
        bool includeDeleted = false
    )
    {
        var notPromoted = employees
        .Where(e =>
            e.PromotionDate == null
            && (includeDeleted || !e.IsDeleted)
            && e.EmployeePost != null
        )
        .GroupBy(e => e.EmployeePost!.Name)
        .Select(g => new { Type = g.Key, Count = g.Count() })
        .ToList();

        var fontFamilies = SKFontManager.Default.FontFamilies;
        Console.WriteLine($"Available fonts: {string.Join(", ", fontFamilies)}");

        if (notPromoted.Count == 0)
        {
            using var emptyDocStream = new MemoryStream();
            using (
                var emptyDoc = WordprocessingDocument.Create(
                    emptyDocStream,
                    WordprocessingDocumentType.Document,
                    true
                )
            )
            {
                var mainPart = emptyDoc.AddMainDocumentPart();
                mainPart.Document = new Word.Document(
                    new Word.Body(
                        new Word.Paragraph(
                            new Word.Run(new Word.Text("Данные для отчёта отсутствуют."))
                        ),
                        new Word.SectionProperties(new Word.PageSize(), new Word.PageMargin())
                    )
                );
                mainPart.Document.Save();
            }

            return emptyDocStream.ToArray();
        }

        QuestPDF.Settings.License = LicenseType.Community;

        // Увеличиваем размер для размещения текста
        int width = 600, height = 700;
        using var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        float total = notPromoted.Sum(r => r.Count);
        float startAngle = 0f;
        var colors = new SKColor[]
    {
        SKColors.Red,
        SKColors.Green,
        SKColors.Blue,
        SKColors.Orange,
        SKColors.Purple,
        SKColors.Teal,
        SKColors.Brown,
        SKColors.Pink,
        SKColors.Gray,
    };

        // Создаем шрифты с использованием нового API
        // Пробуем разные способы получения шрифта для работы в Docker
        var defaultTypeface =
            SKTypeface.FromFamilyName("DejaVu Sans", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright)
            ?? SKTypeface.Default
            ?? SKTypeface.CreateDefault()
            ?? SKTypeface.FromFamilyName(null)
            ?? SKTypeface.FromFamilyName("Arial")
            ?? SKTypeface.FromFamilyName("DejaVu Sans");

        defaultTypeface ??= SKTypeface.CreateDefault();

        using var titleFont = new SKFont(defaultTypeface, 24);
        using var boldFont = new SKFont(defaultTypeface, 16);
        using var legendFont = new SKFont(defaultTypeface, 14);
        using var centerFont = new SKFont(defaultTypeface, 18);

        // Заголовок диаграммы
        using var titlePaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true
        };
        canvas.DrawText("Статистика по сотрудникам", width / 2f, 40, SKTextAlign.Center, titleFont, titlePaint);

        // Отрисовка круговой диаграммы (с отступом сверху для заголовка)
        var chartRect = new SKRect(50, 80, width - 50, height - 150);
        for (int i = 0; i < notPromoted.Count; i++)
        {
            float sweep = total > 0 ? 360f * notPromoted[i].Count / total : 0f;
            using var paint = new SKPaint
            {
                Color = colors[i % colors.Length],
                IsAntialias = true,
            };
            canvas.DrawArc(chartRect, startAngle, sweep, true, paint);
            startAngle += sweep;
        }

        // Легенда внизу диаграммы
        using var legendPaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true
        };

        using var boldPaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true
        };

        // Заголовок легенды
        canvas.DrawText("Легенда:", width / 2f, height - 130, SKTextAlign.Center, boldFont, boldPaint);

        // Элементы легенды
        int legendX = 50;
        int legendY = height - 100;
        int colorBoxSize = 12;
        int itemsPerRow = 2; // Количество элементов в строке
        int itemWidth = (width - 100) / itemsPerRow;

        for (int i = 0; i < notPromoted.Count; i++)
        {
            var color = colors[i % colors.Length];
            var row = i / itemsPerRow;
            var col = i % itemsPerRow;

            int x = legendX + col * itemWidth;
            int y = legendY + row * 25;

            // Цветной квадратик
            using var colorPaint = new SKPaint { Color = color };
            canvas.DrawRect(x, y - colorBoxSize, colorBoxSize, colorBoxSize, colorPaint);

            // Текст легенды
            string legendText =
            $"{notPromoted[i].Type}: {notPromoted[i].Count} ({((notPromoted[i].Count / total) * 100):F1}%)";
            canvas.DrawText(legendText, x + colorBoxSize + 5, y, SKTextAlign.Left, legendFont, legendPaint);
        }

        // Общее количество в центре диаграммы
        using var centerPaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true
        };

        string totalText = $"Всего: {total}";
        canvas.DrawText(totalText, chartRect.MidX, chartRect.MidY, SKTextAlign.Center, centerFont, centerPaint);

        using var imgStream = new MemoryStream();
        bitmap.Encode(imgStream, SKEncodedImageFormat.Png, 100);
        imgStream.Position = 0;

        using var docStream = new MemoryStream();
        using (
            var doc = WordprocessingDocument.Create(
                docStream,
                WordprocessingDocumentType.Document,
                true
            )
        )
        {
            var mainPart = doc.AddMainDocumentPart();
            mainPart.Document = new Word.Document(new Word.Body());

            // Заголовок по центру в Word-документе
            var titleParagraph = new Word.Paragraph(
                new Word.ParagraphProperties(
                    new Word.Justification { Val = Word.JustificationValues.Center }
                ),
                new Word.Run(
                    new Word.RunProperties(
                        new Word.Bold(),
                        new Word.FontSize { Val = "28" }
                    ),
                    new Word.Text("Статистика по сотрудникам, не прошедшим квалификацию")
                )
            );
            mainPart.Document.Body!.Append(titleParagraph);

            // Пустая строка после заголовка
            mainPart.Document.Body.Append(new Word.Paragraph());

            var imagePart = mainPart.AddImagePart(ImagePartType.Png);
            imagePart.FeedData(imgStream);
            var imageId = mainPart.GetIdOfPart(imagePart);

            // Размер 14×14 см в EMU
            long widthEmu = 14 * 360000L;
            long heightEmu = 14 * 360000L;

            var drawingElement = new Word.Drawing(
            new DW.Inline(
                new DW.Extent { Cx = widthEmu, Cy = heightEmu },
                new DW.EffectExtent
                {
                    LeftEdge = 0L,
                    TopEdge = 0L,
                    RightEdge = 0L,
                    BottomEdge = 0L,
                },
                new DW.DocProperties { Id = (UInt32Value)1U, Name = "PieChart" },
                new DW.NonVisualGraphicFrameDrawingProperties(
                    new Drawing.GraphicFrameLocks { NoChangeAspect = true }
                ),
                new Drawing.Graphic(
                    new Drawing.GraphicData(
                        new PIC.Picture(
                            new PIC.NonVisualPictureProperties(
                                new PIC.NonVisualDrawingProperties
                                {
                                    Id = (UInt32Value)0U,
                                    Name = "Chart",
                                },
                                new PIC.NonVisualPictureDrawingProperties()
                            ),
                            new PIC.BlipFill(
                                new Drawing.Blip { Embed = imageId },
                                new Drawing.Stretch(new Drawing.FillRectangle())
                            ),
                            new PIC.ShapeProperties(
                                new Drawing.Transform2D(
                                    new Drawing.Offset { X = 0L, Y = 0L },
                                    new Drawing.Extents { Cx = widthEmu, Cy = heightEmu }
                                ),
                                new Drawing.PresetGeometry(new Drawing.AdjustValueList())
                                {
                                    Preset = Drawing.ShapeTypeValues.Rectangle,
                                }
                            )
                        )
                    )
                    {
                        Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture",
                    }
                )
            )
        );

            // Параграф с выравниванием по центру для картинки
            var imageParagraph = new Word.Paragraph(
            new Word.ParagraphProperties(
                new Word.Justification { Val = Word.JustificationValues.Center }
            ),
            new Word.Run(drawingElement)
        );
            mainPart.Document.Body!.Append(imageParagraph);

            mainPart.Document.Body.AppendChild(
                new Word.SectionProperties(new Word.PageSize(), new Word.PageMargin())
            );
            mainPart.Document.Save();
        }

        docStream.Position = 0;
        return docStream.ToArray();
    }
}
