using Microsoft.EntityFrameworkCore;
using ComponentContract;
using ComponentContract.Entities;
using PluginsContracts;
using ComponentOprientedApp.Composition;
using ComponentOprientedApp.Utils;
using msg = System.Windows.Forms.MessageBox;

namespace ComponentOprientedApp;

public partial class ExtensionsForm : Form
{
    private readonly DateTime _date = DateTime.Now;

    private IReadOnlyList<IReportDocumentContract>? _extensions;

    private IHostServices _hostServices;

    public ExtensionsForm(IHostServices host)
    {
        InitializeComponent();
        LoadExtensions();
        _hostServices = host;
        buttonPdf.Click += PdfClick;
        buttonWord.Click += WordClick;
        buttonExcel.Click += ExcelClick;
    }

    private void LoadExtensions()
    {
        var loader = new ExtensionLoader(
            Program.ExtensionPath ?? throw new ArgumentNullException(nameof(Program.ExtensionPath))
        );

        _extensions = loader.LoadAll();
    }

    private async Task<List<Employee>> LoadEmployeeData()
    {
        List<Employee> employees;
        employees = await _hostServices
                .DbContext.Employees
                .Include(e => e.EmployeePost)
                .AsNoTracking()
                .ToListAsync();
        return employees;
    }

