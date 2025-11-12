using Microsoft.EntityFrameworkCore;
using ComponentContract;
using ComponentContract.Data;
using ComponentContract.Entities;
using System.ComponentModel;

namespace SimpleListComponent
{
    public partial class CompanySimpleListControl : UserControl
    {
        private readonly IHostServices _host;
        private readonly CompanyDbContext _dbContext;
        private readonly BindingSource _bindingSource = new BindingSource();

        public CompanySimpleListControl(IHostServices host)
        {
            InitializeComponent();
            _host = host;
            _dbContext = host.DbContext;
            // Проверяем подключение при создании контрола
            // _ = CheckTableExists();
            Load += async (_, __) => await LoadDataAsync();
            dataGridViewCustom.DataSource = _bindingSource;

            dataGridViewCustom.KeyDown += DataGridViewCustom_OnKeyDown;
            dataGridViewCustom.CellEndEdit += DataGridView_CellEndEdit;
        }

        private async Task CheckTableExists()
        {
            // Создаем новый экземпляр DbContext для проверки
            var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=company_db;Username=postgres;Password=postgres");

            using var checkContext = new CompanyDbContext(optionsBuilder.Options);

            try
            {
                var canConnect = await checkContext.Database.CanConnectAsync();

                if (canConnect)
                {
                    var postsExist = await checkContext.EmployeePosts.IgnoreQueryFilters().AnyAsync();
                    var employeesExist = await checkContext.Employees.IgnoreQueryFilters().AnyAsync();

                    MessageBox.Show($"База данных подключена\nТаблица должностей: {postsExist}\nТаблица сотрудников: {employeesExist}");
                }
                else
                {
                    MessageBox.Show("Не удалось подключиться к базе данных");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void DataGridViewCustom_OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert)
            {
                // Создаём новый объект с временным именем
                var newType = new EmployeePost { Id = Guid.NewGuid(), Name = "Новая запись" };
                _bindingSource.Add(newType);

                BeginInvoke(() =>
                {
                    if (dataGridViewCustom.Rows.Count > 0)
                    {
                        dataGridViewCustom.CurrentCell = dataGridViewCustom.Rows[^1].Cells["NameColumn"];
                        dataGridViewCustom.BeginEdit(true);
                    }
                });

                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && dataGridViewCustom.CurrentRow is not null)
            {
                if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (_bindingSource.Current is EmployeePost current)
                    {
                        // Проверяем, существует ли объект в базе
                        var entityInDb = await _dbContext.EmployeePosts.FirstOrDefaultAsync(x => x.Id == current.Id);
                        if (entityInDb != null)
                        {
                            _dbContext.EmployeePosts.Remove(entityInDb);
                            await _dbContext.SaveChangesAsync();
                        }

                        _bindingSource.RemoveCurrent();
                    }
                }
                e.Handled = true;
            }
        }

        private async void DataGridView_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCustom.Rows[e.RowIndex].DataBoundItem is not EmployeePost subDiv)
                return;

            if (string.IsNullOrWhiteSpace(subDiv.Name))
            {
                dataGridViewCustom.Rows[e.RowIndex].ErrorText = "Пустая строка не допускается";
                dataGridViewCustom.CancelEdit();
                return;
            }

            dataGridViewCustom.Rows[e.RowIndex].ErrorText = null;

            try
            {
                if (_dbContext.Entry(subDiv).State == EntityState.Detached)
                {
                    _dbContext.EmployeePosts.Add(subDiv);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var types = await _dbContext.EmployeePosts.ToListAsync();
                _bindingSource.DataSource = new BindingList<EmployeePost>(types);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
