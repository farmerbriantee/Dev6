using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormHelp : Form
    {
        public FormHelp(string text, string caption, bool show = false)
        {
            InitializeComponent();

            label1.Text = text;

            this.Height += (text.Length / 30) * 10;
            btnReturn.Visible = show;
        }
    }
}
