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

        private bool isSaving, EditName, ModeAB;

        public FormABLine(Form callingForm, bool _ModeAB)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            ModeAB = _ModeAB;

            InitializeComponent();

            Text = ModeAB ? gStr.gsABline : gStr.gsABCurve;
            btnListDelete.Image = ModeAB ? Properties.Resources.ABLineDelete : Properties.Resources.HideContour;
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
                if (mf.gyd.curveArr[i].mode.HasFlag(ModeAB ? Mode.AB : Mode.Curve) && mf.gyd.curveArr[i].curvePts.Count > 1)
                {
                    lvLines.Items.Add(new ListViewItem(mf.gyd.curveArr[i].Name.Trim(), i));
                    if (mf.gyd.curveArr[i].Name == mf.gyd.currentABLine?.Name) idx = lvLines.Items.Count - 1;
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
            if (ModeAB)
                btnBPoint.BackColor = System.Drawing.Color.Transparent;
            else
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
            if (ModeAB)
            {
                vec3 fix = new vec3(mf.pivotAxlePos);

                mf.gyd.EditGuidanceLine.curvePts.Add(new vec3(fix.easting + Math.Cos(fix.heading) * mf.tool.toolOffset, fix.northing - Math.Sin(fix.heading) * mf.tool.toolOffset, fix.heading));

                nudHeading.Enabled = true;
                nudHeading.Value = (decimal)glm.toDegrees(fix.heading);

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
            if (ModeAB)
            {
                vec3 fix = new vec3(mf.pivotAxlePos);

                btnBPoint.BackColor = System.Drawing.Color.Teal;

                vec3 tt = new vec3(fix.easting + Math.Cos(fix.heading) * mf.tool.toolOffset, fix.northing - Math.Sin(fix.heading) * mf.tool.toolOffset, fix.heading);

                // heading based on AB points
                double heading = Math.Atan2(tt.easting - mf.gyd.EditGuidanceLine.curvePts[0].easting,
                    tt.northing - mf.gyd.EditGuidanceLine.curvePts[0].northing);
                if (heading < 0) heading += glm.twoPI;
                tt.heading = heading;
                if (mf.gyd.EditGuidanceLine.curvePts.Count > 0)
                    mf.gyd.EditGuidanceLine.curvePts[0] = new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting, mf.gyd.EditGuidanceLine.curvePts[0].northing, heading);

                if (mf.gyd.EditGuidanceLine.curvePts.Count > 1)
                    mf.gyd.EditGuidanceLine.curvePts[1] = tt;
                else
                    mf.gyd.EditGuidanceLine.curvePts.Add(tt);

                nudHeading.Value = (decimal)glm.toDegrees(heading);

                textBox1.Text = "AB " +
                    (Math.Round(glm.toDegrees(heading), 1)).ToString(CultureInfo.InvariantCulture) +
                    "\u00B0 " + mf.FindDirection(heading);
            }
            else
            {
                mf.gyd.isOkToAddDesPoints = false;
                panelAPlus.Visible = false;
                panelName.Visible = true;

                int cnt = mf.gyd.EditGuidanceLine.curvePts.Count;
                if (cnt > 3)
                {
                    //make sure distance isn't too big between points on Turn
                    for (int i = 0; i < cnt - 1; i++)
                    {
                        int j = i + 1;
                        //if (j == cnt) j = 0;
                        double distance = glm.Distance(mf.gyd.EditGuidanceLine.curvePts[i], mf.gyd.EditGuidanceLine.curvePts[j]);
                        if (distance > 1.2)
                        {
                            vec3 pointB = new vec3((mf.gyd.EditGuidanceLine.curvePts[i].easting + mf.gyd.EditGuidanceLine.curvePts[j].easting) / 2.0,
                                (mf.gyd.EditGuidanceLine.curvePts[i].northing + mf.gyd.EditGuidanceLine.curvePts[j].northing) / 2.0,
                                mf.gyd.EditGuidanceLine.curvePts[i].heading);

                            mf.gyd.EditGuidanceLine.curvePts.Insert(j, pointB);
                            cnt = mf.gyd.EditGuidanceLine.curvePts.Count;
                            i = -1;
                        }
                    }

                    //calculate average heading of line
                    double x = 0, y = 0;
                    foreach (vec3 pt in mf.gyd.EditGuidanceLine.curvePts)
                    {
                        x += Math.Cos(pt.heading);
                        y += Math.Sin(pt.heading);
                    }
                    x /= mf.gyd.EditGuidanceLine.curvePts.Count;
                    y /= mf.gyd.EditGuidanceLine.curvePts.Count;
                    double aveLineHeading = Math.Atan2(y, x);
                    if (aveLineHeading < 0) aveLineHeading += glm.twoPI;

                    //build the tail extensions
                    AddFirstLastPoints();
                    SmoothAB(4);
                    mf.gyd.EditGuidanceLine.curvePts.CalculateHeadings(false);

                    panelAPlus.Visible = false;
                    panelName.Visible = true;

                    textBox1.Text = "Cu " +
                        (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture) +
                        "\u00B0 " + mf.FindDirection(aveLineHeading);
                }
                else
                {
                    mf.gyd.EditGuidanceLine.curvePts.Clear();

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
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                double heading = glm.toRadians((double)nudHeading.Value);

                if (mf.gyd.EditGuidanceLine.curvePts.Count > 1)
                {
                    mf.gyd.EditGuidanceLine.curvePts[0] = new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting, mf.gyd.EditGuidanceLine.curvePts[0].northing, heading);
                    mf.gyd.EditGuidanceLine.curvePts[1] = new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting + Math.Sin(heading), mf.gyd.EditGuidanceLine.curvePts[0].northing + Math.Cos(heading), heading);
                }
                else if (mf.gyd.EditGuidanceLine.curvePts.Count > 0)
                {
                    mf.gyd.EditGuidanceLine.curvePts[0] = new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting, mf.gyd.EditGuidanceLine.curvePts[0].northing, heading);
                    mf.gyd.EditGuidanceLine.curvePts.Add(new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting + Math.Sin(heading), mf.gyd.EditGuidanceLine.curvePts[0].northing + Math.Cos(heading), heading));
                }
                else
                {
                    mf.gyd.EditGuidanceLine.curvePts.Add(new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting, mf.gyd.EditGuidanceLine.curvePts[0].northing, heading));
                    mf.gyd.EditGuidanceLine.curvePts.Add(new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting + Math.Sin(heading), mf.gyd.EditGuidanceLine.curvePts[0].northing + Math.Cos(heading), heading));
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

            if (ModeAB)
            {
                btnManual.Visible = true;
                btnPausePlay.Visible = false;
                nudHeading.Enabled = false;
                btnEnter_APlus.Enabled = false;

                mf.gyd.EditGuidanceLine = new CGuidanceLine(Mode.AB);
            }
            else
            {
                btnManual.Visible = false;
                nudHeading.Visible = false;
                btnEnter_APlus.Visible = false;
                btnPausePlay.Enabled = false;
                mf.gyd.EditGuidanceLine = new CGuidanceLine(Mode.Curve);
            }
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
            else if (mf.gyd.EditGuidanceLine != null && mf.gyd.EditGuidanceLine.curvePts.Count > 1)
            {
                mf.gyd.EditGuidanceLine.Name = text;
                mf.gyd.curveArr.Add(mf.gyd.EditGuidanceLine);
                mf.gyd.EditGuidanceLine = null;
            }

            if (ModeAB)
                mf.FileSaveABLines();
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

                    mf.gyd.EditGuidanceLine = new CGuidanceLine(mf.gyd.curveArr[idx].mode);
                    mf.gyd.EditGuidanceLine.curvePts.AddRange(mf.gyd.curveArr[idx].curvePts.ToArray());
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
                if (ModeAB)
                    mf.FileSaveABLines();
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

                    if (mf.gyd.currentGuidanceLine?.Name == mf.gyd.curveArr[idx].Name)
                        mf.SetGuidanceMode(Mode.None);

                    mf.gyd.curveArr.RemoveAt(idx);
                    lvLines.SelectedItems[0].Remove();
                }
                if (ModeAB)
                    mf.FileSaveABLines();
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
                    mf.SetGuidanceMode(ModeAB ? Mode.AB : Mode.Curve);

                    mf.gyd.currentGuidanceLine = new CGuidanceLine(mf.gyd.curveArr[idx]);

                    if (ModeAB)
                        mf.gyd.currentABLine = mf.gyd.currentGuidanceLine;
                    else
                        mf.gyd.currentCurveLine = mf.gyd.currentGuidanceLine;
                }
            }
            else
            {
                if (ModeAB)
                    mf.gyd.currentABLine = null;
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
            if (mf.isKeyboardOn)
                mf.KeyboardToText((TextBox)sender, this);
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

        public void SmoothAB(int smPts)
        {
            //count the reference list of original curve
            int cnt = mf.gyd.EditGuidanceLine.curvePts.Count;

            //the temp array
            vec3[] arr = new vec3[cnt];

            //read the points before and after the setpoint
            for (int s = 0; s < smPts / 2; s++)
            {
                arr[s].easting = mf.gyd.EditGuidanceLine.curvePts[s].easting;
                arr[s].northing = mf.gyd.EditGuidanceLine.curvePts[s].northing;
                arr[s].heading = mf.gyd.EditGuidanceLine.curvePts[s].heading;
            }

            for (int s = cnt - (smPts / 2); s < cnt; s++)
            {
                arr[s].easting = mf.gyd.EditGuidanceLine.curvePts[s].easting;
                arr[s].northing = mf.gyd.EditGuidanceLine.curvePts[s].northing;
                arr[s].heading = mf.gyd.EditGuidanceLine.curvePts[s].heading;
            }

            //average them - center weighted average
            for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
            {
                for (int j = -smPts / 2; j < smPts / 2; j++)
                {
                    arr[i].easting += mf.gyd.EditGuidanceLine.curvePts[j + i].easting;
                    arr[i].northing += mf.gyd.EditGuidanceLine.curvePts[j + i].northing;
                }
                arr[i].easting /= smPts;
                arr[i].northing /= smPts;
                arr[i].heading = mf.gyd.EditGuidanceLine.curvePts[i].heading;
            }

            //make a list to draw
            mf.gyd.EditGuidanceLine.curvePts.Clear();
            for (int i = 0; i < cnt; i++)
            {
                mf.gyd.EditGuidanceLine.curvePts.Add(arr[i]);
            }
        }

        public void AddFirstLastPoints()
        {
            int ptCnt = mf.gyd.EditGuidanceLine.curvePts.Count - 1;
            for (int i = 1; i < 200; i++)
            {
                vec3 pt = new vec3(mf.gyd.EditGuidanceLine.curvePts[ptCnt]);
                pt.easting += (Math.Sin(pt.heading) * i);
                pt.northing += (Math.Cos(pt.heading) * i);
                mf.gyd.EditGuidanceLine.curvePts.Add(pt);
            }

            //and the beginning
            vec3 start = new vec3(mf.gyd.EditGuidanceLine.curvePts[0]);
            for (int i = 1; i < 200; i++)
            {
                vec3 pt = new vec3(start);
                pt.easting -= (Math.Sin(pt.heading) * i);
                pt.northing -= (Math.Cos(pt.heading) * i);
                mf.gyd.EditGuidanceLine.curvePts.Insert(0, pt);
            }
        }

        #region Help
        private void btnListDelete_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnListDelete, gStr.gsHelp);
        }

        private void btnCancel_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancel, gStr.gsHelp);
        }

        private void btnNewABLine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnNewABLine, gStr.gsHelp);
        }

        private void btnListUse_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnListUse, gStr.gsHelp);
        }

        private void btnSwapAB_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ht_btnSwapAB, gStr.gsHelp);
        }

        private void btnEditName_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_tboxNameLine, gStr.gsHelp);
        }

        private void btnDuplicate_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnDuplicate, gStr.gsHelp);
        }

        private void btnAPoint_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnAPoint, gStr.gsHelp);
        }

        private void btnBPoint_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnBPoint, gStr.gsHelp);
        }

        private void nudHeading_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_nudHeading, gStr.gsHelp);
        }

        private void btnManual_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnManual, gStr.gsHelp);
        }

        private void textBox1_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_textBox1, gStr.gsHelp);
        }

        private void btnAddTime_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnAddTime, gStr.gsHelp);
        }

        private void btnCancel_APlus_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancelCreate, gStr.gsHelp);
        }

        private void btnCancel_Name_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancelCreate, gStr.gsHelp);
        }

        private void btnPausePlay_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hcur_btnPausePlay, gStr.gsHelp);
        }

        private void btnEnter_APlus_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnEnterContinue, gStr.gsHelp);
        }

        private void btnAdd_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnEnterContinue, gStr.gsHelp);
        }
        #endregion
    }
}
