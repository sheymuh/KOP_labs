using System.ComponentModel;
using System.Text.RegularExpressions;
using WinFormsControlLibrary1.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WinFormsControlLibrary1
{
    public partial class UserControl2 : UserControl
    {
        public event Action? ChangeText;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Pattern { get; set; } = string.Empty;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PhoneExample { get; private set; } = string.Empty;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PhoneNumber
        {
            get
            {
                string number = textBoxPhoneNumber.Text;

                if (string.IsNullOrEmpty(Pattern)) throw new PatternIsNullOrEmptyException();
                
                if (!Regex.IsMatch(number, Pattern)) throw new IncorrectStringException();

                return number;
            }
            set
            {
                string number = value;

                if (string.IsNullOrEmpty(Pattern)) throw new PatternIsNullOrEmptyException();

                if (!Regex.IsMatch(number, Pattern)) throw new IncorrectStringException();

                textBoxPhoneNumber.Text = value;
            }
        }
        
        private ToolTip _textFieldToolTip = new ToolTip();

        public UserControl2()
        {
            InitializeComponent();
            SetPhoneExample("8(+7)9876543210");
            _textFieldToolTip.SetToolTip(textBoxPhoneNumber, PhoneExample);
            textBoxPhoneNumber.TextChanged += (object? sender, EventArgs e) => ChangeText?.Invoke();
        }

        public void SetPhoneExample(string example)
        {
            if (!string.IsNullOrEmpty(example)) PhoneExample = example;
        }
    }
}
