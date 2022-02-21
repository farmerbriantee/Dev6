using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormEditAB : Form
    {
        private readonly FormGPS mf = null;

        private double snapAdj = 0;
        CGuidanceLine currentLine;
        private bool isClosing;
        private int smoothCount = 20;

        public FormEditAB(Form callingForm, CGuidanceLine _currentLine)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            currentLine = _currentLine;

            Text = currentLine.mode.HasFlag(Mode.AB) ? gStr.gsEditABLine : gStr.gsEditABCurve;
            nudMinTurnRadius.Controls[0].Enabled = false;

            tboxHeading.Visible = cboxDegrees.Visible = currentLine.mode.HasFlag(Mode.AB);
            btnSouth.Visible = lblSmooth.Visible = btnNorth.Visible = !currentLine.mode.HasFlag(Mode.AB);
        }

        private void FormEditAB_Load(object sender, EventArgs e)
        {
            if (mf.isMetric)
            {
                nudMinTurnRadius.DecimalPlaces = 0;
                nudMinTurnRadius.Value = (int)((double)Properties.Settings.Default.setAS_snapDistance * mf.cm2CmOrIn);
            }
            else
            {
                nudMinTurnRadius.DecimalPlaces = 1;
                nudMinTurnRadius.Value = (decimal)Math.Round(((double)Properties.Settings.Default.setAS_snapDistance * mf.cm2CmOrIn), 1);
            }

            label1.Text = mf.unitsInCm;
            lblHalfSnapFtM.Text = mf.unitsFtM;
            lblHalfWidth.Text = (mf.tool.toolWidth * 0.5 * mf.m2FtOrM).ToString("N2");

            if (currentLine != null && currentLine.curvePts.Count > 0)
                tboxHeading.Text = Math.Round(glm.toDegrees(currentLine.curvePts[0].heading), 5).ToString();

            mf.panelRight.Enabled = false;
        }

        private void tboxHeading_Click(object sender, EventArgs e)
        {
            mf.gyd.isValid = false;

            if (currentLine != null && currentLine.curvePts.Count > 0)
            {
                using (FormNumeric form = new FormNumeric(0, 360, Math.Round(glm.toDegrees(currentLine.curvePts[0].heading), 5)))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        tboxHeading.Text = ((double)form.ReturnValue).ToString();
                        double heading = glm.toRadians((double)form.ReturnValue);

                        currentLine.curvePts[0] = new vec3(currentLine.curvePts[0].easting, currentLine.curvePts[0].northing, heading);
                        currentLine.curvePts[1] = new vec3(currentLine.curvePts[0].easting + Math.Sin(heading), currentLine.curvePts[0].northing + Math.Cos(heading), heading);
                    }
                }
            }
        }

        private void nudMinTurnRadius_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
        }

        private void nudMinTurnRadius_ValueChanged(object sender, EventArgs e)
        {
            snapAdj = (double)nudMinTurnRadius.Value * mf.inOrCm2Cm * 0.01;
        }

        private void btnAdjRight_Click(object sender, EventArgs e)
        {
            mf.gyd.MoveGuidanceLine(currentLine, snapAdj);
        }

        private void btnAdjLeft_Click(object sender, EventArgs e)
        {
            mf.gyd.MoveGuidanceLine(currentLine, -snapAdj);
        }

        private void bntOk_Click(object sender, EventArgs e)
        {
            if (currentLine != null)
            {
                int idx = mf.gyd.curveArr.FindIndex(x => x.Name == currentLine?.Name);
                if (idx > -1)
                    mf.gyd.curveArr[idx] = new CGuidanceLine(currentLine);

                //save entire list
                if (currentLine.mode.HasFlag(Mode.AB))
                    mf.FileSaveABLines();
                else
                    mf.FileSaveCurveLines();
            }
            mf.gyd.moveDistance = 0;
            mf.gyd.isValid = false;

            isClosing = true;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.gyd.moveDistance = 0;
            mf.gyd.isValid = false;

            CGuidanceLine New = mf.gyd.curveArr.Find(x => x.Name == currentLine?.Name);
            if (New != null)
            {
                if (New.mode.HasFlag(Mode.AB))
                    mf.gyd.currentGuidanceLine = mf.gyd.currentABLine = new CGuidanceLine(New);
                else
                    mf.gyd.currentGuidanceLine = mf.gyd.currentCurveLine = new CGuidanceLine(New);
            }
            else
                mf.gyd.currentGuidanceLine = null;

            isClosing = true;
            Close();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (currentLine != null)
            {
                mf.gyd.isValid = false;
                mf.gyd.ReverseGuidanceLine(currentLine);

                if (currentLine.curvePts.Count > 0)
                    tboxHeading.Text = Math.Round(glm.toDegrees(currentLine.curvePts[0].heading), 5).ToString();
            }
        }

        private void btnContourPriority_Click(object sender, EventArgs e)
        {
            mf.gyd.MoveGuidanceLine(currentLine, mf.gyd.distanceFromCurrentLinePivot);
        }

        private void btnRightHalfWidth_Click(object sender, EventArgs e)
        {
            mf.gyd.MoveGuidanceLine(currentLine, mf.tool.toolWidth * 0.5);
        }

        private void btnLeftHalfWidth_Click(object sender, EventArgs e)
        {
            mf.gyd.MoveGuidanceLine(currentLine, mf.tool.toolWidth * -0.5);
        }

        private void btnNoSave_Click(object sender, EventArgs e)
        {
            isClosing = true;
            mf.gyd.isValid = false;
            Close();
        }

        private void btnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            if (--smoothCount < 2) smoothCount = 2;
            SmoothAB(smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
            mf.gyd.isSmoothWindowOpen = true;
        }

        private void btnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            if (++smoothCount > 100) smoothCount = 100;
            SmoothAB(smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
            mf.gyd.isSmoothWindowOpen = true;
        }

        private void cboxDegrees_SelectedIndexChanged(object sender, EventArgs e)
        {
            double heading = glm.toRadians(double.Parse(cboxDegrees.SelectedItem.ToString()));

            if (currentLine != null && currentLine.curvePts.Count > 0)
            {
                currentLine.curvePts[0] = new vec3(currentLine.curvePts[0].easting, currentLine.curvePts[0].northing, heading);
                currentLine.curvePts[1] = new vec3(currentLine.curvePts[0].easting + Math.Sin(heading), currentLine.curvePts[0].northing + Math.Cos(heading), heading);
            }

            tboxHeading.Text = Math.Round(glm.toDegrees(heading), 5).ToString();
        }

        private void FormEditAB_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.gyd.isSmoothWindowOpen = false;

            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }
            mf.panelRight.Enabled = true;
        }

        //for calculating for display the averaged new line
        public void SmoothAB(int smPts)
        {
            int idx = mf.gyd.curveArr.FindIndex(x => x.Name == currentLine?.Name);
            if (idx > -1)
            {
                CGuidanceLine currentLine2 = new CGuidanceLine(mf.gyd.curveArr[idx]);
                if (mf.gyd.moveDistance != 0)
                {
                    double old = mf.gyd.isHeadingSameWay ? mf.gyd.moveDistance : -mf.gyd.moveDistance;
                    mf.gyd.moveDistance = 0;
                    mf.gyd.MoveGuidanceLine(currentLine2, old);
                }

                //count the reference list of original curve
                int cnt = currentLine2.curvePts.Count;

                //just go back if not very long
                if (cnt < 200) return;

                //the temp array
                vec3[] arr = new vec3[cnt];

                //read the points before and after the setpoint
                for (int s = 0; s < smPts / 2; s++)
                {
                    arr[s].easting = currentLine2.curvePts[s].easting;
                    arr[s].northing = currentLine2.curvePts[s].northing;
                    arr[s].heading = currentLine2.curvePts[s].heading;
                }

                for (int s = cnt - (smPts / 2); s < cnt; s++)
                {
                    arr[s].easting = currentLine2.curvePts[s].easting;
                    arr[s].northing = currentLine2.curvePts[s].northing;
                    arr[s].heading = currentLine2.curvePts[s].heading;
                }

                //average them - center weighted average
                for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
                {
                    for (int j = -smPts / 2; j < smPts / 2; j++)
                    {
                        arr[i].easting += currentLine2.curvePts[j + i].easting;
                        arr[i].northing += currentLine2.curvePts[j + i].northing;
                    }
                    arr[i].easting /= smPts;
                    arr[i].northing /= smPts;
                    arr[i].heading = currentLine2.curvePts[i].heading;
                }

                if (arr == null || cnt < 1) return;

                currentLine2.curvePts.Clear();
                currentLine2.curvePts.AddRange(arr);
                currentLine2.curvePts.CalculateHeadings(currentLine2.mode.HasFlag(Mode.Boundary));

                mf.gyd.isValid = false;
                mf.gyd.currentGuidanceLine = mf.gyd.currentCurveLine = currentLine = currentLine2;
            }
        }

        private void btnCancel_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancel, gStr.gsHelp);
        }

        private void btnNoSave_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.he_btnNoSave, gStr.gsHelp);
        }

        private void btnOK_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.he_btnOK, gStr.gsHelp);
        }

        private void btnContourPriority_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.h_btnSnapToPivot, gStr.gsHelp);
        }
    }
}
