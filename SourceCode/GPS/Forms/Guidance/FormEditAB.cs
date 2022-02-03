using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormEditAB : Form
    {
        private readonly FormGPS mf = null;

        private double snapAdj = 0;

        private static bool isCurve;
        private bool isClosing;

        public FormEditAB(Form callingForm, bool Curve)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            this.Text = Curve ? gStr.gsEditABCurve : gStr.gsEditABLine;
            nudMinTurnRadius.Controls[0].Enabled = false;

            isCurve = Curve;
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

            tboxHeading.Visible = !isCurve;
            cboxDegrees.Visible = !isCurve;

            tboxHeading.Text = Math.Round(glm.toDegrees(mf.ABLine.abHeading), 5).ToString();

            mf.panelRight.Enabled = false;
        }

        private void tboxHeading_Click(object sender, EventArgs e)
        {
            tboxHeading.Text = "";

            using (FormNumeric form = new FormNumeric(0, 360, Math.Round(glm.toDegrees(mf.ABLine.abHeading), 5)))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    tboxHeading.Text = ((double)form.ReturnValue).ToString();
                    mf.ABLine.abHeading = glm.toRadians((double)form.ReturnValue);
                    mf.ABLine.SetABLineByHeading();
                }
                else tboxHeading.Text = Math.Round(glm.toDegrees(mf.ABLine.abHeading), 5).ToString();

            }

            mf.ABLine.isABValid = false;
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
            if (isCurve)
                mf.curve.MoveABCurve(snapAdj);
            else
                mf.ABLine.MoveABLine(snapAdj);
        }

        private void btnAdjLeft_Click(object sender, EventArgs e)
        {
            if (isCurve)
                mf.curve.MoveABCurve(-snapAdj);
            else
                mf.ABLine.MoveABLine(-snapAdj);
        }

        private void bntOk_Click(object sender, EventArgs e)
        {
            if (isCurve)
            {
                if (mf.curve.refList.Count > 0)
                {
                    //array number is 1 less since it starts at zero
                    int idx = mf.curve.numCurveLineSelected - 1;

                    if (idx >= 0)
                    {
                        mf.curve.curveArr[idx].aveHeading = mf.curve.aveLineHeading;
                        mf.curve.curveArr[idx].curvePts.Clear();
                        //write out the Curve Points
                        foreach (vec3 item in mf.curve.refList)
                        {
                            mf.curve.curveArr[idx].curvePts.Add(item);
                        }
                    }

                    //save entire list
                    mf.FileSaveCurveLines();
                }
                mf.curve.moveDistance = 0;
                mf.curve.isCurveValid = false;
            }
            else
            {
                //index to last one. 
                int idx = mf.ABLine.numABLineSelected - 1;

                if (idx >= 0)
                {
                    mf.ABLine.lineArr[idx].heading = mf.ABLine.abHeading;
                    //calculate the new points for the reference line and points
                    mf.ABLine.lineArr[idx].origin.easting = mf.ABLine.refPoint1.easting;
                    mf.ABLine.lineArr[idx].origin.northing = mf.ABLine.refPoint1.northing;
                }

                mf.FileSaveABLines();

                mf.ABLine.moveDistance = 0;
                mf.ABLine.isABValid = false;
            }

            isClosing = true;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (isCurve)
            {
                int last = mf.curve.numCurveLineSelected;
                mf.FileLoadCurveLines();

                if (mf.curve.curveArr.Count > 0)
                {
                    mf.curve.numCurveLineSelected = last;
                    int idx = mf.curve.numCurveLineSelected - 1;
                    mf.curve.aveLineHeading = mf.curve.curveArr[idx].aveHeading;

                    mf.curve.refList?.Clear();
                    for (int i = 0; i < mf.curve.curveArr[idx].curvePts.Count; i++)
                    {
                        mf.curve.refList.Add(mf.curve.curveArr[idx].curvePts[i]);
                    }
                    mf.curve.isCurveSet = true;
                }
                mf.curve.isCurveValid = false;
            }
            else
            {
                int last = mf.ABLine.numABLineSelected;
                mf.FileLoadABLines();

                mf.ABLine.numABLineSelected = last;
                mf.ABLine.refPoint1 = mf.ABLine.lineArr[mf.ABLine.numABLineSelected - 1].origin;
                mf.ABLine.abHeading = mf.ABLine.lineArr[mf.ABLine.numABLineSelected - 1].heading;
                mf.ABLine.SetABLineByHeading();
                mf.ABLine.isABLineSet = true;
                mf.ABLine.isABLineLoaded = true;
                mf.ABLine.moveDistance = 0;
                mf.ABLine.isABValid = false;
            }

            isClosing = true;
            Close();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (isCurve)
            {
                mf.curve.isCurveValid = false;
                mf.curve.lastSecond = 0;
                int cnt = mf.curve.refList.Count;
                if (cnt > 0)
                {
                    mf.curve.refList.Reverse();

                    vec3[] arr = new vec3[cnt];
                    cnt--;
                    mf.curve.refList.CopyTo(arr);
                    mf.curve.refList.Clear();

                    mf.curve.aveLineHeading += Math.PI;
                    if (mf.curve.aveLineHeading < 0) mf.curve.aveLineHeading += glm.twoPI;
                    if (mf.curve.aveLineHeading > glm.twoPI) mf.curve.aveLineHeading -= glm.twoPI;

                    for (int i = 1; i < cnt; i++)
                    {
                        vec3 pt3 = arr[i];
                        pt3.heading += Math.PI;
                        if (pt3.heading > glm.twoPI) pt3.heading -= glm.twoPI;
                        if (pt3.heading < 0) pt3.heading += glm.twoPI;
                        mf.curve.refList.Add(pt3);
                    }
                }
            }
            else
            {
                mf.ABLine.abHeading += Math.PI;
                if (mf.ABLine.abHeading > glm.twoPI) mf.ABLine.abHeading -= glm.twoPI;

                mf.ABLine.refABLineP1.easting = mf.ABLine.refPoint1.easting - (Math.Sin(mf.ABLine.abHeading) * mf.ABLine.abLength);
                mf.ABLine.refABLineP1.northing = mf.ABLine.refPoint1.northing - (Math.Cos(mf.ABLine.abHeading) * mf.ABLine.abLength);
                mf.ABLine.refABLineP2.easting = mf.ABLine.refPoint1.easting + (Math.Sin(mf.ABLine.abHeading) * mf.ABLine.abLength);
                mf.ABLine.refABLineP2.northing = mf.ABLine.refPoint1.northing + (Math.Cos(mf.ABLine.abHeading) * mf.ABLine.abLength);

                mf.ABLine.refPoint2.easting = mf.ABLine.refABLineP2.easting;
                mf.ABLine.refPoint2.northing = mf.ABLine.refABLineP2.northing;
                tboxHeading.Text = Math.Round(glm.toDegrees(mf.ABLine.abHeading), 5).ToString();
                mf.ABLine.isABValid = false;
            }
        }

        private void btnContourPriority_Click(object sender, EventArgs e)
        {
            if (isCurve && mf.curve.isBtnCurveOn)
                mf.curve.MoveABCurve(mf.isStanleyUsed ? mf.gyd.distanceFromCurrentLinePivot : mf.curve.distanceFromCurrentLinePivot);
            else if (!isCurve && mf.ABLine.isABLineSet)
                mf.ABLine.MoveABLine(mf.ABLine.distanceFromCurrentLinePivot);
        }

        private void btnRightHalfWidth_Click(object sender, EventArgs e)
        {
            if (isCurve)
                mf.curve.MoveABCurve(mf.tool.toolWidth * 0.5);
            else
               mf.ABLine.MoveABLine(mf.tool.toolWidth * 0.5);
        }

        private void btnLeftHalfWidth_Click(object sender, EventArgs e)
        {
            if (isCurve)
                mf.curve.MoveABCurve(mf.tool.toolWidth * -0.5);
            else
                mf.ABLine.MoveABLine(mf.tool.toolWidth * -0.5);
        }

        private void btnNoSave_Click(object sender, EventArgs e)
        {
            isClosing = true;
            mf.ABLine.isABValid = false;
            mf.curve.isCurveValid = false;
            Close();
        }

        private void cboxDegrees_SelectedIndexChanged(object sender, EventArgs e)
        {
            mf.ABLine.abHeading = glm.toRadians(double.Parse(cboxDegrees.SelectedItem.ToString()));
            mf.ABLine.SetABLineByHeading();
            tboxHeading.Text = Math.Round(glm.toDegrees(mf.ABLine.abHeading), 5).ToString();
        }

        private void FormEditAB_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }
            mf.panelRight.Enabled = true;
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
