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
            lookAhead = Properties.Vehicle.Default.setVehicle_toolLookAheadOn;
            nudLookAhead.Text = lookAhead.ToString("0.0");
            lookAheadOff = Properties.Vehicle.Default.setVehicle_toolLookAheadOff;
            nudLookAheadOff.Text = lookAheadOff.ToString("0.0");
            turnOffDelay = Properties.Vehicle.Default.setVehicle_toolOffDelay;
            nudTurnOffDelay.Text = turnOffDelay.ToString("0.0");
            offset = Properties.Vehicle.Default.setVehicle_toolOffset;
            nudOffset.Text = (offset * mf.mToUser).ToString("0");
            overlap = Properties.Vehicle.Default.setVehicle_toolOverlap;
            nudOverlap.Text = (overlap * mf.mToUser).ToString("0");
        }

        public override void Close()
        {
            Properties.Vehicle.Default.setVehicle_toolLookAheadOn = mf.tool.lookAheadOnSetting = lookAhead;
            Properties.Vehicle.Default.setVehicle_toolLookAheadOff = mf.tool.lookAheadOffSetting = lookAheadOff;
            Properties.Vehicle.Default.setVehicle_toolOffDelay = mf.tool.turnOffDelay = turnOffDelay;
            Properties.Vehicle.Default.setVehicle_toolOverlap = mf.tool.toolOverlap = overlap;

            if (mf.tool.toolOffset != offset)
            {
                Properties.Vehicle.Default.setVehicle_toolOffset = mf.tool.toolOffset = offset;

                //update the sections to newly configured widths and positions in main
                mf.SectionSetPosition();
            }

            Properties.Vehicle.Default.Save();
        }

        private void nudLookAhead_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudLookAhead, ref lookAhead, 0.2, 22, 1))
            {
                if (lookAheadOff > (lookAhead * 0.8))
                {
                    lookAheadOff = lookAhead * 0.8;
                    nudLookAheadOff.Text = lookAheadOff.ToString("0.0");
                }
            }
        }

        private void nudLookAheadOff_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudLookAheadOff, ref lookAheadOff, 0, 20, 1))
            {
                if (lookAheadOff > (lookAhead * 0.8))
                {
                    lookAheadOff = lookAhead * 0.8;
                    nudLookAheadOff.Text = lookAheadOff.ToString("0.0");
                }

                if (lookAheadOff > 0)
                {
                    turnOffDelay = 0;
                    nudTurnOffDelay.Text = "0.0";
                }
            }
        }

        private void nudTurnOffDelay_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudTurnOffDelay, ref turnOffDelay, 0, 10, 1))
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
            mf.KeypadToButton(ref nudOffset, ref offset, -25, 25, 0, true, mf.mToUser, mf.userToM);
        }

        private void nudOverlap_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudOverlap, ref overlap, -30, 30, 0, true, mf.mToUser, mf.userToM);
        }

        private void nudLookAhead_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudLookAheadOn, gStr.gsHelp);
        }

        private void nudLookAheadOff_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudLookAheadOff, gStr.gsHelp);
        }

        private void nudTurnOffDelay_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudTurnOffDelay, gStr.gsHelp);
        }

        private void nudOffset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudOffset, gStr.gsHelp);
        }

        private void nudOverlap_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudOverlap, gStr.gsHelp);
        }
    }
}
