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

        private void bntOK_Click(object sender, EventArgs e)
        {
            mf.gyd.isSmoothWindowOpen = false;
            mf.gyd.SaveSmoothAsRefList();
            mf.gyd.smooList?.Clear();
            Close();
        }

        private void FormSmoothAB_Load(object sender, EventArgs e)
        {
            mf.gyd.isSmoothWindowOpen = true;
            smoothCount = 20;
            lblSmooth.Text = "**";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.gyd.isSmoothWindowOpen = false;
            mf.gyd.smooList?.Clear();
            Close();
        }

        private void btnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            if (smoothCount++ > 100) smoothCount = 100;
            mf.gyd.SmoothAB(smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
        }

        private void btnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            smoothCount--;
            if (smoothCount < 2) smoothCount = 2;
            mf.gyd.SmoothAB(smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            mf.gyd.isSmoothWindowOpen = false;
            mf.gyd.SaveSmoothAsRefList();
            mf.gyd.smooList?.Clear();

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
                mf.gyd.moveDistance = 0;

                //Close();
            }

            //mf.FileSaveCurveLines();
            Close();
        }
    }
}