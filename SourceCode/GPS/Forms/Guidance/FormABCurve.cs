using System;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormABCurve : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;
        private double aveLineHeading;
        private int originalLine = 0;
        private bool isClosing;

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

            originalLine = mf.gyd.numCurveLineSelected;
            mf.gyd.isOkToAddDesPoints = false;

            UpdateLineList();

            if (lvLines.Items.Count > 0 && originalLine > 0)
            {
                lvLines.Items[originalLine - 1].EnsureVisible();
                lvLines.Items[originalLine - 1].Selected = true;
                lvLines.Select();
            }

        }

        private void UpdateLineList()
        {
            lvLines.Clear();
            ListViewItem itm;

            foreach (CCurveLines item in mf.gyd.curveArr)
            {
                itm = new ListViewItem(item.Name);
                lvLines.Items.Add(itm);
            }

            // go to bottom of list - if there is a bottom
            if (lvLines.Items.Count > 0)
            {
                lvLines.Items[lvLines.Items.Count - 1].EnsureVisible();
                lvLines.Items[lvLines.Items.Count - 1].Selected = true;
                lvLines.Select();
            }
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
            mf.gyd.desList?.Clear();

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
            aveLineHeading = 0;
            mf.gyd.isOkToAddDesPoints = false;
            panelAPlus.Visible = false;
            panelName.Visible = true;

            int cnt = mf.gyd.desList.Count;
            if (cnt > 3)
            {
                //make sure distance isn't too big between points on Turn
                for (int i = 0; i < cnt - 1; i++)
                {
                    int j = i + 1;
                    //if (j == cnt) j = 0;
                    double distance = glm.Distance(mf.gyd.desList[i], mf.gyd.desList[j]);
                    if (distance > 1.2)
                    {
                        vec3 pointB = new vec3((mf.gyd.desList[i].easting + mf.gyd.desList[j].easting) / 2.0,
                            (mf.gyd.desList[i].northing + mf.gyd.desList[j].northing) / 2.0,
                            mf.gyd.desList[i].heading);

                        mf.gyd.desList.Insert(j, pointB);
                        cnt = mf.gyd.desList.Count;
                        i = -1;
                    }
                }

                //calculate average heading of line
                double x = 0, y = 0;
                foreach (vec3 pt in mf.gyd.desList)
                {
                    x += Math.Cos(pt.heading);
                    y += Math.Sin(pt.heading);
                }
                x /= mf.gyd.desList.Count;
                y /= mf.gyd.desList.Count;
                aveLineHeading = Math.Atan2(y, x);
                if (aveLineHeading < 0) aveLineHeading += glm.twoPI;

                //build the tail extensions
                AddFirstLastPoints();
                SmoothAB(4);
                CalculateTurnHeadings();

                panelAPlus.Visible = false;
                panelName.Visible = true;

                mf.gyd.desName = "Cu " +
                    (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture) +
                    "\u00B0 " + mf.FindDirection(aveLineHeading);

                textBox1.Text = mf.gyd.desName;
            }
            else
            {
                mf.gyd.isOkToAddDesPoints = false;
                mf.gyd.desList?.Clear();

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
            mf.gyd.desName = textBox1.Text;
        }

        private void btnPausePlay_Click(object sender, EventArgs e)
        {
            if (mf.gyd.isOkToAddDesPoints)
            {
                mf.gyd.isOkToAddDesPoints = false;
                btnPausePlay.Image = Properties.Resources.BoundaryRecord;
                //btnPausePlay.Text = gStr.gsRecord;
                btnBPoint.Enabled = false;
            }
            else
            {
                mf.gyd.isOkToAddDesPoints = true;
                btnPausePlay.Image = Properties.Resources.boundaryPause;
                //btnPausePlay.Text = gStr.gsPause;
                btnBPoint.Enabled = true;
            }
        }


        private void btnCancelMain_Click(object sender, EventArgs e)
        {
            isClosing = true;
            mf.gyd.isCurveValid = false;
            mf.gyd.moveDistance = 0;
            mf.gyd.isOkToAddDesPoints = false;
            mf.gyd.isCurveSet = false;
            mf.gyd.refList?.Clear();
            mf.gyd.isCurveSet = false;
            mf.DisableYouTurnButtons();
            //mf.btnContourPriority.Enabled = false;
            //mf.curve.ResetCurveLine();
            mf.gyd.isBtnCurveOn = false;
            mf.btnCurve.Image = Properties.Resources.CurveOff;
            if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
            if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();

            mf.gyd.numCurveLineSelected = 0;
            Close();
        }

        private void btnCancelCurve_Click(object sender, EventArgs e)
        {
            mf.gyd.isOkToAddDesPoints = false;
            mf.gyd.desList?.Clear();

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
            if (mf.gyd.desList.Count > 0)
            {
                if (textBox1.Text.Length == 0) textBox2.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

                mf.gyd.curveArr.Add(new CCurveLines());

                //array number is 1 less since it starts at zero
                int idx = mf.gyd.curveArr.Count - 1;

                mf.gyd.curveArr[idx].Name = textBox1.Text.Trim();
                mf.gyd.curveArr[idx].aveHeading = aveLineHeading;

                //write out the Curve Points
                foreach (vec3 item in mf.gyd.desList)
                {
                    mf.gyd.curveArr[idx].curvePts.Add(item);
                }

                mf.FileSaveCurveLines();
                mf.gyd.desList?.Clear();
            }

            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
            lvLines.Focus();
            mf.gyd.desList?.Clear();
        }

        private void btnListDelete_Click(object sender, EventArgs e)
        {
            mf.gyd.moveDistance = 0;

            if (lvLines.SelectedItems.Count > 0)
            {
                int num = lvLines.SelectedIndices[0];
                mf.gyd.curveArr.RemoveAt(num);
                lvLines.SelectedItems[0].Remove();

                //everything changed, so make sure its right
                mf.gyd.numCurveLines = mf.gyd.curveArr.Count;
                if (mf.gyd.numCurveLineSelected > mf.gyd.numCurveLines) mf.gyd.numCurveLineSelected = mf.gyd.numCurveLines;

                //if there are no saved oned, empty out current curve line and turn off
                if (mf.gyd.numCurveLines == 0)
                {
                    mf.gyd.ResetCurveLine();
                    if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                    if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                }

                mf.FileSaveCurveLines();
            }

            UpdateLineList();
            lvLines.Focus();
        }

        private void btnListUse_Click(object sender, EventArgs e)
        {
            isClosing = true;
            //reset to generate new reference
            mf.gyd.isCurveValid = false;
            mf.gyd.moveDistance = 0;

            if (lvLines.SelectedItems.Count > 0)
            {

                int idx = lvLines.SelectedIndices[0];
                mf.gyd.numCurveLineSelected = idx + 1;


                mf.gyd.aveLineHeading = mf.gyd.curveArr[idx].aveHeading;
                mf.gyd.refList?.Clear();
                for (int i = 0; i < mf.gyd.curveArr[idx].curvePts.Count; i++)
                {
                    mf.gyd.refList.Add(mf.gyd.curveArr[idx].curvePts[i]);
                }
                mf.gyd.isCurveSet = true;
                mf.gyd.ResetYouTurn();

                Close();
            }
            else
            {
                mf.gyd.moveDistance = 0;
                mf.gyd.isOkToAddDesPoints = false;
                mf.gyd.isCurveSet = false;
                mf.gyd.refList?.Clear();
                mf.gyd.isCurveSet = false;
                mf.DisableYouTurnButtons();
                mf.gyd.isBtnCurveOn = false;
                mf.btnCurve.Image = Properties.Resources.CurveOff;
                if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();

                mf.gyd.numCurveLineSelected = 0;
                Close();
            }
        }

        public void SmoothAB(int smPts)
        {
            //count the reference list of original curve
            int cnt = mf.gyd.desList.Count;

            //the temp array
            vec3[] arr = new vec3[cnt];

            //read the points before and after the setpoint
            for (int s = 0; s < smPts / 2; s++)
            {
                arr[s].easting = mf.gyd.desList[s].easting;
                arr[s].northing = mf.gyd.desList[s].northing;
                arr[s].heading = mf.gyd.desList[s].heading;
            }

            for (int s = cnt - (smPts / 2); s < cnt; s++)
            {
                arr[s].easting = mf.gyd.desList[s].easting;
                arr[s].northing = mf.gyd.desList[s].northing;
                arr[s].heading = mf.gyd.desList[s].heading;
            }

            //average them - center weighted average
            for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
            {
                for (int j = -smPts / 2; j < smPts / 2; j++)
                {
                    arr[i].easting += mf.gyd.desList[j + i].easting;
                    arr[i].northing += mf.gyd.desList[j + i].northing;
                }
                arr[i].easting /= smPts;
                arr[i].northing /= smPts;
                arr[i].heading = mf.gyd.desList[i].heading;
            }

            //make a list to draw
            mf.gyd.desList?.Clear();
            for (int i = 0; i < cnt; i++)
            {
                mf.gyd.desList.Add(arr[i]);
            }
        }

        public void AddFirstLastPoints()
        {
            int ptCnt = mf.gyd.desList.Count - 1;
            for (int i = 1; i < 200; i++)
            {
                vec3 pt = new vec3(mf.gyd.desList[ptCnt]);
                pt.easting += (Math.Sin(pt.heading) * i);
                pt.northing += (Math.Cos(pt.heading) * i);
                mf.gyd.desList.Add(pt);
            }

            //and the beginning
            vec3 start = new vec3(mf.gyd.desList[0]);
            for (int i = 1; i < 200; i++)
            {
                vec3 pt = new vec3(start);
                pt.easting -= (Math.Sin(pt.heading) * i);
                pt.northing -= (Math.Cos(pt.heading) * i);
                mf.gyd.desList.Insert(0, pt);
            }
        }

        public void CalculateTurnHeadings()
        {
            //to calc heading based on next and previous points to give an average heading.
            int cnt = mf.gyd.desList.Count;
            if (cnt > 0)
            {
                vec3[] arr = new vec3[cnt];
                cnt--;
                mf.gyd.desList.CopyTo(arr);
                mf.gyd.desList.Clear();

                //middle points
                for (int i = 1; i < cnt; i++)
                {
                    vec3 pt3 = arr[i];
                    pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                    if (pt3.heading < 0) pt3.heading += glm.twoPI;
                    mf.gyd.desList.Add(pt3);
                }
            }
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.SelectedIndices[0];


                panelPick.Visible = false;
                panelName.Visible = true;
                this.Size = new System.Drawing.Size(270, 360);

                panelAPlus.Visible = false;
                panelName.Visible = true;

                textBox1.Text = mf.gyd.curveArr[idx].Name + " Copy";
                mf.gyd.desName = textBox1.Text;

                aveLineHeading = mf.gyd.curveArr[idx].aveHeading;
                mf.gyd.desList?.Clear();

                for (int i = 0; i < mf.gyd.curveArr[idx].curvePts.Count; i++)
                {
                    vec3 pt = new vec3(mf.gyd.curveArr[idx].curvePts[i]);
                    mf.gyd.desList.Add(pt);
                }
            }
        }

        private void btnEditName_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.SelectedIndices[0];
                textBox2.Text = mf.gyd.curveArr[idx].Name;

                panelPick.Visible = false;
                panelEditName.Visible = true;
                this.Size = new System.Drawing.Size(270, 360);
            }
        }

        private void btnAddTimeEdit_Click(object sender, EventArgs e)
        {
            textBox2.Text += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
        }

        private void btnSaveEditName_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "") textBox2.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            int idx = lvLines.SelectedIndices[0];
            mf.gyd.curveArr[idx].Name = textBox2.Text.Trim();

            panelEditName.Visible = false;
            panelPick.Visible = true;

            mf.FileSaveCurveLines();
            mf.gyd.desList?.Clear();

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
            lvLines.Focus();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.SelectedIndices[0];

                int cnt = mf.gyd.curveArr[idx].curvePts.Count;
                if (cnt > 0)
                {
                    mf.gyd.curveArr[idx].curvePts.Reverse();

                    vec3[] arr = new vec3[cnt];
                    cnt--;
                    mf.gyd.curveArr[idx].curvePts.CopyTo(arr);
                    mf.gyd.curveArr[idx].curvePts.Clear();

                    mf.gyd.curveArr[idx].aveHeading += Math.PI;
                    if (mf.gyd.curveArr[idx].aveHeading < 0) mf.gyd.curveArr[idx].aveHeading += glm.twoPI;
                    if (mf.gyd.curveArr[idx].aveHeading > glm.twoPI) mf.gyd.curveArr[idx].aveHeading -= glm.twoPI;

                    for (int i = 1; i < cnt; i++)
                    {
                        vec3 pt3 = arr[i];
                        pt3.heading += Math.PI;
                        if (pt3.heading > glm.twoPI) pt3.heading -= glm.twoPI;
                        if (pt3.heading < 0) pt3.heading += glm.twoPI;
                        mf.gyd.curveArr[idx].curvePts.Add(pt3);
                    }
                }

                mf.FileSaveCurveLines();
                UpdateLineList();
                lvLines.Focus();

                _ = new FormTimedMessage(1500, "A B Swapped", "Curve is Reversed");
            }
        }

        private void FormABCurve_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }
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