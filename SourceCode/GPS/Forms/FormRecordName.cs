using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS.Forms
{
    public partial class FormRecordName : Form
    {
        //class variables
        private readonly FormGPS mf = null;
        public string filename = string.Empty;

        public FormRecordName(Form _callingForm)
        {
            //get copy of the calling main form
            mf = _callingForm as FormGPS;

            InitializeComponent();
            btnSave.Enabled = false;
            lblFilename.Text = "";
        }

        private void tboxFieldName_TextChanged(object sender, EventArgs e)
        {
            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            if (string.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }

            lblFilename.Text = tboxFieldName.Text.Trim();
            if (checkBoxRecordAddDate.Checked) lblFilename.Text += " " + DateTime.Now.ToString("MMM.dd", CultureInfo.InvariantCulture);
            if (checkBoxRecordAddTime.Checked) lblFilename.Text += " " + DateTime.Now.ToString("HH_mm", CultureInfo.InvariantCulture);
            filename = lblFilename.Text;
        }

        private void tboxFieldName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
            }
        }
    }
}
