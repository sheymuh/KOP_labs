using Microsoft.EntityFrameworkCore;
using ComponentContract;
using ComponentContract.Data;
using ComponentContract.Entities;

namespace ReportComponent
{
    public partial class CompanyReportControl : UserControl
    {

        private readonly CompanyDbContext _dbContext;
        private readonly DataGridView _dataGridView;
        private readonly ComboBox _postComboBox;
        private readonly Button _generateButton;
        private readonly Button _exportButton;

        public CompanyReportControl(IHostServices host)
        {
            InitializeComponent();
            _dbContext = host.DbContext;

            // Создаем элементы управления
            _postComboBox = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Height = 30
            };

            _generateButton = new Button
            {
                Text = "Сформировать отчет",
                Dock = DockStyle.Top,
                Height = 40
            };

            _exportButton = new Button
            {
                Text = "Экспорт в CSV",
                Dock = DockStyle.Top,
                Height = 40,
                Enabled = false
            };

            _dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true
            };

            // Добавляем элементы на форму
            Controls.Add(_dataGridView);
            Controls.Add(_exportButton);
            Controls.Add(_generateButton);
            Controls.Add(_postComboBox);

            // Обработчики событий
            _generateButton.Click += GenerateReport_Click;
            _exportButton.Click += ExportToCsv_Click;

            // Загружаем данные
            Load += async (_, __) => await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var posts = await _dbContext.EmployeePosts.ToListAsync() ?? [];
                _postComboBox.DisplayMember = nameof(EmployeePost.Name);
                _postComboBox.ValueMember = nameof(EmployeePost.Id);
                _postComboBox.DataSource = posts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке типов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void GenerateReport_Click(object? sender, EventArgs e)
        {
            if (_postComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип подразделения", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedType = (EmployeePost)_postComboBox.SelectedItem;

                var employees = await _dbContext.Employees
                    .Include(e => e.EmployeePost)
                    .Where(e => e.EmployeePostId == selectedType.Id)
                    .OrderBy(e => e.FIO)
                    .ToListAsync();

                _dataGridView.DataSource = employees.Select(e => new
                {
                    Наименование = e.FIO,
                    Цель = e.Autobiography ?? "",
                    Тип = e.EmployeePost.Name,
                    ДатаОтчета = e.PromotionDate?.ToString("yyyy-MM-dd") ?? "",
                    Идентификатор = e.Id.ToString()
                }).ToList();

                _exportButton.Enabled = employees.Any();

                MessageBox.Show($"Найдено подразделений: {employees.Count}", "Отчет сформирован",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчета: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCsv_Click(object? sender, EventArgs e)
        {
            if (_dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var saveDialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*",
                DefaultExt = "csv",
                FileName = $"Отчет_подразделений_{DateTime.Now:yyyy-MM-dd}.csv"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var writer = new StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8);

                    // Заголовки
                    var headers = new List<string>();
                    foreach (DataGridViewColumn column in _dataGridView.Columns)
                    {
                        headers.Add(column.HeaderText);
                    }
                    writer.WriteLine(string.Join(",", headers));

                    // Данные
                    foreach (DataGridViewRow row in _dataGridView.Rows)
                    {
                        var values = new List<string>();
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            var value = cell.Value?.ToString() ?? "";
                            // Экранируем запятые и кавычки
                            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                            {
                                value = "\"" + value.Replace("\"", "\"\"") + "\"";
                            }
                            values.Add(value);
                        }
                        writer.WriteLine(string.Join(",", values));
                    }

                    MessageBox.Show($"Отчет сохранен в файл: {saveDialog.FileName}", "Экспорт завершен",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
