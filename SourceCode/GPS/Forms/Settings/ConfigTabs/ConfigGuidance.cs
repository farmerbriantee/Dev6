using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigGuidance : UserControl2
    {
        private readonly FormGPS mf;

        private int lightbarCmPerPixel, lineWidth;
        private double lineLength, lookAheadTime, snapDistance;

        public ConfigGuidance(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigGuidance_Load(object sender, EventArgs e)
        {
            snapDistance = Properties.Settings.Default.setAS_snapDistance;
            nudSnapDistance.Text = (snapDistance * mf.mToUser).ToString(mf.isMetric ? "0" : "0.0");

            lineLength = Properties.Settings.Default.setAB_lineLength;
            nudABLength.Text = (lineLength * mf.mToUserBig).ToString("0");

            lookAheadTime = Properties.Settings.Default.setAS_guidanceLookAheadTime;
            nudGuidanceLookAhead.Text = lookAheadTime.ToString("0.0");

            lightbarCmPerPixel = Properties.Settings.Default.setDisplay_lightbarCmPerPixel;
            nudLightbarCmPerPixel.Text = lightbarCmPerPixel.ToString("0");

            lineWidth = Properties.Settings.Default.setDisplay_lineWidth;
            nudLineWidth.Text = lineWidth.ToString();

            cboxAutoSteerAuto.Checked = Properties.Settings.Default.setAS_isAutoSteerAutoOn;
            if (Properties.Settings.Default.setAS_isAutoSteerAutoOn)
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOn;
                cboxAutoSteerAuto.Text = "Remote";
            }
            else
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOff;
                cboxAutoSteerAuto.Text = gStr.gsManual;
            }

            label20.Text = mf.unitsInCm;
            label79.Text = mf.unitsFtM;
            label102.Text = mf.unitsInCm;
        }

        public override void Close()
        {
            Properties.Settings.Default.setAS_isAutoSteerAutoOn = mf.ahrs.isAutoSteerAuto = cboxAutoSteerAuto.Checked;
            mf.SetAutoSteerText();
            Properties.Settings.Default.setDisplay_lightbarCmPerPixel = mf.lightbarCmPerPixel = lightbarCmPerPixel;
            Properties.Settings.Default.setDisplay_lineWidth = mf.gyd.lineWidth = lineWidth;
            Properties.Settings.Default.setAB_lineLength = mf.gyd.abLength = lineLength;
            Properties.Settings.Default.setAS_snapDistance = snapDistance;
            Properties.Settings.Default.setAS_guidanceLookAheadTime = mf.guidanceLookAheadTime = lookAheadTime;

            Properties.Settings.Default.Save();
        }

        private void cboxAutoSteerAuto_Click(object sender, EventArgs e)
        {
            if (cboxAutoSteerAuto.Checked)
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOn;
                cboxAutoSteerAuto.Text = "Remote";
            }
            else
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOff;
                cboxAutoSteerAuto.Text = gStr.gsManual;
            }
        }

        private void nudLightbarCmPerPixel_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudLightbarCmPerPixel, ref lightbarCmPerPixel, 1, 100);
        }

        private void nudABLength_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudABLength, ref lineLength, 200, 5000, 0, true, mf.mToUserBig, mf.userBigToM);
        }

        private void nudSnapDistance_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudSnapDistance, ref snapDistance, 0, 10, mf.isMetric ? 0 : 1, true, mf.mToUser, mf.userToM);
        }

        private void nudGuidanceLookAhead_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudGuidanceLookAhead, ref lookAheadTime, 0, 10, 1);
        }

        private void nudLineWidth_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudLineWidth, ref lineWidth, 1, 8);
        }

        private void cboxAutoSteerAuto_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_cboxAutoSteerAuto, gStr.gsHelp);
        }

        private void nudLightbarCmPerPixel_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudLightbarCmPerPixel, gStr.gsHelp);
        }

        private void nudABLength_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudABLength, gStr.gsHelp);
        }

        private void nudLineWidth_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudLineWidth, gStr.gsHelp);
        }

        private void nudSnapDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudSnapDistance, gStr.gsHelp);
        }

        private void nudGuidanceLookAhead_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudGuidanceLookAhead, gStr.gsHelp);
        }
    }
}
