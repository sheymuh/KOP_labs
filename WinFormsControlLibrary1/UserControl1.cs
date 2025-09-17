using System.ComponentModel;
using System.Windows.Forms;

namespace WinFormsControlLibrary1
{
    public partial class UserControl1 : UserControl
    {
        public ComboBox.ObjectCollection Items => comboBox1.Items;

        public UserControl1()
        {
            InitializeComponent();
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        private void Clear()
        {
            comboBox1?.Items.Clear();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SelectedValue
        {
            get
            {
                return comboBox1.SelectedItem?.ToString() ?? string.Empty;
            }
            set
            {
                if (comboBox1.Items.Contains(value))
                {
                    comboBox1.SelectedItem = value;
                }
            }
        }

        public event EventHandler SelectedValueChanged;

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
