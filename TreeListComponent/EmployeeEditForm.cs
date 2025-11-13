using ComponentContract;
using ComponentContract.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;


namespace TreeListComponent
{
    public partial class EmployeeEditForm : Form
    {
        private readonly IHostServices _host;
        private readonly Employee _model;
        private bool _dirty;

        public EmployeeEditForm(IHostServices host, Employee model)
        {
            InitializeComponent();
            _host = host;
            _model = model;

            Load += async (_, __) => await InitAsync();
            FormClosing += OnFormClosing;
        }

        private async Task InitAsync()
        {
            var types = await _host.DbContext.EmployeePosts.ToListAsync() ?? [];

            if (cmbPost != null)
            {
                cmbPost.DisplayMember = nameof(EmployeePost.Name);
                cmbPost.ValueMember = nameof(EmployeePost.Id);
                cmbPost.DataSource = types;
                cmbPost.DropDownStyle = ComboBoxStyle.DropDownList;

                var match = types.FirstOrDefault(t => t.Id == _model.EmployeePostId);
                if (match != null)
                {
                    cmbPost.SelectedItem = match;
                }
                else
                {
                    // Если нет — сбрасываем выбор
                    cmbPost.SelectedIndex = -1;
                }
            }


            if (txtFIO != null)
                txtFIO.Text = _model.FIO;
            if (txtAutobiography != null)
                txtAutobiography.Text = _model.Autobiography ?? string.Empty;
            if (dtPromotionDate != null && _model.PromotionDate.HasValue)
            {
                dtPromotionDate.Value = _model.PromotionDate.Value;
            }

            if (txtFIO != null)
                txtFIO.TextChanged += (_, __) => _dirty = true;
            if (txtAutobiography != null)
                txtAutobiography.TextChanged += (_, __) => _dirty = true;
            if (cmbPost != null)
                cmbPost.SelectedIndexChanged += (_, __) => _dirty = true;
            if (dtPromotionDate != null)
                dtPromotionDate.ValueChanged += (_, __) => _dirty = true;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            AcceptButton = Controls.OfType<Button>().FirstOrDefault(b => b.DialogResult == DialogResult.OK);
            CancelButton = Controls.OfType<Button>().FirstOrDefault(b => b.DialogResult == DialogResult.Cancel);
        }

        private void OnFormClosing(object? sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                // Перенос значений в модель
                if (txtFIO != null)
                    _model.FIO = txtFIO.Text.Trim();

                if (txtAutobiography != null)
                    _model.Autobiography = string.IsNullOrWhiteSpace(txtAutobiography.Text) ? null : txtAutobiography.Text.Trim();

                if (cmbPost != null)
                {
                    if (cmbPost.SelectedIndex >= 0 && cmbPost.SelectedValue is EmployeePost subType)
                    {
                        _model.EmployeePostId = subType.Id;
                    }
                    else
                    {
                        MessageBox.Show("Нужно выбрать должность", "Валидация",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }
                }

                if (dtPromotionDate != null)
                {
                    var noDate = Controls.OfType<CheckBox>().FirstOrDefault(c => c.Name == "chkNoDate");
                    if (noDate is not null && noDate.Checked)
                    {
                        _model.PromotionDate = null;
                    }
                    else
                    {
                        var d = dtPromotionDate.Value.Date;
                        _model.PromotionDate = DateTime.SpecifyKind(d, DateTimeKind.Local);
                    }
                }

                return;
            }

            if (_dirty)
            {
                var res = MessageBox.Show(
                    "Есть несохранённые изменения. Закрыть без сохранения?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (res != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
