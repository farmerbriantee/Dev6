﻿using System;
using System.Linq;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigHeading : UserControl2
    {
        private readonly FormGPS mf;

        private int frameTime, ageAlarm;
        private double dualHeadingOffset, startSpeed, forwardComp, reverseComp, stepDistance;

        public ConfigHeading(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigHeading_Load(object sender, EventArgs e)
        {
            if ( mf.mc.fusionWeight > 0.2)
            {
                Properties.Settings.Default.setIMU_fusionWeight = 0.2;
                Properties.Settings.Default.Save();
                mf.mc.fusionWeight = 0.2;
            }

            dualHeadingOffset = mf.mc.headingTrueDualOffset;
            nudDualHeadingOffset.Text = dualHeadingOffset.ToString("0.0");

            hsbarFusion.Value = (int)(mf.mc.fusionWeight * 500);

            lblFusion.Text = hsbarFusion.Value.ToString();
            lblFusionIMU.Text = (100 - hsbarFusion.Value).ToString();
            label8.Text = "distance (" + glm.unitsInCm + " )";

            cboxIsRTK.Checked = mf.isRTK;
            cboxIsRTK_KillAutoSteer.Checked = mf.isRTK_KillAutosteer;
            cboxIsReverseOn.Checked = mf.mc.isReverseOn;
            cboxIsDualAsIMU.Checked = mf.mc.isDualAsIMU;

            stepDistance = mf.minFixStepDist;
            nudMinFixStepDistance.Text = (stepDistance * glm.mToUser).ToString("0");

            startSpeed = mf.startSpeed;
            nudStartSpeed.Text = (startSpeed * glm.KMHToUser).ToString("0.0");

            frameTime = mf.udpWatchLimit;
            nudMinimumFrameTime.Text = frameTime.ToString();

            forwardComp = mf.mc.forwardComp;
            nudForwardComp.Text = forwardComp.ToString("0.00");

            reverseComp = mf.mc.reverseComp;
            nudReverseComp.Text = reverseComp.ToString("0.00");

            ageAlarm = mf.mc.ageAlarm;
            nudAgeAlarm.Text = ageAlarm.ToString();
        }

        public override void Close()
        {
            Properties.Settings.Default.setIMU_isDualAsIMU = mf.mc.isDualAsIMU = cboxIsDualAsIMU.Checked;
            Properties.Settings.Default.setIMU_fusionWeight = mf.mc.fusionWeight = hsbarFusion.Value * 0.002;
            Properties.Settings.Default.setIMU_isReverseOn = mf.mc.isReverseOn = cboxIsReverseOn.Checked;

            Properties.Settings.Default.setGPS_isRTK = mf.isRTK = cboxIsRTK.Checked;
            Properties.Settings.Default.setGPS_isRTK_KillAutoSteer = mf.isRTK_KillAutosteer = cboxIsRTK_KillAutoSteer.Checked;
            Properties.Settings.Default.setGPS_ageAlarm = mf.mc.ageAlarm = ageAlarm;
            Properties.Settings.Default.setGPS_forwardComp = mf.mc.forwardComp = forwardComp;
            Properties.Settings.Default.setGPS_reverseComp = mf.mc.reverseComp = reverseComp;
            Properties.Settings.Default.SetGPS_udpWatchMsec = mf.udpWatchLimit = frameTime;
            Properties.Settings.Default.setGPS_dualHeadingOffset = mf.mc.headingTrueDualOffset = dualHeadingOffset;

            Properties.Settings.Default.setF_minFixStep = mf.minFixStepDist = stepDistance;
            Properties.Vehicle.Default.setVehicle_startSpeed = mf.startSpeed = startSpeed;

            Properties.Vehicle.Default.Save();
            Properties.Settings.Default.Save();
        }

        private void nudMinFixStepDistance_Click(object sender, EventArgs e)
        {
            nudMinFixStepDistance.KeypadToButton(ref stepDistance, 0.2, 10, 2, glm.mToUser, glm.userToM);
        }

        private void nudStartSpeed_Click(object sender, EventArgs e)
        {
            nudStartSpeed.KeypadToButton(ref startSpeed, 0.5, 5, 1, glm.KMHToUser, glm.userToKMH);
        }

        private void nudForwardComp_Click(object sender, EventArgs e)
        {
            nudForwardComp.KeypadToButton(ref forwardComp, 0, 5, 2);
        }

        private void nudReverseComp_Click(object sender, EventArgs e)
        {
            nudReverseComp.KeypadToButton(ref reverseComp, 0, 3, 2);
        }

        private void hsbarFusion_ValueChanged(object sender, EventArgs e)
        {
            lblFusion.Text = (hsbarFusion.Value).ToString();
            lblFusionIMU.Text = (100 - hsbarFusion.Value).ToString();
        }

        private void nudMinimumFrameTime_Click(object sender, EventArgs e)
        {
            nudMinimumFrameTime.KeypadToButton(ref frameTime, 40, 90);
        }

        private void nudAgeAlarm_Click(object sender, EventArgs e)
        {
            nudAgeAlarm.KeypadToButton(ref ageAlarm, 2, 300);
        }

        private void nudDualHeadingOffset_Click(object sender, EventArgs e)
        {
            nudDualHeadingOffset.KeypadToButton(ref dualHeadingOffset, -90, 90, 3);
        }

        private void nudReverseComp_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            if (new FormHelp(gStr.hc_nudReverseComp, gStr.gsHelp, true).ShowDialog() == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=rsJMRZrcuX4");
            }
        }

        private void nudForwardComp_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            if (new FormHelp(gStr.hc_nudForwardComp, gStr.gsHelp, true).ShowDialog() == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=rsJMRZrcuX4");
            }
        }

        private void hsbarFusion_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_hsbarFusion, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxIsDualAsIMU_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxIsDualAsIMU, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxIsReverseOn_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxIsReverseOn, gStr.gsHelp).ShowDialog(this);
        }

        private void nudMinFixStepDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudMinFixStepDistance, gStr.gsHelp).ShowDialog(this);
        }

        private void nudStartSpeed_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudStartSpeed, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxIsRTK_KillAutoSteer_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxIsRTK_KillAutoSteer, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxIsRTK_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxIsRTK, gStr.gsHelp).ShowDialog(this);
        }

        private void nudMinimumFrameTime_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudMinimumFrameTime, gStr.gsHelp).ShowDialog(this);
        }

        private void nudAgeAlarm_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudAgeAlarm, gStr.gsHelp).ShowDialog(this);
        }
    }
}
