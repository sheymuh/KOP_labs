namespace ComponentOprientedApp
{
    partial class ExtensionsForm
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

            comboBoxPdf = new ComboBox();
            buttonPdf = new Button();
            buttonWord = new Button();
            comboBoxWord = new ComboBox();
            buttonExcel = new Button();
            comboBoxExcel = new ComboBox();
            SuspendLayout();
            // 
            // comboBoxPdf
            // 
            comboBoxPdf.FormattingEnabled = true;
            comboBoxPdf.Items.AddRange(new object[] { "pdf" });
            comboBoxPdf.Location = new Point(32, 50);
            comboBoxPdf.Name = "comboBoxPdf";
            comboBoxPdf.Size = new Size(121, 23);
            comboBoxPdf.TabIndex = 0;
            // 
            // buttonPdf
            // 
            buttonPdf.Location = new Point(250, 50);
            buttonPdf.Name = "buttonPdf";
            buttonPdf.Size = new Size(127, 23);
            buttonPdf.TabIndex = 1;
            buttonPdf.Text = "Создать pdf";
            buttonPdf.UseVisualStyleBackColor = true;
            // 
            // buttonWord
            // 
            buttonWord.Location = new Point(250, 140);
            buttonWord.Name = "buttonWord";
            buttonWord.Size = new Size(127, 23);
            buttonWord.TabIndex = 3;
            buttonWord.Text = "Создать word";
            buttonWord.UseVisualStyleBackColor = true;
            // 
            // comboBoxWord
            // 
            comboBoxWord.FormattingEnabled = true;
            comboBoxWord.Items.AddRange(new object[] { "word" });
            comboBoxWord.Location = new Point(32, 140);
            comboBoxWord.Name = "comboBoxWord";
            comboBoxWord.Size = new Size(121, 23);
            comboBoxWord.TabIndex = 2;
            // 
            // buttonExcel
            // 
            buttonExcel.Location = new Point(250, 228);
            buttonExcel.Name = "buttonExcel";
            buttonExcel.Size = new Size(127, 23);
            buttonExcel.TabIndex = 5;
            buttonExcel.Text = "Создать excel";
            buttonExcel.UseVisualStyleBackColor = true;
            // 
            // comboBoxExcel
            // 
            comboBoxExcel.FormattingEnabled = true;
            comboBoxExcel.Items.AddRange(new object[] { "xlsx" });
            comboBoxExcel.Location = new Point(32, 228);
            comboBoxExcel.Name = "comboBoxExcel";
            comboBoxExcel.Size = new Size(121, 23);
            comboBoxExcel.TabIndex = 4;
            // 
            // ExtensionsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(413, 307);
            Controls.Add(buttonExcel);
            Controls.Add(comboBoxExcel);
            Controls.Add(buttonWord);
            Controls.Add(comboBoxWord);
            Controls.Add(buttonPdf);
            Controls.Add(comboBoxPdf);
            Name = "ExtensionsForm";
            Text = "Расширения";
            ResumeLayout(false);
        }

        #endregion

        private ComboBox comboBoxPdf;
        private Button buttonPdf;
        private Button buttonWord;
        private ComboBox comboBoxWord;
        private Button buttonExcel;
        private ComboBox comboBoxExcel;
    }
}