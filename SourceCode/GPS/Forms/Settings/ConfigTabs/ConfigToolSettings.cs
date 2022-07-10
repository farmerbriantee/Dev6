using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigToolSettings : UserControl2
    {
        private readonly FormGPS mf;

        private double lookAhead, lookAheadOff, turnOffDelay, offset, overlap;

        public ConfigToolSettings(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigToolSettings_Load(object sender, EventArgs e)
        {
            lookAhead = mf.tool.lookAheadOnSetting;
            nudLookAhead.Text = lookAhead.ToString("0.0");
            lookAheadOff = mf.tool.lookAheadOffSetting;
            nudLookAheadOff.Text = lookAheadOff.ToString("0.0");
            turnOffDelay = mf.tool.turnOffDelay;
            nudTurnOffDelay.Text = turnOffDelay.ToString("0.0");
            offset = mf.tool.toolOffset;
            nudOffset.Text = (offset * glm.mToUser).ToString("0");
            overlap = mf.tool.toolOverlap;
            nudOverlap.Text = (overlap * glm.mToUser).ToString("0");
        }

        public override void Close()
        {
            Properties.Vehicle.Default.Tool_LookAheadOn = mf.tool.lookAheadOnSetting = lookAhead;
            Properties.Vehicle.Default.Tool_LookAheadOff = mf.tool.lookAheadOffSetting = lookAheadOff;
            Properties.Vehicle.Default.Tool_OffDelay = mf.tool.turnOffDelay = turnOffDelay;
            Properties.Vehicle.Default.Tool_Overlap = mf.tool.toolOverlap = overlap;
            Properties.Vehicle.Default.Tool_Offset = mf.tool.toolOffset = offset;

            Properties.Vehicle.Default.Save();
        }

        private void nudLookAhead_Click(object sender, EventArgs e)
        {
            nudLookAhead.KeypadToButton(ref lookAhead, 0.0, 10, 1);
        }

        private void nudLookAheadOff_Click(object sender, EventArgs e)
        {
            if (nudLookAheadOff.KeypadToButton(ref lookAheadOff, 0, 10, 1))
            {
                if (lookAheadOff > 0)
                {
                    turnOffDelay = 0;
                    nudTurnOffDelay.Text = "0.0";
                }
            }
        }

        private void nudTurnOffDelay_Click(object sender, EventArgs e)
        {
            if (nudTurnOffDelay.KeypadToButton(ref turnOffDelay, 0, 10, 1))
            {
                if (turnOffDelay > 0)
                {
                    lookAheadOff = 0;
                    nudLookAheadOff.Text = "0.0";
                }
            }
        }

        private void nudOffset_Click(object sender, EventArgs e)
        {
            nudOffset.KeypadToButton(ref offset, -25, 25, 0, glm.mToUser, glm.userToM);
        }

        private void nudOverlap_Click(object sender, EventArgs e)
        {
            nudOverlap.KeypadToButton(ref overlap, -30, 30, 0, glm.mToUser, glm.userToM);
        }

        private void nudLookAhead_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudLookAheadOn, gStr.gsHelp).ShowDialog(this);
        }

        private void nudLookAheadOff_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudLookAheadOff, gStr.gsHelp).ShowDialog(this);
        }

        private void nudTurnOffDelay_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudTurnOffDelay, gStr.gsHelp).ShowDialog(this);
        }

        private void nudOffset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudOffset, gStr.gsHelp).ShowDialog(this);
        }

        private void nudOverlap_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudOverlap, gStr.gsHelp).ShowDialog(this);
        }
    }
}
