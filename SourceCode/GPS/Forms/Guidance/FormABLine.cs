//Please, if you use this, share the improvements

using System;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormABLine : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private bool isSaving, EditName;
        private Mode Mode;

        public FormABLine(Form callingForm, Mode _Mode)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            Mode = _Mode;

            InitializeComponent();

            btnSwapAB.Visible = Mode != Mode.RecPath;

            Text = Mode == Mode.AB ? gStr.gsABline : Mode == Mode.Curve ? gStr.gsABCurve : gStr.gsRecord;

            btnListDelete.Image = Mode == Mode.AB ? Properties.Resources.ABLineDelete : Mode == Mode.Curve ? Properties.Resources.CurveDelete : Properties.Resources.RecPathDelete;
        }

        private void FormABLine_Load(object sender, EventArgs e)
        {
            panelAPlus.Top = 3;
            panelAPlus.Left = 3;
            panelName.Top = 3;
            panelName.Left = 3;
            panelPick.Top = 3;
            panelPick.Left = 3;

            panelAPlus.Visible = false;
            panelName.Visible = false;
            panelPick.Visible = true;

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
        }

        private void UpdateLineList()
        {
            lvLines.Clear();

            int idx = -1;

            for (int i = 0; i < mf.gyd.curveArr.Count; i++)
            {
                if (mf.gyd.curveArr[i].mode.HasFlag(Mode) && mf.gyd.curveArr[i].points.Count > 1)
                {
                    lvLines.Items.Add(new ListViewItem(mf.gyd.curveArr[i].Name.Trim(), i));

                    if (Mode == Mode.AB && mf.gyd.curveArr[i].Name == mf.gyd.currentABLine?.Name) idx = lvLines.Items.Count - 1;
                    else if (Mode == Mode.Curve && mf.gyd.curveArr[i].Name == mf.gyd.currentCurveLine?.Name) idx = lvLines.Items.Count - 1;
                    else if (Mode == Mode.RecPath && mf.gyd.curveArr[i].Name == mf.gyd.currentRecPath?.Name) idx = lvLines.Items.Count - 1;
                }
            }

            if (idx > -1)
            {
                lvLines.Items[idx].EnsureVisible();
                lvLines.Items[idx].Selected = true;
            }
            // go to bottom of list - if there is a bottom
            else if (lvLines.Items.Count > 0)
            {
                lvLines.Items[lvLines.Items.Count - 1].EnsureVisible();
                lvLines.Items[lvLines.Items.Count - 1].Selected = true;
            }
            lvLines.Select();
        }

        private void btnCancel_APlus_Click(object sender, EventArgs e)
        {
            if (Mode != Mode.AB)
                mf.gyd.isOkToAddDesPoints = false;

            mf.gyd.EditGuidanceLine = null;
            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelName.Visible = false;
            this.Size = new System.Drawing.Size(470, 360);
            UpdateLineList();
        }

        private void btnAPoint_Click(object sender, EventArgs e)
        {
            if (Mode == Mode.AB)
            {
                vec2 fix = mf.pivotAxlePos;

                mf.gyd.EditGuidanceLine.points.Add(new vec2(fix.easting + mf.cosH * mf.tool.toolOffset, fix.northing - mf.sinH * mf.tool.toolOffset));
                mf.gyd.EditGuidanceLine.points.Add(new vec2(mf.gyd.EditGuidanceLine.points[0].easting + mf.sinH, mf.gyd.EditGuidanceLine.points[0].northing + mf.cosH));

                nudHeading.Enabled = true;
                nudHeading.Value = (decimal)glm.toDegrees(mf.fixHeading);

                textBox1.Text = "AB " +
                    (Math.Round(glm.toDegrees(mf.fixHeading), 1)).ToString(CultureInfo.InvariantCulture) +
                    "\u00B0 " + mf.FindDirection(mf.fixHeading);

                btnEnter_APlus.Enabled = true;
            }
            else
            {
                mf.gyd.isOkToAddDesPoints = true;
                btnPausePlay.Enabled = true;
                btnPausePlay.Visible = true;
            }
            btnBPoint.Enabled = true;
            btnAPoint.Enabled = false;
        }

        private void btnBPoint_Click(object sender, EventArgs e)
        {
            if (Mode == Mode.AB)
            {
                vec2 fix = mf.pivotAxlePos;

                btnBPoint.BackColor = System.Drawing.Color.Teal;

                vec2 tt = new vec2(fix.easting + Math.Cos(mf.fixHeading) * mf.tool.toolOffset, fix.northing - Math.Sin(mf.fixHeading) * mf.tool.toolOffset);

                // heading based on AB points
                double heading = Math.Atan2(tt.easting - mf.gyd.EditGuidanceLine.points[0].easting,
                    tt.northing - mf.gyd.EditGuidanceLine.points[0].northing);
                if (heading < 0) heading += glm.twoPI;

                if (mf.gyd.EditGuidanceLine.points.Count > 1)
                    mf.gyd.EditGuidanceLine.points[1] = tt;
                else
                    mf.gyd.EditGuidanceLine.points.Add(tt);

                nudHeading.Value = (decimal)glm.toDegrees(heading);

                textBox1.Text = "AB " +
                    (Math.Round(glm.toDegrees(heading), 1)).ToString(CultureInfo.InvariantCulture) +
                    "\u00B0 " + mf.FindDirection(heading);
            }
            else if (Mode == Mode.RecPath)
            {
                mf.gyd.isOkToAddDesPoints = false;
                panelAPlus.Visible = false;
                int cnt = mf.gyd.EditGuidanceLine.points.Count;
                if (cnt > 3)
                {
                    panelName.Visible = true;

                    double aveLineHeading = mf.gyd.EditGuidanceLine.points.CalculateAverageHeadings();
                    if (aveLineHeading < 0) aveLineHeading += glm.twoPI;
                    textBox1.Text = "Rec " +
                        (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture) +
                        "\u00B0 " + mf.FindDirection(aveLineHeading);
                }
                else
                {
                    mf.gyd.EditGuidanceLine = null;

                    panelPick.Visible = true;
                    panelName.Visible = false;

                    this.Size = panelPick.Size;

                    UpdateLineList();
                }
            }
            else
            {
                mf.gyd.isOkToAddDesPoints = false;
                panelAPlus.Visible = false;
                panelName.Visible = true;

                int cnt = mf.gyd.EditGuidanceLine.points.Count;
                if (cnt > 3)
                {
                    //make sure distance isn't too big between points on Turn
                    for (int i = 0; i < cnt - 1; i++)
                    {
                        int j = i + 1;
                        //if (j == cnt) j = 0;
                        double distance = glm.Distance(mf.gyd.EditGuidanceLine.points[i], mf.gyd.EditGuidanceLine.points[j]);
                        if (distance > 1.2)
                        {
                            vec2 pointB = new vec2((mf.gyd.EditGuidanceLine.points[i].easting + mf.gyd.EditGuidanceLine.points[j].easting) / 2.0,
                                (mf.gyd.EditGuidanceLine.points[i].northing + mf.gyd.EditGuidanceLine.points[j].northing) / 2.0);

                            mf.gyd.EditGuidanceLine.points.Insert(j, pointB);
                            cnt = mf.gyd.EditGuidanceLine.points.Count;
                            i = -1;
                        }
                    }

                    //calculate average heading of line
                    double aveLineHeading = mf.gyd.EditGuidanceLine.points.CalculateAverageHeadings();
                    if (aveLineHeading < 0) aveLineHeading += glm.twoPI;

                    //build the tail extensions
                    mf.gyd.EditGuidanceLine.points.AddFirstLastPoints(200, mf.tool.toolWidth, false);

                    panelAPlus.Visible = false;
                    panelName.Visible = true;

                    textBox1.Text = "Cu " +
                        (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture) +
                        "\u00B0 " + mf.FindDirection(aveLineHeading);
                }
                else
                {
                    mf.gyd.EditGuidanceLine = null;

                    panelPick.Visible = true;
                    panelAPlus.Visible = false;
                    panelName.Visible = false;

                    this.Size = new System.Drawing.Size(470, 360);

                    UpdateLineList();
                }
            }
        }

        private void nudHeading_Click(object sender, EventArgs e)
        {
            if (nudHeading.KeypadToNUD())
            {
                double heading = glm.toRadians((double)nudHeading.Value);

                if (mf.gyd.EditGuidanceLine.points.Count > 1)
                {
                    mf.gyd.EditGuidanceLine.points[0] = new vec2(mf.gyd.EditGuidanceLine.points[0].easting, mf.gyd.EditGuidanceLine.points[0].northing);
                    mf.gyd.EditGuidanceLine.points[1] = new vec2(mf.gyd.EditGuidanceLine.points[0].easting + Math.Sin(heading), mf.gyd.EditGuidanceLine.points[0].northing + Math.Cos(heading));
                }
                else if (mf.gyd.EditGuidanceLine.points.Count > 0)
                {
                    mf.gyd.EditGuidanceLine.points[0] = new vec2(mf.gyd.EditGuidanceLine.points[0].easting, mf.gyd.EditGuidanceLine.points[0].northing);
                    mf.gyd.EditGuidanceLine.points.Add(new vec2(mf.gyd.EditGuidanceLine.points[0].easting + Math.Sin(heading), mf.gyd.EditGuidanceLine.points[0].northing + Math.Cos(heading)));
                }
                else
                {
                    mf.gyd.EditGuidanceLine.points.Add(new vec2(mf.gyd.EditGuidanceLine.points[0].easting, mf.gyd.EditGuidanceLine.points[0].northing));
                    mf.gyd.EditGuidanceLine.points.Add(new vec2(mf.gyd.EditGuidanceLine.points[0].easting + Math.Sin(heading), mf.gyd.EditGuidanceLine.points[0].northing + Math.Cos(heading)));
                }

                textBox1.Text = "AB " +
                    (Math.Round(glm.toDegrees(heading), 1)).ToString(CultureInfo.InvariantCulture) +
                    "\u00B0 " + mf.FindDirection(heading);
            }
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            textBox1.Text += DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
        }

        private void btnEnter_APlus_Click(object sender, EventArgs e)
        {
            panelAPlus.Visible = false;
            panelName.Visible = true;
        }

        private void BtnNewABLine_Click(object sender, EventArgs e)
        {
            lvLines.SelectedItems.Clear();
            panelPick.Visible = false;
            panelAPlus.Visible = true;
            panelName.Visible = false;
            btnAPoint.Enabled = true;
            btnBPoint.Enabled = false;
            this.Size = new System.Drawing.Size(270, 360);

            if (Mode == Mode.AB)
            {
                btnManual.Visible = true;
                nudHeading.Enabled = false;
                btnPausePlay.Visible = false;
                btnEnter_APlus.Enabled = false;
            }
            else
            {
                btnManual.Visible = false;
                nudHeading.Visible = false;
                btnPausePlay.Enabled = false;
                btnEnter_APlus.Visible = false;
            }

            if (Mode == Mode.RecPath)
                mf.gyd.EditGuidanceLine = new CGuidanceRecPath(Mode.RecPath);
            else
                mf.gyd.EditGuidanceLine = new CGuidanceLine(Mode);
        }

        private void btnEditName_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    textBox1.Text = mf.gyd.curveArr[idx].Name.Trim();
                    EditName = true;
                    panelPick.Visible = false;
                    panelName.Visible = true;
                    this.Size = new System.Drawing.Size(270, 360);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "") textBox1.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            string text = textBox1.Text.Trim();
            while (mf.gyd.curveArr.Exists(L => L.Name == text))//generate unique name!
                text += " ";

            if (EditName)
            {
                EditName = false;
                if (lvLines.SelectedItems.Count > 0)
                {
                    int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                    if (idx > -1)
                    {
                        mf.gyd.curveArr[idx].Name = text;
                    }
                }
            }
            else if (mf.gyd.EditGuidanceLine != null && mf.gyd.EditGuidanceLine.points.Count > 1)
            {
                mf.gyd.EditGuidanceLine.Name = text;
                mf.gyd.curveArr.Add(mf.gyd.EditGuidanceLine);
                mf.gyd.EditGuidanceLine = null;
            }

            if (Mode == Mode.AB)
                mf.FileSaveABLines();
            else if (Mode == Mode.RecPath)
                mf.FileSaveRecPath();
            else
                mf.FileSaveCurveLines();

            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
            lvLines.Focus();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    panelAPlus.Visible = false;
                    panelPick.Visible = false;
                    panelName.Visible = true;

                    this.Size = new System.Drawing.Size(270, 360);

                    textBox1.Text = mf.gyd.curveArr[idx].Name.Trim() + " Copy";
                    if (Mode == Mode.RecPath)
                        mf.gyd.EditGuidanceLine = new CGuidanceRecPath(mf.gyd.curveArr[idx]);
                    else
                        mf.gyd.EditGuidanceLine = new CGuidanceLine(mf.gyd.curveArr[idx]);
                }
            }
        }

        private void btnListUse_Click(object sender, EventArgs e)
        {
            isSaving = true;
            Close();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                mf.gyd.isValid = false;

                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                    mf.gyd.ReverseGuidanceLine(mf.gyd.curveArr[idx]);

                if (Mode == Mode.AB)
                    mf.FileSaveABLines();
                else if (Mode == Mode.RecPath)
                    mf.FileSaveRecPath();
                else
                    mf.FileSaveCurveLines();

                UpdateLineList();
                lvLines.Focus();
            }
        }

        private void btnListDelete_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    if (mf.gyd.currentABLine?.Name == mf.gyd.curveArr[idx].Name)
                        mf.gyd.currentABLine = null;

                    if (mf.gyd.currentCurveLine?.Name == mf.gyd.curveArr[idx].Name)
                        mf.gyd.currentCurveLine = null;

                    if (mf.gyd.currentRecPath?.Name == mf.gyd.curveArr[idx].Name)
                        mf.gyd.currentCurveLine = null;

                    if (mf.gyd.currentGuidanceLine?.Name == mf.gyd.curveArr[idx].Name)
                        mf.SetGuidanceMode(Mode.None);

                    mf.gyd.curveArr.RemoveAt(idx);
                    lvLines.SelectedItems[0].Remove();
                }
                if (Mode == Mode.AB)
                    mf.FileSaveABLines();
                else if (Mode == Mode.RecPath)
                    mf.FileSaveRecPath();
                else
                    mf.FileSaveCurveLines();
            }

            UpdateLineList();
            lvLines.Focus();
        }

        private void FormABLine_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.gyd.isValid = false;
            mf.gyd.moveDistance = 0;
            mf.gyd.isOkToAddDesPoints = false;
            mf.gyd.EditGuidanceLine = null;

            if (isSaving && lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    mf.SetGuidanceMode(Mode);

                    if (Mode == Mode.RecPath)
                        mf.gyd.currentGuidanceLine = new CGuidanceRecPath(mf.gyd.curveArr[idx]);
                    else
                        mf.gyd.currentGuidanceLine = new CGuidanceLine(mf.gyd.curveArr[idx]);

                    if (Mode == Mode.AB)
                        mf.gyd.currentABLine = mf.gyd.currentGuidanceLine;
                    else if (Mode == Mode.RecPath)
                        mf.gyd.currentRecPath = mf.gyd.currentGuidanceLine;
                    else
                        mf.gyd.currentCurveLine = mf.gyd.currentGuidanceLine;
                }
            }
            else
            {
                if (Mode == Mode.AB)
                    mf.gyd.currentABLine = null;
                else if (Mode == Mode.RecPath)
                    mf.gyd.currentRecPath = null;
                else
                    mf.gyd.currentCurveLine = null;

                mf.SetGuidanceMode(Mode.None);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.KeyboardToText();
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            using (var form = new FormEnterAB(mf))
            {
                if (form.ShowDialog(this) == DialogResult.OK) 
                {
                    panelAPlus.Visible = false;
                    panelName.Visible = true;
                    textBox1.Text = form.headingText;
                }
                else
                {
                    btnCancel_APlus.PerformClick();
                }
            }
        }

        private void btnPausePlay_Click(object sender, EventArgs e)
        {
            mf.gyd.isOkToAddDesPoints = !mf.gyd.isOkToAddDesPoints;
            btnPausePlay.Image = mf.gyd.isOkToAddDesPoints ? Properties.Resources.boundaryPause : Properties.Resources.BoundaryRecord;
        }

        #region Help
        private void btnListDelete_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnListDelete, gStr.gsHelp).ShowDialog(this);
        }

        private void btnCancel_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnCancel, gStr.gsHelp).ShowDialog(this);
        }

        private void btnNewABLine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnNewABLine, gStr.gsHelp).ShowDialog(this);
        }

        private void btnListUse_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnListUse, gStr.gsHelp).ShowDialog(this);
        }

        private void btnSwapAB_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ht_btnSwapAB, gStr.gsHelp).ShowDialog(this);
        }

        private void btnEditName_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_tboxNameLine, gStr.gsHelp).ShowDialog(this);
        }

        private void btnDuplicate_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnDuplicate, gStr.gsHelp).ShowDialog(this);
        }

        private void btnAPoint_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnAPoint, gStr.gsHelp).ShowDialog(this);
        }

        private void btnBPoint_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnBPoint, gStr.gsHelp).ShowDialog(this);
        }

        private void nudHeading_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_nudHeading, gStr.gsHelp).ShowDialog(this);
        }

        private void btnManual_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnManual, gStr.gsHelp).ShowDialog(this);
        }

        private void textBox1_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_textBox1, gStr.gsHelp).ShowDialog(this);
        }

        private void btnAddTime_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnAddTime, gStr.gsHelp).ShowDialog(this);
        }

        private void btnCancel_APlus_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnCancelCreate, gStr.gsHelp).ShowDialog(this);
        }

        private void btnCancel_Name_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnCancelCreate, gStr.gsHelp).ShowDialog(this);
        }

        private void btnPausePlay_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hcur_btnPausePlay, gStr.gsHelp).ShowDialog(this);
        }

        private void btnEnter_APlus_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnEnterContinue, gStr.gsHelp).ShowDialog(this);
        }

        private void btnAdd_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnEnterContinue, gStr.gsHelp).ShowDialog(this);
        }
        #endregion
    }
}
