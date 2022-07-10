using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormJob : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormJob(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            btnJobOpen.Text = gStr.gsOpen;
            btnJobNew.Text = gStr.gsNew;
            btnJobResume.Text = gStr.gsResume;

            this.Text = gStr.gsStartNewField;
        }

        private void btnJobNew_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnJobResume_Click(object sender, EventArgs e)
        {
            mf.FileOpenField();

            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormJob_Load(object sender, EventArgs e)
        {
            //check if directory and file exists, maybe was deleted etc
            if (String.IsNullOrEmpty(mf.currentFieldDirectory)) btnJobResume.Enabled = false;
            string directoryName = mf.fieldsDirectory + mf.currentFieldDirectory + "\\";

            string fileAndDirectory = directoryName + "Field.txt";

            if (!File.Exists(fileAndDirectory))
            {
                lblFieldName.Text = "";
                btnJobResume.Enabled = false;
                mf.currentFieldDirectory = "";


                Properties.Settings.Default.setF_CurrentDir = "";
                Properties.Settings.Default.Save();
            }
            else
            {
                lblFieldName.Text = mf.currentFieldDirectory;
            }

            mf.CloseTopMosts();
        }

        private void btnJobOpen_Click(object sender, EventArgs e)
        {
            using (FormFilePicker form = new FormFilePicker(mf, 0, ""))
            {
                //returns full field.txt file dir name
                if (form.ShowDialog(this) == DialogResult.Yes)
                {
                    Close();
                }
                else
                {
                    return;
                }
            }
        }

        private void btnInField_Click(object sender, EventArgs e)
        {
            string infieldList = "";
            int numFields = 0;

            string[] dirs = Directory.GetDirectories(mf.fieldsDirectory);

            foreach (string dir in dirs)
            {
                double lat = 0;
                double lon = 0;

                string fieldDirectory = Path.GetFileName(dir);
                string filename = dir + "\\Field.txt";
                string line;

                //make sure directory has a field.txt in it
                if (File.Exists(filename))
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        try
                        {
                            //Date time line
                            for (int i = 0; i < 8; i++)
                            {
                                line = reader.ReadLine();
                            }

                            //start positions
                            if (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                string[] offs = line.Split(',');

                                lat = (double.Parse(offs[0], CultureInfo.InvariantCulture));
                                lon = (double.Parse(offs[1], CultureInfo.InvariantCulture));

                                double dist = GetDistance(lon, lat, mf.mc.longitude, mf.mc.latitude);

                                if (dist < 500)
                                {
                                    numFields++;
                                    if (string.IsNullOrEmpty(infieldList))
                                        infieldList += Path.GetFileName(dir);
                                    else
                                        infieldList += "," + Path.GetFileName(dir);
                                }
                            }

                        }
                        catch (Exception)
                        {
                            this.TimedMessageBox(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(infieldList))
            {
                if (numFields > 1)
                {
                    using (FormFilePicker form = new FormFilePicker(mf, 1, infieldList))
                    {
                        //returns full field.txt file dir name
                        if (form.ShowDialog(this) == DialogResult.Yes)
                        {
                            Close();
                        }
                    }
                }
                else // 1 field found
                {
                    mf.currentFieldDirectory = infieldList;
                    mf.FileOpenField();
                    Close();
                }
            }
            else //no fields found
            {
                this.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsFieldNotOpen);
            }
        }

        private double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            double d1 = latitude * (Math.PI / 180.0);
            double num1 = longitude * (Math.PI / 180.0);
            double d2 = otherLatitude * (Math.PI / 180.0);
            double num2 = otherLongitude * (Math.PI / 180.0) - num1;
            double d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        private void btnFromKML_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            DialogResult = DialogResult.No;
            Close();
        }
    }
}