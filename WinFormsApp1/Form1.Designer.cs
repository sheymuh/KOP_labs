namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            userControl11 = new WinFormsControlLibrary1.UserControl1();
            userControl21 = new WinFormsControlLibrary1.UserControl2();
            labelComponent1 = new Label();
            labelComponent2 = new Label();
            SuspendLayout();
            // 
            // userControl11
            // 
            userControl11.Location = new Point(12, 56);
            userControl11.Name = "userControl11";
            userControl11.SelectedValue = "";
            userControl11.Size = new Size(518, 112);
            userControl11.TabIndex = 0;
            // 
            // userControl21
            // 
            userControl21.Location = new Point(12, 204);
            userControl21.Name = "userControl21";
            userControl21.Size = new Size(518, 123);
            userControl21.TabIndex = 1;
            // 
            // labelComponent1
            // 
            labelComponent1.AutoSize = true;
            labelComponent1.Location = new Point(30, 23);
            labelComponent1.Name = "labelComponent1";
            labelComponent1.Size = new Size(137, 30);
            labelComponent1.TabIndex = 2;
            labelComponent1.Text = "Компонент 1";
            // 
            // labelComponent2
            // 
            labelComponent2.AutoSize = true;
            labelComponent2.Location = new Point(30, 171);
            labelComponent2.Name = "labelComponent2";
            labelComponent2.Size = new Size(137, 30);
            labelComponent2.TabIndex = 3;
            labelComponent2.Text = "Компонент 2";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(542, 330);
            Controls.Add(labelComponent2);
            Controls.Add(labelComponent1);
            Controls.Add(userControl21);
            Controls.Add(userControl11);
            Name = "Form1";
            Text = "Пользовательские компоненты";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private WinFormsControlLibrary1.UserControl1 userControl11;
        private WinFormsControlLibrary1.UserControl2 userControl21;
        private Label labelComponent1;
        private Label labelComponent2;
    }
}
