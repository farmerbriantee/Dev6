using System;
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
            if (Properties.Settings.Default.setIMU_fusionWeight > 0.2)
            {
                Properties.Settings.Default.setIMU_fusionWeight = 0.2;
                Properties.Settings.Default.Save();
                mf.mc.fusionWeight = 0.2;
            }

            dualHeadingOffset = Properties.Settings.Default.setGPS_dualHeadingOffset;
            nudDualHeadingOffset.Text = dualHeadingOffset.ToString("0.0");

            hsbarFusion.Value = (int)(Properties.Settings.Default.setIMU_fusionWeight * 500);

            lblFusion.Text = hsbarFusion.Value.ToString();
            lblFusionIMU.Text = (100 - hsbarFusion.Value).ToString();
            label8.Text = "distance (" + mf.unitsInCm + " )";

            cboxIsRTK.Checked = Properties.Settings.Default.setGPS_isRTK;
            cboxIsRTK_KillAutoSteer.Checked = Properties.Settings.Default.setGPS_isRTK_KillAutoSteer;
            cboxIsReverseOn.Checked = Properties.Settings.Default.setIMU_isReverseOn;
            cboxIsDualAsIMU.Checked = Properties.Settings.Default.setIMU_isDualAsIMU;

            stepDistance = Properties.Settings.Default.setF_minFixStep;
            nudMinFixStepDistance.Text = (stepDistance * mf.mToUser).ToString("0");

            startSpeed = Properties.Vehicle.Default.setVehicle_startSpeed;
            nudStartSpeed.Text = (startSpeed * mf.KMHToUser).ToString("0.0");

            frameTime = Properties.Settings.Default.SetGPS_udpWatchMsec;
            nudMinimumFrameTime.Text = frameTime.ToString();

            forwardComp = Properties.Settings.Default.setGPS_forwardComp;
            nudForwardComp.Text = forwardComp.ToString("0.00");

            reverseComp = Properties.Settings.Default.setGPS_reverseComp;
            nudReverseComp.Text = reverseComp.ToString("0.00");

            ageAlarm = Properties.Settings.Default.setGPS_ageAlarm;
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
            mf.KeypadToButton(ref nudMinFixStepDistance, ref stepDistance, 0.2, 10, 2, mf.mToUser, mf.userToM);
        }

        private void nudStartSpeed_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudStartSpeed, ref startSpeed, 0.5, 5, 1, mf.KMHToUser, mf.userToKMH);
        }

        private void nudForwardComp_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudForwardComp, ref forwardComp, 0, 5, 2);
        }

        private void nudReverseComp_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudReverseComp, ref reverseComp, 0, 3, 2);
        }

        private void hsbarFusion_ValueChanged(object sender, EventArgs e)
        {
            lblFusion.Text = (hsbarFusion.Value).ToString();
            lblFusionIMU.Text = (100 - hsbarFusion.Value).ToString();
        }

        private void nudMinimumFrameTime_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudMinimumFrameTime, ref frameTime, 40, 90);
        }

        private void nudAgeAlarm_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudAgeAlarm, ref ageAlarm, 2, 300);
        }

        private void nudDualHeadingOffset_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudDualHeadingOffset, ref dualHeadingOffset, -90, 90, 3);
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
