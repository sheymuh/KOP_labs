using System.Reflection;
using PluginsContracts;
using OxyPlot;
using OxyPlot.Series;

namespace ComponentOprientedApp.Utils;

internal class ReportEmployeePieBuilder
{
    /// <summary>
    /// Word отчет
    /// </summary>
    public static async Task Build(
        string filePath,
        string header,
        List<(string TypeName, int Count)> subdivisions,
        DateTime nowDate,
        IReportDocumentContract reportContract
    )
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));
        if (subdivisions is null || subdivisions.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(subdivisions));

        int year = nowDate.Year;

        if (reportContract is not null)
        {
            Type implType = reportContract.GetType();

            var createMethod =
                implType.GetMethod(
                    "CreateDocumentAsync",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                )
                ?? throw new InvalidOperationException(
                    "Метод CreateDocumentAsync не найден в реализации."
                );

            var seriesData = subdivisions
                .Select(g => (Parameter: g.TypeName, Value: (double)g.Count))
                .ToList();

            //var chartImage = GeneratePieChartImage(seriesData);

            //var taskObj = (Task)
            //    createMethod?.Invoke(
            //        reportContract,
            //        [
            //            filePath,
            //            header,
            //            new List<byte[]> { chartImage }
            //        ]
            //    );
            var taskObj = (Task)createMethod.Invoke(
                reportContract,
                new object[]
                {
                    filePath,                           // string
                    header,                             // string
                    "Сотрудники, не прошедшие повышение квалификации в разрезе должностей",         // chartTitle: string
                    "Количество сотрудников",           // seriesName: string  
                    subdivisions.Select(g => (g.TypeName, (double)g.Count)).ToList() // List<(string, double)>
                }
            );

            taskObj ??= Task.CompletedTask;
            await taskObj;
        }
    }

    //private static byte[] GeneratePieChartImage(List<(string Parameter, double Value)> data, int width = 800, int height = 600)
    //{
    //    if (data == null || data.Count == 0)
    //        throw new ArgumentException("Data cannot be null or empty", nameof(data));

    //    var plotModel = new PlotModel { Title = "Сотрудники, не прошедшие повышение квалификации в разрезе должностей" };

    //    var pieSeries = new PieSeries
    //    {
    //        StrokeThickness = 2.0,
    //        InsideLabelPosition = 0.8,
    //        AngleSpan = 360,
    //        StartAngle = 0,
    //        OutsideLabelFormat = "{0}: {1}",
    //        InsideLabelFormat = "{1}"
    //    };

    //    foreach (var item in data)
    //    {
    //        pieSeries.Slices.Add(new PieSlice(item.Parameter, item.Value));
    //    }

    //    plotModel.Series.Add(pieSeries);

    //    var pngExporter = new OxyPlot.ImageSharp.PngExporter(width, height);
    //    using var stream = new MemoryStream();
    //    pngExporter.Export(plotModel, stream);
    //    return stream.ToArray();
    //}
}
