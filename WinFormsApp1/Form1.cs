namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadList();
            LoadTextBox();
        }

        public void LoadTextBox()
        {
            userControl21.Pattern = @"^(8|\+7)(\d{10})$";
            userControl21.PhoneNumber = "+79896365423";
            userControl21.ChangeText += () => MessageBox.Show("Number changed", "", MessageBoxButtons.OK, MessageBoxIcon.None);
            try
            {
                string number = userControl21.PhoneNumber;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        public void LoadList()
        {
            userControl11.Items.Add("лесочек");
            userControl11.Items.Add("опушечка");
            userControl11.Items.Add("подберёзовичек");
            userControl11.Items.Add("опёнок");
        }
    }
}
