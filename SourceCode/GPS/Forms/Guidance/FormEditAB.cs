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
        double heading = 0;

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
                nudMinTurnRadius.Value = (int)((double)Properties.Settings.Default.setAS_snapDistance * mf.mToUser);
            }
            else
            {
                nudMinTurnRadius.DecimalPlaces = 1;
                nudMinTurnRadius.Value = (decimal)Math.Round(((double)Properties.Settings.Default.setAS_snapDistance * mf.mToUser), 1);
            }

            label1.Text = mf.unitsInCm;
            lblHalfSnapFtM.Text = mf.unitsFtM;
            lblHalfWidth.Text = (mf.tool.toolWidth * 0.5 * mf.mToUserBig).ToString("0.00");

            if (currentLine != null && currentLine.points.Count > 1)
            {
                double dx = currentLine.points[1].easting - currentLine.points[0].easting;
                double dy = currentLine.points[1].northing - currentLine.points[0].northing;

                heading = Math.Atan2(dx, dy);
                if (heading < 0) heading += glm.twoPI;
                tboxHeading.Text = glm.toDegrees(heading).ToString("0.00000");
            }

            mf.panelRight.Enabled = false;
        }

        private void tboxHeading_Click(object sender, EventArgs e)
        {
            mf.gyd.isValid = false;

            using (FormNumeric form = new FormNumeric(0, 360, Math.Round(glm.toDegrees(heading), 5), 5))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    tboxHeading.Text = ((double)form.ReturnValue).ToString();
                    heading = glm.toRadians((double)form.ReturnValue);

                    if (currentLine != null && currentLine.points.Count > 1)
                    {
                        currentLine.points[0] = new vec2(currentLine.points[0].easting, currentLine.points[0].northing);
                        currentLine.points[1] = new vec2(currentLine.points[0].easting + Math.Sin(heading), currentLine.points[0].northing + Math.Cos(heading));
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
            snapAdj = (double)nudMinTurnRadius.Value * mf.userToM;
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

            int idx = mf.gyd.curveArr.FindIndex(x => x.Name == currentLine?.Name);
            if (idx > -1)
            {
                if (mf.gyd.curveArr[idx].mode.HasFlag(Mode.AB))
                    mf.gyd.currentGuidanceLine = mf.gyd.currentABLine = new CGuidanceLine(mf.gyd.curveArr[idx]);
                else
                    mf.gyd.currentGuidanceLine = mf.gyd.currentCurveLine = new CGuidanceLine(mf.gyd.curveArr[idx]);
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

                if (currentLine.points.Count > 1)
                {
                    double dx = currentLine.points[1].easting - currentLine.points[0].easting;
                    double dy = currentLine.points[1].northing - currentLine.points[0].northing;

                    heading = Math.Atan2(dx, dy);
                    if (heading < 0) heading += glm.twoPI;

                    tboxHeading.Text = glm.toDegrees(heading).ToString("0.00000");
                }
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
            heading = glm.toRadians(double.Parse(cboxDegrees.SelectedItem.ToString()));

            if (currentLine != null && currentLine.points.Count > 0)
            {
                currentLine.points[0] = new vec2(currentLine.points[0].easting, currentLine.points[0].northing);
                currentLine.points[1] = new vec2(currentLine.points[0].easting + Math.Sin(heading), currentLine.points[0].northing + Math.Cos(heading));
            }

            tboxHeading.Text = glm.toDegrees(heading).ToString("0.00000");
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


                vec2[] arr = currentLine2.points.SmoothAB(smPts);
                currentLine2.points.Clear();
                currentLine2.points.AddRange(arr);

                mf.gyd.isValid = false;
                mf.gyd.currentGuidanceLine = mf.gyd.currentCurveLine = currentLine = currentLine2;
            }
        }

        private void btnCancel_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ha_btnCancel, gStr.gsHelp).ShowDialog(this);
        }

        private void btnNoSave_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.he_btnNoSave, gStr.gsHelp).ShowDialog(this);
        }

        private void btnOK_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.he_btnOK, gStr.gsHelp).ShowDialog(this);
        }

        private void btnContourPriority_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_btnSnapToPivot, gStr.gsHelp).ShowDialog(this);
        }
    }
}
