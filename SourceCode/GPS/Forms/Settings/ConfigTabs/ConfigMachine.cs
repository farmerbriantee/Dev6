using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigMachine : UserControl2
    {
        private readonly FormGPS mf;

        private int user1, user2, user3, user4, raiseTime, lowerTime;
        private double lookAhead;

        public ConfigMachine(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigMachine_Load(object sender, EventArgs e)
        {
            int sett = Properties.Vehicle.Default.setArdMac_setting0;

            if ((sett & 1) == 0) cboxMachInvertRelays.Checked = false;
            else cboxMachInvertRelays.Checked = true;

            if ((sett & 2) == 0) cboxIsHydOn.Checked = false;
            else cboxIsHydOn.Checked = true;

            if (cboxIsHydOn.Checked)
            {
                cboxIsHydOn.Image = Properties.Resources.SwitchOn;
                nudHydLiftLookAhead.Enabled = true;
                nudLowerTime.Enabled = true;
                nudRaiseTime.Enabled = true;
            }
            else
            {
                cboxIsHydOn.Image = Properties.Resources.SwitchOff;
                nudHydLiftLookAhead.Enabled = false;
                nudLowerTime.Enabled = false;
                nudRaiseTime.Enabled = false;
            }

            raiseTime = mf.p_238.pgn[mf.p_238.raiseTime];
            nudRaiseTime.Text = raiseTime.ToString();
            lowerTime = mf.p_238.pgn[mf.p_238.lowerTime];
            nudLowerTime.Text = lowerTime.ToString();

            user1 = mf.p_238.pgn[mf.p_238.user1];
            nudUser1.Text = user1.ToString();
            user2 = mf.p_238.pgn[mf.p_238.user2];
            nudUser2.Text = user2.ToString();
            user3 = mf.p_238.pgn[mf.p_238.user3];
            nudUser3.Text = user3.ToString();
            user4 = mf.p_238.pgn[mf.p_238.user4];
            nudUser4.Text = user4.ToString();

            lookAhead = mf.vehicle.hydLiftLookAheadTime;
            nudHydLiftLookAhead.Text = lookAhead.ToString("0.0");
        }

        private void Enable_AlertM_Click(object sender, EventArgs e)
        {
            pboxSendMachine.Visible = true;
        }

        private void btnSendMachinePGN_Click(object sender, EventArgs e)
        {
            SaveSettingsMachine();

            Properties.Vehicle.Default.Save();

            mf.TimedMessageBox(1000, gStr.gsMachinePort, gStr.gsSentToMachineModule);

            pboxSendMachine.Visible = false;
        }

        private void SaveSettingsMachine()
        {
            int value = 0;

            if (cboxMachInvertRelays.Checked) value |= 0x01;
            if (cboxIsHydOn.Checked) value |= 0x02;

            Properties.Vehicle.Default.setArdMac_setting0 = (byte)value;
            Properties.Vehicle.Default.setArdMac_hydRaiseTime = mf.p_238.pgn[mf.p_238.raiseTime] = (byte)raiseTime;
            Properties.Vehicle.Default.setArdMac_hydLowerTime = mf.p_238.pgn[mf.p_238.lowerTime] = (byte)lowerTime;

            Properties.Vehicle.Default.setArdMac_user1 = mf.p_238.pgn[mf.p_238.user1] = (byte)user1;
            Properties.Vehicle.Default.setArdMac_user2 = mf.p_238.pgn[mf.p_238.user2] = (byte)user2;
            Properties.Vehicle.Default.setArdMac_user3 = mf.p_238.pgn[mf.p_238.user3] = (byte)user3;
            Properties.Vehicle.Default.setArdMac_user4 = mf.p_238.pgn[mf.p_238.user4] = (byte)user4;

            Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead = mf.vehicle.hydLiftLookAheadTime = lookAhead;

            mf.p_238.pgn[mf.p_238.set0] = (byte)value;

            mf.SendPgnToLoop(mf.p_238.pgn);
            pboxSendMachine.Visible = false;
        }

        private void nudUser1_Click(object sender, EventArgs e)
        {
            if (nudUser1.KeypadToButton(ref user1, 0, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudUser2_Click(object sender, EventArgs e)
        {
            if (nudUser2.KeypadToButton(ref user2, 0, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudUser3_Click(object sender, EventArgs e)
        {
            if (nudUser3.KeypadToButton(ref user3, 0, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudUser4_Click(object sender, EventArgs e)
        {
            if (nudUser4.KeypadToButton(ref user4, 0, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudRaiseTime_Click(object sender, EventArgs e)
        {
            if (nudRaiseTime.KeypadToButton(ref raiseTime, 1, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudLowerTime_Click(object sender, EventArgs e)
        {
            if (nudLowerTime.KeypadToButton(ref lowerTime, 1, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudHydLiftLookAhead_Click(object sender, EventArgs e)
        {
            if (nudHydLiftLookAhead.KeypadToButton(ref lookAhead, 1, 20, 1))
                pboxSendMachine.Visible = true;
        }

        private void cboxIsHydOn_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxIsHydOn.Checked)
            {
                cboxIsHydOn.Image = Properties.Resources.SwitchOn;
                nudHydLiftLookAhead.Enabled = true;
                nudLowerTime.Enabled = true;
                nudRaiseTime.Enabled = true;
            }
            else
            {
                cboxIsHydOn.Image = Properties.Resources.SwitchOff;
                nudHydLiftLookAhead.Enabled = false;
                nudLowerTime.Enabled = false;
                nudRaiseTime.Enabled = false;
            }
        }

        private void btnSendMachinePGN_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_btnSendSteerConfigPGN, gStr.gsHelp).ShowDialog(this);
        }

        private void nudLowerTime_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudLowerTime, gStr.gsHelp).ShowDialog(this);
        }

        private void nudRaiseTime_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudRaiseTime, gStr.gsHelp).ShowDialog(this);
        }

        private void nudHydLiftLookAhead_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudHydLiftLookAhead, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxIsHydOn_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxIsHydOn, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxMachInvertRelays_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxMachInvertRelays, gStr.gsHelp).ShowDialog(this);
        }
        private void pboxSendMachine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_pboxSendSteer, gStr.gsHelp).ShowDialog(this);
        }
    }
}
