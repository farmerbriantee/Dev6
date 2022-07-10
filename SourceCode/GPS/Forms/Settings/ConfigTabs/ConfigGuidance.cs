using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigGuidance : UserControl2
    {
        private readonly FormGPS mf;

        private int lightbarCmPerPixel, lineWidth;
        private double lineLength, lookAheadTime, snapDistance, PanicStopSpeed;

        public ConfigGuidance(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigGuidance_Load(object sender, EventArgs e)
        {
            snapDistance = Properties.Settings.Default.setAS_snapDistance;
            nudSnapDistance.Text = (snapDistance * glm.mToUser).ToString(glm.isMetric ? "0" : "0.0");

            lineLength = Properties.Settings.Default.setAB_lineLength;
            nudABLength.Text = (lineLength * glm.mToUserBig).ToString("0");

            lookAheadTime = Properties.Settings.Default.setAS_guidanceLookAheadTime;
            nudGuidanceLookAhead.Text = lookAheadTime.ToString("0.0");

            lightbarCmPerPixel = Properties.Settings.Default.setDisplay_lightbarCmPerPixel;
            nudLightbarCmPerPixel.Text = lightbarCmPerPixel.ToString("0");

            lineWidth = Properties.Settings.Default.setDisplay_lineWidth;
            nudLineWidth.Text = lineWidth.ToString();

            PanicStopSpeed = Properties.Settings.Default.setVehicle_panicStopSpeed;
            nudPanicStopSpeed.Text = (PanicStopSpeed * glm.KMHToUser).ToString("0.0");

            label20.Text = glm.unitsInCm;
            label79.Text = glm.unitsFtM;;
            label102.Text = glm.unitsInCm;
        }

        public override void Close()
        {
            Properties.Settings.Default.setDisplay_lightbarCmPerPixel = mf.lightbarCmPerPixel = lightbarCmPerPixel;
            Properties.Settings.Default.setDisplay_lineWidth = mf.gyd.lineWidth = lineWidth;
            Properties.Settings.Default.setAB_lineLength = mf.gyd.abLength = lineLength;
            Properties.Settings.Default.setAS_snapDistance = snapDistance;
            Properties.Settings.Default.setAS_guidanceLookAheadTime = mf.guidanceLookAheadTime = lookAheadTime;
            Properties.Settings.Default.setVehicle_panicStopSpeed = mf.mc.panicStopSpeed = PanicStopSpeed;

            Properties.Settings.Default.Save();
        }

        private void nudLightbarCmPerPixel_Click(object sender, EventArgs e)
        {
            nudLightbarCmPerPixel.KeypadToButton(ref lightbarCmPerPixel, 1, 100);
        }

        private void nudABLength_Click(object sender, EventArgs e)
        {
            nudABLength.KeypadToButton(ref lineLength, 200, 5000, 0, glm.mToUserBig, glm.userBigToM);
        }

        private void nudSnapDistance_Click(object sender, EventArgs e)
        {
            nudSnapDistance.KeypadToButton(ref snapDistance, 0, 10, glm.isMetric ? 0 : 1, glm.mToUser, glm.userToM);
        }

        private void nudGuidanceLookAhead_Click(object sender, EventArgs e)
        {
            nudGuidanceLookAhead.KeypadToButton(ref lookAheadTime, 0, 10, 1);
        }

        private void nudLineWidth_Click(object sender, EventArgs e)
        {
            nudLineWidth.KeypadToButton(ref lineWidth, 1, 8);
        }

        private void nudPanicStopSpeed_Click(object sender, EventArgs e)
        {
            nudPanicStopSpeed.KeypadToButton(ref PanicStopSpeed, 0.0, 100.0, 1, glm.KMHToUser, glm.userToKMH);
        }

        private void nudLightbarCmPerPixel_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudLightbarCmPerPixel, gStr.gsHelp).ShowDialog(this);
        }

        private void nudABLength_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudABLength, gStr.gsHelp).ShowDialog(this);
        }

        private void nudLineWidth_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudLineWidth, gStr.gsHelp).ShowDialog(this);
        }

        private void nudSnapDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudSnapDistance, gStr.gsHelp).ShowDialog(this);
        }

        private void nudGuidanceLookAhead_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudGuidanceLookAhead, gStr.gsHelp).ShowDialog(this);
        }
    }
}
