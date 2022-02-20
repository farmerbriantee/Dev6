using System;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormABCurve : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;
        private bool isSaving;

        public FormABCurve(Form _mf)
        {
            mf = _mf as FormGPS;
            InitializeComponent();

            //btnPausePlay.Text = gStr.gsPause;
            this.Text = gStr.gsABCurve;
        }

        private void FormABCurve_Load(object sender, EventArgs e)
        {
            panelPick.Top = 3;
            panelPick.Left = 3;
            panelAPlus.Top = 3;
            panelAPlus.Left = 3;
            panelName.Top = 3;
            panelName.Left = 3;

            panelEditName.Top = 3;
            panelEditName.Left = 3;

            panelEditName.Visible = false;

            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
        }

        private void UpdateLineList()
        {
            lvLines.Clear();
            int idx = -1;

            for (int i = 0; i < mf.gyd.curveArr.Count; i++)
            {
                if (mf.gyd.curveArr[i].mode.HasFlag(Mode.Curve) && mf.gyd.curveArr[i].curvePts.Count > 1)
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
        //for calculating for display the averaged new line

        private void btnNewCurve_Click(object sender, EventArgs e)
        {
            lvLines.SelectedItems.Clear();
            panelPick.Visible = false;
            panelAPlus.Visible = true;
            panelName.Visible = false;

            btnAPoint.Enabled = true;
            btnBPoint.Enabled = false;
            btnPausePlay.Enabled = false;
            mf.gyd.EditGuidanceLine = new CGuidanceLine(Mode.Curve);

            this.Size = new System.Drawing.Size(270, 360);
        }

        private void btnAPoint_Click(object sender, System.EventArgs e)
        {
            //mf.curve.moveDistance = 0;
            //clear out the reference list
            lblCurveExists.Text = gStr.gsDriving;
            btnBPoint.Enabled = true;
            //mf.curve.ResetCurveLine();

            btnAPoint.Enabled = false;
            mf.gyd.isOkToAddDesPoints = true;
            btnPausePlay.Enabled = true;
            btnPausePlay.Visible = true;
        }

        private void btnBPoint_Click(object sender, System.EventArgs e)
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
                mf.gyd.EditGuidanceLine.curvePts?.Clear();

                panelPick.Visible = true;
                panelAPlus.Visible = false;
                panelName.Visible = false;

                this.Size = new System.Drawing.Size(470, 360);

                UpdateLineList();
            }
        }
        private void btnAddTime_Click(object sender, EventArgs e)
        {
            textBox1.Text += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
        }

        private void btnPausePlay_Click(object sender, EventArgs e)
        {
            mf.gyd.isOkToAddDesPoints = !mf.gyd.isOkToAddDesPoints;
            btnPausePlay.Image = mf.gyd.isOkToAddDesPoints ? Properties.Resources.boundaryPause : Properties.Resources.BoundaryRecord;
        }

        private void btnCancelMain_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancelCurve_Click(object sender, EventArgs e)
        {
            mf.gyd.isOkToAddDesPoints = false;
            mf.gyd.EditGuidanceLine = null;

            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelEditName.Visible = false;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
                mf.KeyboardToText((TextBox)sender, this);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (mf.gyd.EditGuidanceLine != null && mf.gyd.EditGuidanceLine.curvePts.Count > 1)
            {
                if (textBox1.Text.Length == 0) textBox1.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
                
                string text = textBox1.Text.Trim();
                while (mf.gyd.curveArr.Exists(L => L.Name == text))//generate unique name!
                    text += " ";
                mf.gyd.EditGuidanceLine.Name = text;

                mf.gyd.curveArr.Add(mf.gyd.EditGuidanceLine);

                mf.gyd.EditGuidanceLine = null;

                mf.FileSaveCurveLines();
            }

            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
            lvLines.Focus();
        }

        private void btnListDelete_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    if (mf.gyd.currentCurveLine == mf.gyd.curveArr[idx])
                        mf.gyd.currentCurveLine = null;

                    if (mf.gyd.currentGuidanceLine == mf.gyd.curveArr[idx])
                    {
                        mf.gyd.isValid = false;
                        mf.gyd.moveDistance = 0;

                        mf.gyd.currentGuidanceLine = null;
                        if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                        if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                    }

                    mf.gyd.curveArr.RemoveAt(idx);
                    lvLines.SelectedItems[0].Remove();

                    mf.FileSaveCurveLines();
                }
            }

            UpdateLineList();
            lvLines.Focus();
        }

        private void btnListUse_Click(object sender, EventArgs e)
        {
            isSaving = true;
            Close();
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
            mf.gyd.EditGuidanceLine.curvePts?.Clear();
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

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    panelPick.Visible = false;
                    panelName.Visible = true;
                    this.Size = new System.Drawing.Size(270, 360);

                    panelAPlus.Visible = false;
                    panelName.Visible = true;

                    textBox1.Text = mf.gyd.curveArr[idx].Name.Trim() + " Copy";
                    mf.gyd.EditGuidanceLine = new CGuidanceLine(mf.gyd.curveArr[idx].mode);
                    mf.gyd.EditGuidanceLine.curvePts.AddRange(mf.gyd.curveArr[idx].curvePts.ToArray());
                }
            }
        }

        private void btnEditName_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    textBox2.Text = mf.gyd.curveArr[idx].Name.Trim();

                    panelPick.Visible = false;
                    panelEditName.Visible = true;
                    this.Size = new System.Drawing.Size(270, 360);
                }
            }
        }

        private void btnAddTimeEdit_Click(object sender, EventArgs e)
        {
            textBox2.Text += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
        }

        private void btnSaveEditName_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "") textBox2.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
            if (idx > -1)
            {
                string text = textBox2.Text.Trim();
                while (mf.gyd.curveArr.Exists(L => L.Name == text))//generate unique name!
                    text += " ";
                mf.gyd.curveArr[idx].Name = text;
            }
            panelEditName.Visible = false;
            panelPick.Visible = true;

            mf.FileSaveCurveLines();
            mf.gyd.EditGuidanceLine.curvePts?.Clear();

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
            lvLines.Focus();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                    mf.gyd.ReverseGuidanceLine(mf.gyd.curveArr[idx]);

                mf.FileSaveCurveLines();
                UpdateLineList();
                lvLines.Focus();
            }
        }

        private void FormABCurve_FormClosing(object sender, FormClosingEventArgs e)
        {
            //reset to generate new reference
            mf.gyd.isValid = false;
            mf.gyd.moveDistance = 0;
            mf.gyd.isOkToAddDesPoints = false;

            if (isSaving && lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                    mf.gyd.currentGuidanceLine = mf.gyd.currentCurveLine = new CGuidanceLine(mf.gyd.curveArr[idx]);

                mf.gyd.ResetYouTurn();
            }
            else
            {
                mf.gyd.currentGuidanceLine = mf.gyd.currentCurveLine = null;
                mf.DisableYouTurnButtons();
                mf.gyd.isBtnCurveOn = false;
                mf.btnCurve.Image = Properties.Resources.CurveOff;
                if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
            }

            mf.gyd.EditGuidanceLine = null;
        }

        #region Help

        private void btnListDelete_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnListDelete, gStr.gsHelp);
        }

        private void btnCancelMain_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancel, gStr.gsHelp);
        }

        private void btnNewCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
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

        private void btnAddTime_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnAddTime, gStr.gsHelp);
        }

        private void btnAddTimeEdit_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnAddTime, gStr.gsHelp);
        }

        private void btnCancel_Name_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancelCreate, gStr.gsHelp);
        }

        private void btnCancelCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancelCreate, gStr.gsHelp);
        }

        private void btnCancelEditName_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancelCreate, gStr.gsHelp);
        }

        private void btnAdd_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnEnterContinue, gStr.gsHelp);
        }

        private void btnSaveEditName_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnEnterContinue, gStr.gsHelp);
        }

        private void btnAPoint_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hcur_btnAPoint, gStr.gsHelp);
        }

        private void btnBPoint_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hcur_btnBPoint, gStr.gsHelp);
        }

        private void btnPausePlay_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hcur_btnPausePlay, gStr.gsHelp);
        }

        private void textBox1_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_textBox1, gStr.gsHelp);
        }

        private void textBox2_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_textBox1, gStr.gsHelp);
        }

        #endregion
    }
}