    private async void PdfClick(object? sender, EventArgs e)
    {
        if (comboBoxPdf.SelectedItem is not null && comboBoxPdf.SelectedItem is "pdf")
        {
            var pdfExtension = _extensions?.First(ex => ex.DocumentFormat == "pdf");
            if (pdfExtension is IReportDocumentWithContextTextsContract pdf)
            {
                using var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    Title = "Сохранить PDF документ",
                    FileName = $"Отчет-{_date:yyyy-MM-dd_HH-mm-ss}.pdf",
                    OverwritePrompt = true,
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var data = (await LoadEmployeeData())
                        ?.Select(e => $"{e.FIO} {e.Autobiography}")
                        .ToList();
                    var filePath = saveDialog.FileName;
                    await pdf.CreateDocumentAsync(
                        filePath,
                        "Сотрудники, прошедшие квалификацию",
                        data ?? []
                    );
                    msg.Show(
                        $"Файл успешно сохранен:\n{filePath}",
                        "Готово",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            else
            {
                msg.Show("Нет расширения для pdf");
            }
        }
        else
        {
            msg.Show("Выберите формат");
        }
    }
    //private async void WordClick(object? sender, EventArgs e)
    //{
    //    if (comboBoxWord.SelectedItem == null)
    //    {
    //        msg.Show("Выберите формат Word");
    //        return;
    //    }

    //    try
    //    {
    //        Console.WriteLine("Поиск Word плагина...");
    //        var wordExtension = _extensions?.FirstOrDefault(ex =>
    //            ex is PluginsContracts.IReportDocumentWithChartPieContract);

    //        if (wordExtension is PluginsContracts.IReportDocumentWithChartPieContract wordChart)
    //        {
    //            Console.WriteLine($"Найден Word плагин: {wordChart.GetType().FullName}");

    //            using var saveDialog = new SaveFileDialog
    //            {
    //                Filter = "Docx files (*.docx)|*.docx",
    //                Title = "Сформировать Word документ",
    //                FileName = $"Отчет-{_date:yyyy-MM-dd_HH-mm-ss}.docx",
    //                OverwritePrompt = true,
    //            };

    //            if (saveDialog.ShowDialog() == DialogResult.OK)
    //            {
    //                var filePath = saveDialog.FileName;
    //                Console.WriteLine($"Путь для сохранения: {filePath}");

    //                var data = await LoadEmployeeData();
    //                Console.WriteLine($"Загружено сотрудников: {data?.Count ?? 0}");

    //                var groupedData = data?
    //                    .Where(e => !e.PromotionDate.HasValue)
    //                    .GroupBy(e => e.EmployeePost?.Name ?? "Без должности")
    //                    .Select(g => (TypeName: g.Key, Count: g.Count()))
    //                    .ToList() ?? [];

    //                Console.WriteLine($"Сгруппировано данных: {groupedData.Count}");
    //                foreach (var item in groupedData)
    //                {
    //                    Console.WriteLine($"  - {item.TypeName}: {item.Count}");
    //                }

    //                // Преобразуем данные для передачи в плагин
    //                var chartData = groupedData.Select(g => (g.TypeName, (double)g.Count)).ToList();
    //                Console.WriteLine($"Данные для диаграммы подготовлены, элементов: {chartData.Count}");

    //                Console.WriteLine("Вызов CreateDocumentAsync...");
    //                // Вызываем метод напрямую
    //                await wordChart.CreateDocumentAsync(
    //                    filePath,
    //                    "Отчет по сотрудникам", // header
    //                    "Сотрудники, не прошедшие повышение квалификации в разрезе должностей", // chartTitle
    //                    "Количество сотрудников", // seriesName
    //                    chartData // series
    //                );
    //                Console.WriteLine("CreateDocumentAsync выполнен успешно");

    //                msg.Show(
    //                    $"Файл успешно сохранен:\n{filePath}",
    //                    "Готово",
    //                    MessageBoxButtons.OK,
    //                    MessageBoxIcon.Information
    //                );
    //            }
    //        }
    //        else
    //        {
    //            Console.WriteLine("Word плагин не найден");
    //            var allExtensions = _extensions?.Select(ex => $"{ex.GetType().FullName}: {ex.DocumentFormat}").ToList();
    //            Console.WriteLine($"Доступные расширения: {string.Join(", ", allExtensions ?? new List<string>())}");

    //            msg.Show("Не найден Word плагин с интерфейсом IReportDocumentWithChartPieContract",
    //                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        }
    //    }
    //    catch (ArgumentException ex)
    //    {
    //        Console.WriteLine($"ArgumentException: {ex.Message}\nStackTrace: {ex.StackTrace}");
    //        msg.Show($"Ошибка в данных: {ex.Message}",
    //                "Ошибка данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //    }
    //    catch (InvalidOperationException ex)
    //    {
    //        Console.WriteLine($"InvalidOperationException: {ex.Message}\nStackTrace: {ex.StackTrace}");
    //        msg.Show($"Ошибка операции: {ex.Message}",
    //                "Ошибка операции", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Общее исключение: {ex.GetType().Name}: {ex.Message}\nStackTrace: {ex.StackTrace}");
    //        if (ex.InnerException != null)
    //        {
    //            Console.WriteLine($"Внутреннее исключение: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
    //        }
    //        msg.Show($"Ошибка при создании Word документа: {ex.Message}",
    //                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //    }
    //}

    private async void WordClick(object? sender, EventArgs e)
    {
        if (comboBoxWord.SelectedItem is not null && comboBoxWord.SelectedItem is "word")
        {
            var wordExtension = _extensions?.First(e => e.DocumentFormat == "Word");
            if (wordExtension is not null)
            {
                using var saveDialog = new SaveFileDialog
                {
                    Filter = "Docx files (*.docx)|*.docx",
                    Title = "Сформировать Word документ",
                    FileName = $"Отчет-{_date:yyyy-MM-dd_HH-mm-ss}.docx",
                    OverwritePrompt = true,
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = saveDialog.FileName;

                    int year = DateTime.Now.Year;

                    var data = await LoadEmployeeData();

                    var groupedData = data?
                        .Where(e => !e.PromotionDate.HasValue)
                        .GroupBy(e => e.EmployeePost.Name)
                        .Select(g => (g.Key, g.Count()))
                        .ToList() ?? [];

                    await ReportEmployeePieBuilder.Build(filePath, "Заголовок", groupedData, _date, wordExtension);

                    msg.Show(
                        $"Файл успешно сохранен:\n{filePath}",
                        "Готово",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            else
            {
                msg.Show("Нет расширения для word");
            }
        }
        else
        {
            msg.Show("Выберите формат");
        }
    }

    private async void ExcelClick(object? sender, EventArgs e)
    {
        msg.Show("Not implemented");
    }
}