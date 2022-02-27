using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormTram : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;
        private bool isSaving;

        public FormTram(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.Text = gStr.gsTramLines;
            label3.Text = gStr.gsPasses;
            label2.Text = ((int)(0.1 * mf.m2InchOrCm)).ToString() + mf.unitsInCm;
            lblTramWidth.Text = (mf.tram.tramWidth * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;

            nudPasses.Controls[0].Enabled = false;
        }

        private void FormTram_Load(object sender, EventArgs e)
        {
            nudPasses.Value = Properties.Settings.Default.setTram_passes;
            nudPasses.ValueChanged += nudPasses_ValueChanged;

            lblTrack.Text = (mf.vehicle.trackWidth * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;

            mf.tool.halfToolWidth = (mf.tool.toolWidth - mf.tool.toolOverlap) / 2.0;
            lblToolWidthHalf.Text = (mf.tool.halfToolWidth * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;

            mf.panelRight.Enabled = false;

            //if off, turn it on because they obviously want a tram.
            if (mf.tram.displayMode == 0) mf.tram.displayMode = 1;

            switch (mf.tram.displayMode)
            {
                case 0:
                    btnMode.Image = Properties.Resources.TramOff;
                    break;
                case 1:
                    btnMode.Image = Properties.Resources.TramAll;
                    break;
                case 2:
                    btnMode.Image = Properties.Resources.TramLines;
                    break;
                case 3:
                    btnMode.Image = Properties.Resources.TramOuter;
                    break;

                default:
                    break;
            }
            mf.CloseTopMosts();
        }

        private void MoveBuildTramLine(double Dist)
        {
            if (Dist != 0)
                mf.gyd.MoveGuidanceLine(mf.gyd.currentGuidanceLine, Dist);
            mf.tram.BuildTram(mf.gyd.currentCurveLine);
        }

        private void FormTram_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaving)
            {
                if (mf.gyd.currentGuidanceLine != null)
                {
                    int idx = mf.gyd.curveArr.FindIndex(x => x.Name == mf.gyd.currentGuidanceLine?.Name);
                    if (idx > -1)
                        mf.gyd.curveArr[idx] = new CGuidanceLine(mf.gyd.currentGuidanceLine);

                    //save entire list
                    if (mf.gyd.currentGuidanceLine.mode.HasFlag(Mode.AB))
                        mf.FileSaveABLines();
                    else
                        mf.FileSaveCurveLines();

                    mf.gyd.moveDistance = 0;
                }
            }
            else
            {
                mf.tram.tramArr.Clear();
                mf.tram.tramList.Clear();
                mf.tram.tramBndOuterArr.Clear();
                mf.tram.tramBndInnerArr.Clear();

                mf.tram.displayMode = 0;
            }

            mf.panelRight.Enabled = true;

            mf.FileSaveTram();
            mf.FixTramModeButton();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            isSaving = true;
            Close();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            MoveBuildTramLine(-0.1);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            MoveBuildTramLine(0.1);
        }

        private void btnAdjLeft_Click(object sender, EventArgs e)
        {
            MoveBuildTramLine(-mf.tool.halfToolWidth);
        }

        private void btnAdjRight_Click(object sender, EventArgs e)
        {
            MoveBuildTramLine(mf.tool.halfToolWidth);
        }

        private void nudPasses_ValueChanged(object sender, EventArgs e)
        {
            mf.tram.passes = (int)nudPasses.Value;
            Properties.Settings.Default.setTram_passes = mf.tram.passes;
            Properties.Settings.Default.Save();
            MoveBuildTramLine(0);
        }

        private void nudPasses_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (mf.gyd.currentGuidanceLine != null)
            {
                mf.gyd.isValid = false;
                mf.gyd.ReverseGuidanceLine(mf.gyd.currentGuidanceLine);
            }
            MoveBuildTramLine(0);
        }

        private void btnTriggerDistanceUp_MouseDown(object sender, MouseEventArgs e)
        {
            nudPasses.UpButton();
        }

        private void btnTriggerDistanceDn_MouseDown(object sender, MouseEventArgs e)
        {
            nudPasses.DownButton();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            mf.tram.displayMode++;
            if (mf.tram.displayMode > 3) mf.tram.displayMode = 0;

            switch (mf.tram.displayMode)
            {
                case 0:
                    btnMode.Image = Properties.Resources.TramOff;
                    break;
                case 1:
                    btnMode.Image = Properties.Resources.TramAll;
                    break;
                case 2:
                    btnMode.Image = Properties.Resources.TramLines;
                    break;
                case 3:
                    btnMode.Image = Properties.Resources.TramOuter;
                    break;

                default:
                    break;
            }
        }

        #region Help
        private void btnAdjLeft_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ht_btnAdjHalfToolWidth, gStr.gsHelp);
        }

        private void btnAdjRight_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ht_btnAdjHalfToolWidth, gStr.gsHelp);
        }

        private void btnLeft_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ht_btnLeftRightNudge, gStr.gsHelp);
        }

        private void btnRight_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ht_btnLeftRightNudge, gStr.gsHelp);
        }

        private void btnSwapAB_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ht_btnSwapAB, gStr.gsHelp);
        }

        private void btnMode_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.h_btnTramDisplayMode, gStr.gsHelp);
        }

        private void nudPasses_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ht_nudPasses, gStr.gsHelp);
        }

        private void btnCancel_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ht_btnCancel, gStr.gsHelp);
        }

        private void btnExit_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ht_btnSave, gStr.gsHelp);
        }

        #endregion
    }
}


/*
            
            MessageBox.Show(gStr, gStr.gsHelp);

            DialogResult result2 = MessageBox.Show(gStr, gStr.gsHelp,
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result2 == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=rsJMRZrcuX4");
            }

*/
