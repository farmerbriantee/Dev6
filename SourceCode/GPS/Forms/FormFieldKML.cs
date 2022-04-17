using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormFieldKML : Form
    {
        //class variables
        private readonly FormGPS mf = null;
        private double easting, northing, latK, lonK;
        private bool basedOnKML = false, btnSaveEnabled = false;
        private string[] FileNames;

        public FormFieldKML(Form _callingForm, bool Basedon)
        {
            //get copy of the calling main form
            mf = _callingForm as FormGPS;

            InitializeComponent();

            basedOnKML = Basedon;

            label1.Text = gStr.gsEnterFieldName;
            this.Text = gStr.gsCreateNewField;
        }

        private void FormFieldDir_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            btnLoadKML.Visible = basedOnKML;

            lblFilename.Text = "";
        }

        private void tboxFieldName_TextChanged(object sender, EventArgs e)
        {
            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            if (String.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true && (btnSaveEnabled || !basedOnKML);
            }

            lblFilename.Text = tboxFieldName.Text.Trim();
            if (cboxAddDate.Checked) lblFilename.Text += " " + DateTime.Now.ToString("MMM.dd", CultureInfo.InvariantCulture);
            if (cboxAddTime.Checked) lblFilename.Text += " " + DateTime.Now.ToString("HH_mm", CultureInfo.InvariantCulture);
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //fill something in
            if (string.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                MessageBox.Show(gStr.gsChooseADifferentName, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            //append date time to name
            mf.currentFieldDirectory = tboxFieldName.Text.Trim() + " ";

            //date
            if (cboxAddDate.Checked) mf.currentFieldDirectory += " " + DateTime.Now.ToString("MMM.dd", CultureInfo.InvariantCulture);
            if (cboxAddTime.Checked) mf.currentFieldDirectory += " " + DateTime.Now.ToString("HH_mm", CultureInfo.InvariantCulture);

            //get the directory and make sure it exists, create if not
            string dirNewField = mf.fieldsDirectory + mf.currentFieldDirectory + "\\";

            //if no template set just make a new file.
            try
            {
                //create it for first save
                string directoryName = Path.GetDirectoryName(dirNewField);

                if ((!string.IsNullOrEmpty(directoryName)) && (Directory.Exists(directoryName)))
                {
                   MessageBox.Show(gStr.gsChooseADifferentName, gStr.gsDirectoryExists, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    //start a new job
                    mf.JobNew();

                    if (!basedOnKML)
                    {
                        mf.worldManager.latStart = mf.mc.latitude;
                        mf.worldManager.lonStart = mf.mc.longitude;
                    }
                    else
                    {
                        mf.worldManager.latStart = latK;
                        mf.worldManager.lonStart = lonK;
                    }

                    if (mf.timerSim.Enabled)
                    {
                        Properties.Settings.Default.setGPS_SimLatitude = mf.worldManager.latStart;
                        Properties.Settings.Default.setGPS_SimLongitude = mf.worldManager.lonStart;
                        Properties.Settings.Default.Save();

                        mf.sim.resetSim();
                    }

                    mf.worldManager.SetLocalMetersPerDegree();


                    //make sure directory exists, or create it
                    if ((!string.IsNullOrEmpty(directoryName)) && (!Directory.Exists(directoryName)))
                    { Directory.CreateDirectory(directoryName); }

                    mf.displayFieldName = mf.currentFieldDirectory;

                    //create the field file header info
                    mf.FileCreateField();
                    mf.FileCreateSections();
                    mf.FileCreateRecPath();
                    mf.FileCreateContour();
                    mf.FileCreateElevation();
                    mf.FileSaveFlags();

                    //Load the boundary
                    if (basedOnKML)
                    {
                        for (int i = 0; i < FileNames.Length; i++)
                        {
                            //Load the outer boundary
                            LoadKMLBoundary(FileNames[i]);
                        }
                        mf.bnd.BuildTurnLines();
                        mf.CalculateMinMax();
                    }
                    mf.FileSaveBoundary();
                }
            }
            catch (Exception ex)
            {
                mf.WriteErrorLog("Creating new field " + ex);

                MessageBox.Show(gStr.gsError, ex.ToString());
                mf.currentFieldDirectory = "";
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void tboxFieldName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }

        private void btnLoadKML_Click(object sender, EventArgs e)
        {
            //create the dialog instance
            OpenFileDialog ofd = new OpenFileDialog
            {
                //set the filter to text KML only
                Filter = "KML files (*.KML)|*.KML",

                //the initial directory, fields, for the open dialog
                InitialDirectory = mf.fieldsDirectory,
                Multiselect = true
            };

            //was a file selected
            if (ofd.ShowDialog() == DialogResult.Cancel) return;

            FileNames = ofd.FileNames;
            //get lat and lon from boundary in kml
            FindLatLon(ofd.FileName);
        }

        private void LoadKMLBoundary(string filename)
        {
            string coordinates = null;
            int startIndex;

            using (System.IO.StreamReader reader = new System.IO.StreamReader(filename))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        //start to read the file
                        string line = reader.ReadLine();

                        startIndex = line.IndexOf("<coordinates>");

                        if (startIndex != -1)
                        {
                            while (true)
                            {
                                int endIndex = line.IndexOf("</coordinates>");

                                if (endIndex == -1)
                                {
                                    //just add the line
                                    if (startIndex == -1) coordinates += line.Substring(0);
                                    else coordinates += line.Substring(startIndex + 13);
                                }
                                else
                                {
                                    if (startIndex == -1) coordinates += line.Substring(0, endIndex);
                                    else coordinates += line.Substring(startIndex + 13, endIndex - (startIndex + 13));
                                    break;
                                }
                                line = reader.ReadLine();
                                line = line.Trim();
                                startIndex = -1;
                            }

                            line = coordinates;
                            char[] delimiterChars = { ' ', '\t', '\r', '\n' };
                            string[] numberSets = line.Split();

                            //at least 3 points
                            if (numberSets.Length > 2)
                            {
                                CBoundaryList New = new CBoundaryList();

                                foreach (string item in numberSets)
                                {
                                    if (item.Length < 3)
                                        continue;
                                    string[] fix = item.Split(',');
                                    double.TryParse(fix[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);
                                    double.TryParse(fix[1], NumberStyles.Float, CultureInfo.InvariantCulture, out latK);

                                    mf.worldManager.ConvertWGS84ToLocal(latK, lonK, out northing, out easting);

                                    //add the point to boundary
                                    New.fenceLine.points.Add(new vec2(easting, northing));
                                }

                                //build the boundary, make sure is clockwise for outer counter clockwise for inner
                                New.CalculateFenceArea();

                                mf.bnd.bndList.Add(New);

                                coordinates = "";
                            }
                            else
                            {
                                mf.TimedMessageBox(2000, gStr.gsErrorreadingKML, gStr.gsChooseBuildDifferentone);
                            }
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        private void FindLatLon(string filename)
        {
            string coordinates = null;
            int startIndex;

            using (System.IO.StreamReader reader = new System.IO.StreamReader(filename))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        //start to read the file
                        string line = reader.ReadLine();

                        startIndex = line.IndexOf("<coordinates>");

                        if (startIndex != -1)
                        {
                            while (true)
                            {
                                int endIndex = line.IndexOf("</coordinates>");

                                if (endIndex == -1)
                                {
                                    //just add the line
                                    if (startIndex == -1) coordinates += line.Substring(0);
                                    else coordinates += line.Substring(startIndex + 13);
                                }
                                else
                                {
                                    if (startIndex == -1) coordinates += line.Substring(0, endIndex);
                                    else coordinates += line.Substring(startIndex + 13, endIndex - (startIndex + 13));
                                    break;
                                }
                                line = reader.ReadLine();
                                line = line.Trim();
                                startIndex = -1;
                            }

                            line = coordinates;
                            char[] delimiterChars = { ' ', '\t', '\r', '\n' };
                            string[] numberSets = line.Split(delimiterChars);

                            //at least 3 points
                            if (numberSets.Length > 2)
                            {
                                double counter = 0, lat = 0, lon = 0;
                                latK = lonK = 0;
                                foreach (string item in numberSets)
                                {
                                    if (item.Length < 3)
                                        continue;
                                    string[] fix = item.Split(',');
                                    double.TryParse(fix[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);
                                    double.TryParse(fix[1], NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                                    lat += latK;
                                    lon += lonK;
                                    counter += 1;
                                }
                                lonK = lon / counter;
                                latK = lat / counter;

                                coordinates = "";
                            }
                            else
                            {
                                mf.TimedMessageBox(2000, gStr.gsErrorreadingKML, gStr.gsChooseBuildDifferentone);
                            }
                            break;
                        }
                    }

                    btnSaveEnabled = true;
                    btnSave.Enabled = !String.IsNullOrEmpty(tboxFieldName.Text.Trim());
                }
                catch (Exception)
                {
                    btnSaveEnabled = false;
                    btnSave.Enabled = false;
                    mf.TimedMessageBox(2000, "Exception", "Catch Exception");
                    return;
                }
            }
        }
    }
}
