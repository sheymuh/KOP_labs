namespace ComponentOprientedApp
{
    partial class LoginForm
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
            textBoxLogin = new TextBox();
            buttonSubmit = new Button();
            labelMessage = new Label();
            SuspendLayout();
            // 
            // textBoxLogin
            // 
            textBoxLogin.Location = new Point(12, 64);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(211, 23);
            textBoxLogin.TabIndex = 0;
            // 
            // buttonSubmit
            // 
            buttonSubmit.Location = new Point(12, 115);
            buttonSubmit.Name = "buttonSubmit";
            buttonSubmit.Size = new Size(211, 33);
            buttonSubmit.TabIndex = 1;
            buttonSubmit.Text = "Ок";
            buttonSubmit.UseVisualStyleBackColor = true;
            // 
            // labelMessage
            // 
            labelMessage.AutoSize = true;
            labelMessage.Location = new Point(60, 33);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new Size(112, 15);
            labelMessage.TabIndex = 2;
            labelMessage.Text = "Введите ваш логин";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(235, 174);
            Controls.Add(labelMessage);
            Controls.Add(buttonSubmit);
            Controls.Add(textBoxLogin);
            Name = "LoginForm";
            Text = "Вход";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxLogin;
        private Button buttonSubmit;
        private Label labelMessage;
    }
}