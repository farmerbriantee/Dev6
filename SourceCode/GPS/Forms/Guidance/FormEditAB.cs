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

            tboxHeading.Text = Math.Round(glm.toDegrees(mf.gyd.abHeading), 5).ToString();

            mf.panelRight.Enabled = false;
        }

        private void tboxHeading_Click(object sender, EventArgs e)
        {
            tboxHeading.Text = "";

            using (FormNumeric form = new FormNumeric(0, 360, Math.Round(glm.toDegrees(mf.gyd.abHeading), 5)))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    tboxHeading.Text = ((double)form.ReturnValue).ToString();
                    mf.gyd.abHeading = glm.toRadians((double)form.ReturnValue);
                    mf.gyd.SetABLineByHeading();
                }
                else tboxHeading.Text = Math.Round(glm.toDegrees(mf.gyd.abHeading), 5).ToString();

            }

            mf.gyd.isABValid = false;
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
                mf.gyd.MoveABCurve(snapAdj);
            else
                mf.gyd.MoveABLine(snapAdj);
        }

        private void btnAdjLeft_Click(object sender, EventArgs e)
        {
            if (isCurve)
                mf.gyd.MoveABCurve(-snapAdj);
            else
                mf.gyd.MoveABLine(-snapAdj);
        }

        private void bntOk_Click(object sender, EventArgs e)
        {
            if (isCurve)
            {
                if (mf.gyd.refList.Count > 0)
                {
                    //array number is 1 less since it starts at zero
                    int idx = mf.gyd.numCurveLineSelected - 1;

                    if (idx >= 0)
                    {
                        mf.gyd.curveArr[idx].aveHeading = mf.gyd.aveLineHeading;
                        mf.gyd.curveArr[idx].curvePts.Clear();
                        //write out the Curve Points
                        foreach (vec3 item in mf.gyd.refList)
                        {
                            mf.gyd.curveArr[idx].curvePts.Add(item);
                        }
                    }

                    //save entire list
                    mf.FileSaveCurveLines();
                }
                mf.gyd.moveDistance = 0;
                mf.gyd.isCurveValid = false;
            }
            else
            {
                //index to last one. 
                int idx = mf.gyd.numABLineSelected - 1;

                if (idx >= 0)
                {
                    mf.gyd.lineArr[idx].heading = mf.gyd.abHeading;
                    //calculate the new points for the reference line and points
                    mf.gyd.lineArr[idx].origin.easting = mf.gyd.refPoint1.easting;
                    mf.gyd.lineArr[idx].origin.northing = mf.gyd.refPoint1.northing;
                }

                mf.FileSaveABLines();

                mf.gyd.moveDistance = 0;
                mf.gyd.isABValid = false;
            }

            isClosing = true;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (isCurve)
            {
                int last = mf.gyd.numCurveLineSelected;
                mf.FileLoadCurveLines();

                if (mf.gyd.curveArr.Count > 0)
                {
                    mf.gyd.numCurveLineSelected = last;
                    int idx = mf.gyd.numCurveLineSelected - 1;
                    mf.gyd.aveLineHeading = mf.gyd.curveArr[idx].aveHeading;

                    mf.gyd.refList?.Clear();
                    for (int i = 0; i < mf.gyd.curveArr[idx].curvePts.Count; i++)
                    {
                        mf.gyd.refList.Add(mf.gyd.curveArr[idx].curvePts[i]);
                    }
                    mf.gyd.isCurveSet = true;
                }
                mf.gyd.isCurveValid = false;
            }
            else
            {
                int last = mf.gyd.numABLineSelected;
                mf.FileLoadABLines();

                mf.gyd.numABLineSelected = last;
                mf.gyd.refPoint1 = mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].origin;
                mf.gyd.abHeading = mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].heading;
                mf.gyd.SetABLineByHeading();
                mf.gyd.isABLineSet = true;
                mf.gyd.isABLineLoaded = true;
                mf.gyd.moveDistance = 0;
                mf.gyd.isABValid = false;
            }

            isClosing = true;
            Close();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (isCurve)
            {
                mf.gyd.isCurveValid = false;
                mf.gyd.lastSecond = 0;
                int cnt = mf.gyd.refList.Count;
                if (cnt > 0)
                {
                    mf.gyd.refList.Reverse();

                    vec3[] arr = new vec3[cnt];
                    cnt--;
                    mf.gyd.refList.CopyTo(arr);
                    mf.gyd.refList.Clear();

                    mf.gyd.aveLineHeading += Math.PI;
                    if (mf.gyd.aveLineHeading < 0) mf.gyd.aveLineHeading += glm.twoPI;
                    if (mf.gyd.aveLineHeading > glm.twoPI) mf.gyd.aveLineHeading -= glm.twoPI;

                    for (int i = 1; i < cnt; i++)
                    {
                        vec3 pt3 = arr[i];
                        pt3.heading += Math.PI;
                        if (pt3.heading > glm.twoPI) pt3.heading -= glm.twoPI;
                        if (pt3.heading < 0) pt3.heading += glm.twoPI;
                        mf.gyd.refList.Add(pt3);
                    }
                }
            }
            else
            {
                mf.gyd.abHeading += Math.PI;
                if (mf.gyd.abHeading > glm.twoPI) mf.gyd.abHeading -= glm.twoPI;

                mf.gyd.refABLineP1.easting = mf.gyd.refPoint1.easting - (Math.Sin(mf.gyd.abHeading) * mf.gyd.abLength);
                mf.gyd.refABLineP1.northing = mf.gyd.refPoint1.northing - (Math.Cos(mf.gyd.abHeading) * mf.gyd.abLength);
                mf.gyd.refABLineP2.easting = mf.gyd.refPoint1.easting + (Math.Sin(mf.gyd.abHeading) * mf.gyd.abLength);
                mf.gyd.refABLineP2.northing = mf.gyd.refPoint1.northing + (Math.Cos(mf.gyd.abHeading) * mf.gyd.abLength);

                mf.gyd.refPoint2.easting = mf.gyd.refABLineP2.easting;
                mf.gyd.refPoint2.northing = mf.gyd.refABLineP2.northing;
                tboxHeading.Text = Math.Round(glm.toDegrees(mf.gyd.abHeading), 5).ToString();
                mf.gyd.isABValid = false;
            }
        }

        private void btnContourPriority_Click(object sender, EventArgs e)
        {
            if (isCurve && mf.gyd.isBtnCurveOn)
                mf.gyd.MoveABCurve(mf.isStanleyUsed ? mf.gyd.distanceFromCurrentLinePivot : mf.gyd.distanceFromCurrentLinePivot);
            else if (!isCurve && mf.gyd.isABLineSet)
                mf.gyd.MoveABLine(mf.gyd.distanceFromCurrentLinePivot);
        }

        private void btnRightHalfWidth_Click(object sender, EventArgs e)
        {
            if (isCurve)
                mf.gyd.MoveABCurve(mf.tool.toolWidth * 0.5);
            else
               mf.gyd.MoveABLine(mf.tool.toolWidth * 0.5);
        }

        private void btnLeftHalfWidth_Click(object sender, EventArgs e)
        {
            if (isCurve)
                mf.gyd.MoveABCurve(mf.tool.toolWidth * -0.5);
            else
                mf.gyd.MoveABLine(mf.tool.toolWidth * -0.5);
        }

        private void btnNoSave_Click(object sender, EventArgs e)
        {
            isClosing = true;
            mf.gyd.isABValid = false;
            mf.gyd.isCurveValid = false;
            Close();
        }

        private void cboxDegrees_SelectedIndexChanged(object sender, EventArgs e)
        {
            mf.gyd.abHeading = glm.toRadians(double.Parse(cboxDegrees.SelectedItem.ToString()));
            mf.gyd.SetABLineByHeading();
            tboxHeading.Text = Math.Round(glm.toDegrees(mf.gyd.abHeading), 5).ToString();
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
