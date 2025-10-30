namespace TreeListComponent
{
    partial class EmployeeEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

            lblFIO = new Label();
            txtFIO = new TextBox();
            lblAutobiography = new Label();
            txtAutobiography = new TextBox();
            lblPost = new Label();
            cmbPost = new ComboBox();
            lblPromotionDate = new Label();
            dtPromotionDate = new DateTimePicker();
            chkNoDate = new CheckBox();
            btnOk = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblFIO
            // 
            lblFIO.AutoSize = true;
            lblFIO.Location = new Point(20, 20);
            lblFIO.Name = "lblFIO";
            lblFIO.Size = new Size(92, 15);
            lblFIO.TabIndex = 0;
            lblFIO.Text = "ФИО:";
            // 
            // txtFIO
            // 
            txtFIO.Location = new Point(150, 17);
            txtFIO.Name = "txtFIO";
            txtFIO.Size = new Size(380, 23);
            txtFIO.TabIndex = 1;
            // 
            // lblAutobiography
            // 
            lblAutobiography.AutoSize = true;
            lblAutobiography.Location = new Point(20, 60);
            lblAutobiography.Name = "lblAutobiography";
            lblAutobiography.Size = new Size(87, 15);
            lblAutobiography.TabIndex = 2;
            lblAutobiography.Text = "Автобиография:";
            // 
            // txtAutobiography
            // 
            txtAutobiography.Location = new Point(150, 57);
            txtAutobiography.Multiline = true;
            txtAutobiography.Name = "txtAutobiography";
            txtAutobiography.Size = new Size(380, 80);
            txtAutobiography.TabIndex = 3;
            // 
            // lblPost
            // 
            lblPost.AutoSize = true;
            lblPost.Location = new Point(20, 155);
            lblPost.Name = "lblPost";
            lblPost.Size = new Size(111, 15);
            lblPost.TabIndex = 4;
            lblPost.Text = "Должность:";
            // 
            // cmbPost
            // 
            cmbPost.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPost.FormattingEnabled = true;
            cmbPost.Location = new Point(150, 152);
            cmbPost.Name = "cmbPost";
            cmbPost.Size = new Size(380, 23);
            cmbPost.TabIndex = 5;
            // 
            // lblPromotionDate
            // 
            lblPromotionDate.AutoSize = true;
            lblPromotionDate.Location = new Point(20, 195);
            lblPromotionDate.Name = "lblPromotionDate";
            lblPromotionDate.Size = new Size(81, 15);
            lblPromotionDate.TabIndex = 6;
            lblPromotionDate.Text = "Дата повышения:";
            // 
            // dtPromotionDate
            // 
            dtPromotionDate.Format = DateTimePickerFormat.Short;
            dtPromotionDate.Location = new Point(150, 192);
            dtPromotionDate.Name = "dtPromotionDate";
            dtPromotionDate.Size = new Size(150, 23);
            dtPromotionDate.TabIndex = 7;
            // 
            // chkNoDate
            // 
            chkNoDate.AutoSize = true;
            chkNoDate.Location = new Point(320, 194);
            chkNoDate.Name = "chkNoDate";
            chkNoDate.Size = new Size(79, 19);
            chkNoDate.TabIndex = 8;
            chkNoDate.Text = "Нет даты";
            chkNoDate.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(374, 240);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 27);
            btnOk.TabIndex = 9;
            btnOk.Text = "ОК";
            btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(455, 240);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 27);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // EmployeeEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(560, 280);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(chkNoDate);
            Controls.Add(dtPromotionDate);
            Controls.Add(lblPromotionDate);
            Controls.Add(cmbPost);
            Controls.Add(lblPost);
            Controls.Add(txtAutobiography);
            Controls.Add(lblAutobiography);
            Controls.Add(txtFIO);
            Controls.Add(lblFIO);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EmployeeEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Подразделение";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFIO;
        private TextBox txtFIO;
        private Label lblAutobiography;
        private TextBox txtAutobiography;
        private Label lblPost;
        private ComboBox cmbPost;
        private Label lblPromotionDate;
        private DateTimePicker dtPromotionDate;
        private CheckBox chkNoDate;
        private Button btnOk;
        private Button btnCancel;
    }
}