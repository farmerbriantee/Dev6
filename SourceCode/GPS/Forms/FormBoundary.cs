using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormBoundary : Form
    {
        private readonly FormGPS mf = null;

        private double easting, norting, latK, lonK;
        private int fenceSelected = -1;

        private bool isClosing;

        public FormBoundary(Form callingForm)
        {
            mf = callingForm as FormGPS;

            //winform initialization
            InitializeComponent();

            this.Text = gStr.gsStartDeleteABoundary;

            //Column Header
            Boundary.Text = "Bounds";
            Thru.Text = gStr.gsDriveThru;
            Area.Text = gStr.gsArea;
            btnDelete.Enabled = false;
        }

        private void FormBoundary_Load(object sender, EventArgs e)
        {
            this.Size = new Size(566, 377);

            //update the list view with real data
            UpdateChart();
            panelCreate.Dock = DockStyle.Fill;
            panelMain.Dock = DockStyle.Fill;
            panelChoose.Dock = DockStyle.Fill;
            panelKML.Dock = DockStyle.Fill;

            mf.CloseTopMosts();
        }

        private void UpdateChart()
        {
            int inner = 1;

            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            Font backupfont = new Font(Font.FontFamily, 18F, FontStyle.Bold);

            for (int i = 0; i < mf.bnd.bndList.Count && i < 6; i++)
            {
                //outer inner
                Button a = new Button
                {
                    Margin = new Padding(6),
                    Size = new Size(150, 35),
                    Name = i.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    //ForeColor = System.Drawing.SystemColors.ButtonFace
                };
                a.Click += B_Click;
                a.BackColor = System.Drawing.SystemColors.ButtonFace;
                //a.Font = backupfont;
                //a.FlatStyle = FlatStyle.Flat;
                //a.FlatAppearance.BorderColor = Color.Cyan;
                //a.BackColor = Color.Transparent;
                //a.FlatAppearance.MouseOverBackColor = BackColor;
                //a.FlatAppearance.MouseDownBackColor = BackColor;


                //area
                Button b = new Button
                {
                    Margin = new Padding(6),
                    Size = new System.Drawing.Size(150, 35),
                    Name = i.ToString(),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    //ForeColor = System.Drawing.SystemColors.ButtonFace
                };
                b.Click += B_Click;
                b.BackColor = System.Drawing.SystemColors.ButtonFace;
                //b.FlatStyle = FlatStyle.Flat;
                //b.Font = backupfont;
                //b.FlatAppearance.BorderColor = BackColor;
                //b.FlatAppearance.MouseOverBackColor = BackColor;
                //b.FlatAppearance.MouseDownBackColor = BackColor;

                //drive thru
                Button d = new Button
                {
                    Margin = new Padding(6),
                    Size = new System.Drawing.Size(80, 35),
                    Name = i.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    //ForeColor = System.Drawing.SystemColors.ButtonFace
                    //Font = backupfont
                };
                d.Click += DriveThru_Click;
                d.BackColor = System.Drawing.SystemColors.ButtonFace;
                d.Visible = true;

                tableLayoutPanel1.Controls.Add(a, 0, i);
                tableLayoutPanel1.Controls.Add(b, 1, i);
                tableLayoutPanel1.Controls.Add(d, 2, i);

                if (i == 0)
                {
                    //cc.Text = "Outer";
                    mf.bnd.bndList[i].isDriveThru = false;
                    a.Text = string.Format(gStr.gsOuter);
                    //a.Font = backupfont;
                    d.Text = "--";
                    d.Enabled = false;
                    d.Anchor = System.Windows.Forms.AnchorStyles.None;
                    a.Anchor = System.Windows.Forms.AnchorStyles.None;
                    b.Anchor = System.Windows.Forms.AnchorStyles.None;
                    //d.BackColor = Color.Transparent;
                }
                else
                {
                    //cc.Text = "Inner";
                    inner += 1;
                    a.Text = string.Format(gStr.gsInner + " {0}", inner);
                    //a.Font = backupfont;
                    d.Text = mf.bnd.bndList[i].isDriveThru ? "Yes" : "No";
                    d.Anchor = System.Windows.Forms.AnchorStyles.None;
                    a.Anchor = System.Windows.Forms.AnchorStyles.None;
                    b.Anchor = System.Windows.Forms.AnchorStyles.None;
                    //d.BackColor = Color.Transparent;
                }

                int length = (mf.bnd.bndList[i].area * glm.m2ToUser).ToString("0").Length;
                if (length > 10) length = 10;
                if (length < 3) length = 3;
                b.Text = (mf.bnd.bndList[i].area * glm.m2ToUser).ToString("0.########".Substring(0, 11 - length)) + glm.unitsHaAc;

                if (i == fenceSelected)
                {
                    a.ForeColor = Color.OrangeRed;
                    b.ForeColor = Color.OrangeRed;
                }
                else
                {
                    a.ForeColor = System.Drawing.SystemColors.ControlText;
                    b.ForeColor = System.Drawing.SystemColors.ControlText;
                }
            }
        }

        private void DriveThru_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                mf.bnd.bndList[Convert.ToInt32(b.Name)].isDriveThru = !mf.bnd.bndList[Convert.ToInt32(b.Name)].isDriveThru;
                UpdateChart();
            }
        }

        private void B_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                int oldfenceSelected = fenceSelected;
                fenceSelected = Convert.ToInt32(b.Name);

                if (fenceSelected == oldfenceSelected)
                    fenceSelected = -1;
                else if (fenceSelected == 0)
                    btnDelete.Enabled = mf.bnd.bndList.Count == 1;
                else if (fenceSelected > 0)
                    btnDelete.Enabled = true;

                btnDeleteAll.Enabled = fenceSelected == -1;
            }
            UpdateChart();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (new FormHelp(gStr.gsCompletelyDeleteBoundary, gStr.gsDeleteForSure, true).ShowDialog(this) == DialogResult.Yes)
            {

                btnDelete.Enabled = false;

                if (mf.bnd.bndList.Count > fenceSelected)
                    mf.bnd.RemoveHandles(fenceSelected);
                fenceSelected = -1;

                mf.FileSaveBoundary();
                mf.CalculateMinMax();
                UpdateChart();
            }
            else
            {
                mf.TimedMessageBox(1500, gStr.gsNothingDeleted, gStr.gsActionHasBeenCancelled);
            }
        }

        private void ResetAllBoundary()
        {
            fenceSelected = -1;

            for (int i = mf.bnd.bndList.Count - 1; i >= 0; i--)
                mf.bnd.RemoveHandles(i);

            mf.FileSaveBoundary();
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            UpdateChart();
            btnDelete.Enabled = false;

            mf.CalculateMinMax();
        }

        private void btnOpenGoogleEarth_Click(object sender, EventArgs e)
        {
            //save new copy of kml with selected flag and view in GoogleEarth

            mf.FileMakeKMLFromCurrentPosition(mf.mc.latitude, mf.mc.longitude);
            System.Diagnostics.Process.Start(mf.fieldsDirectory + mf.currentFieldDirectory + "\\CurrentPosition.KML");
            isClosing = true;
            Close();
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (new FormHelp(gStr.gsCompletelyDeleteBoundary, gStr.gsDeleteForSure, true).ShowDialog(this) == DialogResult.Yes)
            {
                ResetAllBoundary();
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            panelMain.Visible = true;
            panelChoose.Visible = false;
            panelKML.Visible = false;

            this.Size = new System.Drawing.Size(566, 377);
            isClosing = true;
            UpdateChart();
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            panelMain.Visible = false;
            panelKML.Visible = false;
            panelChoose.Visible = true;
            panelChoose.Dock = DockStyle.Fill;

            this.Size = new Size(260, 377);
        }

        private void FormBoundary_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }
        }


        private void btnLoadBoundaryFromGE_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string fileAndDirectory;
                {
                    //create the dialog instance
                    OpenFileDialog ofd = new OpenFileDialog
                    {
                        //set the filter to text KML only
                        Filter = "KML files (*.KML)|*.KML",

                        //the initial directory, fields, for the open dialog
                        InitialDirectory = mf.fieldsDirectory + mf.currentFieldDirectory
                    };

                    //was a file selected
                    if (ofd.ShowDialog(this) == DialogResult.Cancel) return;
                    else fileAndDirectory = ofd.FileName;
                }

                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {

                    if (button.Name == "btnLoadMultiBoundaryFromGE") ResetAllBoundary();

                    try
                    {
                        while (!reader.EndOfStream)
                        {
                            string coordinates = "";

                            //start to read the file
                            string line = reader.ReadLine();

                            int startIndex = line.IndexOf("<coordinates>");

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
                                    CBoundaryList New = new CBoundaryList();

                                    foreach (string item in numberSets)
                                    {
                                        string[] fix = item.Split(',');
                                        double.TryParse(fix[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);
                                        double.TryParse(fix[1], NumberStyles.Float, CultureInfo.InvariantCulture, out latK);

                                        mf.worldManager.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                                        //add the point to boundary
                                        New.fenceLine.points.Add(new vec2(easting, norting));
                                    }

                                    New.CalculateFenceArea();

                                    mf.bnd.bndList.Add(New);
                                }
                                else
                                {
                                    mf.TimedMessageBox(2000, gStr.gsErrorreadingKML, gStr.gsChooseBuildDifferentone);
                                }
                                if (button.Name == "btnLoadBoundaryFromGE")
                                {
                                    break;
                                }
                            }
                        }
                        mf.FileSaveBoundary();
                        mf.CalculateMinMax();
                        mf.bnd.BuildTurnLines();
                        UpdateChart();
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }

            panelMain.Visible = true;
            panelChoose.Visible = false;
            panelKML.Visible = false;

            this.Size = new Size(566, 377);

            UpdateChart();
        }

        private void btnDriveOrExt_Click(object sender, EventArgs e)
        {
            panelCreate.Visible = true;
            panelChoose.Visible = false;

            //btnStop.Text = gStr.gsDone;
            //btnPausePlay.Text = gStr.gsRecord;
            Area2.Text = gStr.gsArea + ":";
            this.Text = gStr.gsStopRecordPauseBoundary;
            nudOffset.Controls[0].Enabled = false;
            lblOffset.Text = gStr.gsOffset;

            //mf.bnd.isOkToAddPoints = false;
            nudOffset.Value = (decimal)(mf.tool.toolWidth * 0.5);
            btnPausePlay.Image = Properties.Resources.BoundaryRecord;
            btnLeftRight.Image = mf.bnd.isDrawRightSide ? Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            mf.bnd.createBndOffset = (double)nudOffset.Value;
            mf.bnd.isBndBeingMade = true;

            mf.vehicle.updateVBO = true;
        }

        private void btnGetKML_Click(object sender, EventArgs e)
        {
            panelMain.Visible = false;
            panelChoose.Visible = false;
            panelKML.Visible = true;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            if (new FormHelp(gStr.gsCompletelyDeleteBoundary, gStr.gsDeleteForSure, true).ShowDialog(this) == DialogResult.Yes)
            {
                mf.bnd.bndBeingMadePts.points.Clear();
                mf.bnd.bndBeingMadePts.ResetPoints = true;

                lblPoints.Text = mf.bnd.bndBeingMadePts.points.Count.ToString();
            }
            mf.Focus();
        }

        private void btnPausePlay_Click(object sender, EventArgs e)
        {
            mf.bnd.isOkToAddPoints = !mf.bnd.isOkToAddPoints;

            btnPausePlay.Image = mf.bnd.isOkToAddPoints ? Properties.Resources.boundaryPause : Properties.Resources.BoundaryRecord;
            btnAddPoint.Enabled = btnDeleteLast.Enabled = !mf.bnd.isOkToAddPoints;

            timer1.Enabled = mf.bnd.isOkToAddPoints;

            mf.Focus();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (mf.bnd.bndBeingMadePts.points.Count > 2)
            {
                CBoundaryList New = new CBoundaryList();

                New.fenceLine = mf.bnd.bndBeingMadePts;

                mf.bnd.bndBeingMadePts = new Polyline2();

                New.CalculateFenceArea();

                mf.bnd.bndList.Add(New);

                //turn lines made from boundaries
                mf.CalculateMinMax();
                mf.FileSaveBoundary();
                mf.bnd.BuildTurnLines();
            }
            else
            {
                mf.bnd.bndBeingMadePts.points.Clear();
                mf.bnd.bndBeingMadePts.ResetPoints = true;
            }

            //stop it all for adding
            mf.bnd.isOkToAddPoints = false;
            mf.bnd.isBndBeingMade = false;

            //close window
            isClosing = true;
            Close();
        }

        private void nudOffset_Click(object sender, EventArgs e)
        {
            nudOffset.KeypadToNUD();
            btnPausePlay.Focus();
            mf.bnd.createBndOffset = (double)nudOffset.Value;
            mf.vehicle.updateVBO = true;
        }

        private void btnLeftRight_Click(object sender, EventArgs e)
        {
            mf.bnd.isDrawRightSide = !mf.bnd.isDrawRightSide;
            btnLeftRight.Image = mf.bnd.isDrawRightSide ? Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;

            mf.vehicle.updateVBO = true;
        }

        private void btnDeleteLast_Click(object sender, EventArgs e)
        {
            if (mf.bnd.bndBeingMadePts.points.Count > 0)
            {
                mf.bnd.bndBeingMadePts.points.RemoveAt(mf.bnd.bndBeingMadePts.points.Count - 1);
                mf.bnd.bndBeingMadePts.ResetPoints = true;
            }

            timer1_Tick(null, null);
        }

        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            mf.AddBoundaryPoint();
            timer1_Tick(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int ptCount = mf.bnd.bndBeingMadePts.points.Count;
            double area = 0;

            if (ptCount > 0)
            {
                int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

                for (int i = 0; i < ptCount; j = i++)
                {
                    area += (mf.bnd.bndBeingMadePts.points[j].easting + mf.bnd.bndBeingMadePts.points[i].easting) * (mf.bnd.bndBeingMadePts.points[j].northing - mf.bnd.bndBeingMadePts.points[i].northing);
                }
                area = Math.Abs(area / 2);
            }

            lblArea.Text = Math.Round(area * glm.m2ToUser, 2) + glm.unitsHaAc;
            lblPoints.Text = mf.bnd.bndBeingMadePts.points.Count.ToString();
        }


        #region Help

        private void btnDelete_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnDelete, gStr.gsHelp).ShowDialog(this);
        }

        private void btnDeleteAll_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnDeleteAll, gStr.gsHelp).ShowDialog(this);
        }

        private void btnOpenGoogleEarth_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnOpenGoogleEarth, gStr.gsHelp).ShowDialog(this);
        }

        private void btnAdd_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnAdd, gStr.gsHelp).ShowDialog(this);
        }

        private void btnCancel_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnCancel, gStr.gsHelp).ShowDialog(this);
        }

        private void btnGetKML_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnGetKML, gStr.gsHelp).ShowDialog(this);
        }

        private void nudOffset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_nudOffset, gStr.gsHelp).ShowDialog(this);
        }

        private void btnLeftRight_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnLeftRight, gStr.gsHelp).ShowDialog(this);
        }

        private void btnDeleteLast_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnDeleteLast, gStr.gsHelp).ShowDialog(this);
        }

        private void btnAddPoint_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnAddPoint, gStr.gsHelp).ShowDialog(this);
        }

        private void btnRestart_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnRestart, gStr.gsHelp).ShowDialog(this);
        }

        private void btnPausePlay_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnPausePlay, gStr.gsHelp).ShowDialog(this);
        }

        private void btnStop_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnStop, gStr.gsHelp).ShowDialog(this);
        }

        private void btnBingMaps_Click(object sender, EventArgs e)
        {
            isClosing = true;
            Close();
            var form3 = new FormMap(mf);
            form3.Show(mf);
        }

        private void btnDriveOrExt_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnDriveOrExt, gStr.gsHelp).ShowDialog(this);
        }

        private void btnLoadMultiBoundaryFromGE_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnLoadMultiBoundaryFromGE, gStr.gsHelp).ShowDialog(this);
        }

        private void btnLoadBoundaryFromGE_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hb_btnLoadBoundaryFromGE, gStr.gsHelp).ShowDialog(this);
        }

        #endregion
    }
}

/*
            
            new FormHelp(gStr, gStr.gsHelp).ShowDialog(this)

            DialogResult result2 = new FormHelp(gStr, gStr.gsHelp,
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result2 == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=rsJMRZrcuX4");
            }

*/
