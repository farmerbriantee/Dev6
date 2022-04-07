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

            raiseTime = Properties.Vehicle.Default.setArdMac_hydRaiseTime;
            nudRaiseTime.Text = raiseTime.ToString();
            lowerTime = Properties.Vehicle.Default.setArdMac_hydLowerTime;
            nudLowerTime.Text = lowerTime.ToString();

            user1 = Properties.Vehicle.Default.setArdMac_user1;
            nudUser1.Text = user1.ToString();
            user2 = Properties.Vehicle.Default.setArdMac_user2;
            nudUser2.Text = user2.ToString();
            user3 = Properties.Vehicle.Default.setArdMac_user3;
            nudUser3.Text = user3.ToString();
            user4 = Properties.Vehicle.Default.setArdMac_user4;
            nudUser4.Text = user4.ToString();

            lookAhead = Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead;
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
            if (mf.KeypadToButton(ref nudUser1, ref user1, 0, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudUser2_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudUser2, ref user2, 0, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudUser3_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudUser3, ref user3, 0, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudUser4_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudUser4, ref user4, 0, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudRaiseTime_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudRaiseTime, ref raiseTime, 1, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudLowerTime_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudLowerTime, ref lowerTime, 1, 255))
                pboxSendMachine.Visible = true;
        }

        private void nudHydLiftLookAhead_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudHydLiftLookAhead, ref lookAhead, 1, 20, 1))
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
            MessageBox.Show(gStr.hc_btnSendSteerConfigPGN, gStr.gsHelp);
        }

        private void nudLowerTime_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudLowerTime, gStr.gsHelp);
        }

        private void nudRaiseTime_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudRaiseTime, gStr.gsHelp);
        }

        private void nudHydLiftLookAhead_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudHydLiftLookAhead, gStr.gsHelp);
        }

        private void cboxIsHydOn_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_cboxIsHydOn, gStr.gsHelp);
        }

        private void cboxMachInvertRelays_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_cboxMachInvertRelays, gStr.gsHelp);
        }
        private void pboxSendMachine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_pboxSendSteer, gStr.gsHelp);
        }
    }
}
