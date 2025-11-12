namespace SimpleListComponent
{
    partial class CompanySimpleListControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridViewCustom = new DataGridView();
            NameColumn = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCustom).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewCustom
            // 
            dataGridViewCustom.AllowUserToAddRows = false;
            dataGridViewCustom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCustom.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCustom.Columns.AddRange(new DataGridViewColumn[] { NameColumn });
            dataGridViewCustom.Dock = DockStyle.Fill;
            dataGridViewCustom.Location = new Point(0, 0);
            dataGridViewCustom.Name = "dataGridViewCustom";
            dataGridViewCustom.RowHeadersWidth = 72;
            dataGridViewCustom.Size = new Size(1044, 542);
            dataGridViewCustom.TabIndex = 0;
            // 
            // NameColumn
            // 
            NameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            NameColumn.DataPropertyName = "Name";
            NameColumn.HeaderText = "Должность работника";
            NameColumn.MinimumWidth = 9;
            NameColumn.Name = "NameColumn";
            // 
            // CompanySimpleListControl
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dataGridViewCustom);
            Name = "CompanySimpleListControl";
            Size = new Size(1044, 542);
            ((System.ComponentModel.ISupportInitialize)dataGridViewCustom).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewCustom;
        private DataGridViewTextBoxColumn NameColumn;
    }
}
