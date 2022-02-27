using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormDrivePicker : Form
    {
        private readonly FormGPS mf = null;

        public FormDrivePicker(Form callingForm, string _fileList)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            string[] fileList = _fileList.Split(',');
            for (int i = 0; i < fileList.Length; i++)
            {
                lvLines.Items.Add(new ListViewItem(fileList[i]));
            }
        }

        private void btnOpenExistingLv_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            if (count > 0)
            {
                mf.filePickerFileAndDirectory = (mf.fieldsDirectory + lvLines.SelectedItems[0].SubItems[0].Text + "\\Field.txt");
                Close();
            }
        }

        private void btnDeleteAB_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";
        }
    }
}