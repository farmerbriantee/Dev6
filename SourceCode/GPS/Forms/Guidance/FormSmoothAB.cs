using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSmoothAB : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private int smoothCount = 20;

        public FormSmoothAB(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.bntOK.Text = gStr.gsForNow;
            this.btnSave.Text = gStr.gsToFile;

            this.Text = gStr.gsSmoothABCurve;
        }

        private void FormSmoothAB_Load(object sender, EventArgs e)
        {
            mf.gyd.isSmoothWindowOpen = true;
            smoothCount = 20;
            lblSmooth.Text = "**";
        }

        private void btnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            if (++smoothCount > 100) smoothCount = 100;
            SmoothAB(smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
        }

        private void btnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            if (--smoothCount < 2) smoothCount = 2;
            SmoothAB(smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
        }

        //for calculating for display the averaged new line
        public void SmoothAB(int smPts)
        {
            if (mf.gyd.currentCurveLine != null)
            {
                //count the reference list of original curve
                int cnt = mf.gyd.currentCurveLine.curvePts.Count;

                //just go back if not very long
                if (cnt < 200) return;

                //the temp array
                vec3[] arr = new vec3[cnt];

                //read the points before and after the setpoint
                for (int s = 0; s < smPts / 2; s++)
                {
                    arr[s].easting = mf.gyd.currentCurveLine.curvePts[s].easting;
                    arr[s].northing = mf.gyd.currentCurveLine.curvePts[s].northing;
                    arr[s].heading = mf.gyd.currentCurveLine.curvePts[s].heading;
                }

                for (int s = cnt - (smPts / 2); s < cnt; s++)
                {
                    arr[s].easting = mf.gyd.currentCurveLine.curvePts[s].easting;
                    arr[s].northing = mf.gyd.currentCurveLine.curvePts[s].northing;
                    arr[s].heading = mf.gyd.currentCurveLine.curvePts[s].heading;
                }

                //average them - center weighted average
                for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
                {
                    for (int j = -smPts / 2; j < smPts / 2; j++)
                    {
                        arr[i].easting += mf.gyd.currentCurveLine.curvePts[j + i].easting;
                        arr[i].northing += mf.gyd.currentCurveLine.curvePts[j + i].northing;
                    }
                    arr[i].easting /= smPts;
                    arr[i].northing /= smPts;
                    arr[i].heading = mf.gyd.currentCurveLine.curvePts[i].heading;
                }

                if (arr == null || cnt < 1) return;

                //make a list to draw
                mf.gyd.EditGuidanceLine = new CGuidanceLine(mf.gyd.currentCurveLine.mode);
                mf.gyd.EditGuidanceLine.curvePts.AddRange(arr);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSmoothAsRefList();

            if (mf.gyd.currentCurveLine != null)
            {
                int idx = mf.gyd.curveArr.FindIndex(x => x.Name == mf.gyd.currentCurveLine.Name);
                if (idx > -1)
                {
                    mf.gyd.curveArr[idx] = new CGuidanceLine(mf.gyd.currentCurveLine);

                    //save entire list
                    mf.FileSaveCurveLines();
                }

                mf.gyd.moveDistance = 0;
            }

            Close();
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            SaveSmoothAsRefList();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        //turning the visual line into the real reference line to use
        public void SaveSmoothAsRefList()
        {
            //oops no smooth list generated
            if (mf.gyd.EditGuidanceLine != null)
            {
                int cnt = mf.gyd.EditGuidanceLine.curvePts.Count;
                if (cnt > 1)
                {
                    mf.gyd.isValid = false;
                    mf.gyd.moveDistance = 0;

                    mf.gyd.currentGuidanceLine = mf.gyd.currentCurveLine = mf.gyd.EditGuidanceLine;
                    mf.gyd.EditGuidanceLine.curvePts.CalculateHeadings(mf.gyd.EditGuidanceLine.mode.HasFlag(Mode.Boundary));
                }
            }
        }

        private void FormSmoothAB_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.gyd.isSmoothWindowOpen = false;
            mf.gyd.EditGuidanceLine = null;
        }
    }
}