using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormTram : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;
        private bool isSaving;
        private double halfToolWidth;

        public FormTram(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.Text = gStr.gsTramLines;
            label3.Text = gStr.gsPasses;
            label2.Text = ((int)(0.1 * glm.mToUser)).ToString() + glm.unitsInCm;
            lblTramWidth.Text = (mf.tram.tramWidth * glm.mToUserBig).ToString("0.00") + glm.unitsFtM;

            nudPasses.Controls[0].Enabled = false;
        }

        private void FormTram_Load(object sender, EventArgs e)
        {
            nudPasses.Value = Properties.Settings.Default.setTram_passes;
            nudPasses.ValueChanged += nudPasses_ValueChanged;

            halfToolWidth = (mf.tool.toolWidth - mf.tool.toolOverlap) / 2.0;


            lblTrack.Text = (mf.vehicle.trackWidth * glm.mToUserBig).ToString("0.00") + glm.unitsFtM;;

            lblToolWidthHalf.Text = (halfToolWidth * glm.mToUserBig).ToString("0.00") + glm.unitsFtM;;

            mf.PanelRightEnabled(false);

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
            mf.tram.BuildTram();
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
                for (int j = 0; j < mf.tram.tramList.Count; j++)
                    mf.tram.tramList[j].RemoveHandle();
                mf.tram.tramList.Clear();

                for (int i = 0; i < mf.tram.tramBoundary.Count; i++)
                {
                    for (int j = 0; j < mf.tram.tramBoundary[i].Count; j++)
                        mf.tram.tramBoundary[i][j].RemoveHandle();
                }
                mf.tram.tramBoundary.Clear();

                mf.tram.displayMode = 0;
            }

            mf.PanelRightEnabled(true);

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
            MoveBuildTramLine(-halfToolWidth);
        }

        private void btnAdjRight_Click(object sender, EventArgs e)
        {
            MoveBuildTramLine(halfToolWidth);
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
            nudPasses.KeypadToNUD();
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
            new FormHelp(gStr.ht_btnAdjHalfToolWidth, gStr.gsHelp).ShowDialog(this);
        }

        private void btnAdjRight_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ht_btnAdjHalfToolWidth, gStr.gsHelp).ShowDialog(this);
        }

        private void btnLeft_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ht_btnLeftRightNudge, gStr.gsHelp).ShowDialog(this);
        }

        private void btnRight_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ht_btnLeftRightNudge, gStr.gsHelp).ShowDialog(this);
        }

        private void btnSwapAB_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ht_btnSwapAB, gStr.gsHelp).ShowDialog(this);
        }

        private void btnMode_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_btnTramDisplayMode, gStr.gsHelp).ShowDialog(this);
        }

        private void nudPasses_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ht_nudPasses, gStr.gsHelp).ShowDialog(this);
        }

        private void btnCancel_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ht_btnCancel, gStr.gsHelp).ShowDialog(this);
        }

        private void btnExit_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.ht_btnSave, gStr.gsHelp).ShowDialog(this);
        }

        #endregion
    }
}


/*
            

            DialogResult result2 = new FormAccept(gStr, gStr.gsHelp).ShowDialog(this);

            if (result2 == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=rsJMRZrcuX4");
            }

*/
