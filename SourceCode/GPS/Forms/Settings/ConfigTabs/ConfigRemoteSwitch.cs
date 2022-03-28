using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigRemoteSwitch : UserControl2
    {
        private readonly FormGPS mf;

        public ConfigRemoteSwitch(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigRemoteSwitch_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.setF_IsWorkSwitchManual)
            {
                chkSetManualSections.Checked = true;
                chkSetAutoSections.Checked = false;
            }
            else
            {
                chkSetManualSections.Checked = false;
                chkSetAutoSections.Checked = true;
            }

            if (mf.mc.isWorkSwitchEnabled)
            {
                chkRemoteSwitchEnable.Checked = true;
                chkSelectSteerSwitch.Checked = false;
                chkSelectWorkSwitch.Checked = true;
            }

            if (mf.mc.isSteerControlsManual)
            {
                chkRemoteSwitchEnable.Checked = true;
                chkSelectSteerSwitch.Checked = true;
                chkSelectWorkSwitch.Checked = false;
            }

            if (!mf.mc.isSteerControlsManual && !mf.mc.isWorkSwitchEnabled)
            {
                chkRemoteSwitchEnable.Checked = false;
                chkSelectSteerSwitch.Checked = false;
                chkSelectWorkSwitch.Checked = false;
            }

            if (!chkRemoteSwitchEnable.Checked)
            {
                chkRemoteSwitchEnable.Image = Properties.Resources.SwitchOff;
                grpControls.Enabled = false;
                grpSwitch.Enabled = false;
                chkRemoteSwitchEnable.Checked = false;
                chkSelectSteerSwitch.Checked = false;
                chkSelectWorkSwitch.Checked = false;
                grpControls.Enabled = false;
                grpSwitch.Enabled = false;
                chkSetManualSections.Checked = false;
                chkSetAutoSections.Checked = false;
            }
            else
            {
                chkRemoteSwitchEnable.Image = Properties.Resources.SwitchOn;
                grpControls.Enabled = true;
                grpSwitch.Enabled = true;
            }

            chkWorkSwActiveLow.Checked = Properties.Settings.Default.setF_IsWorkSwitchActiveLow;
            if (chkWorkSwActiveLow.Checked) chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveClosed;
            else chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveOpen;

            chkWorkSwActiveLow.Visible = !chkSelectSteerSwitch.Checked;
        }

        public override void Close()
        {
            Properties.Settings.Default.setF_IsWorkSwitchActiveLow = mf.mc.isWorkSwitchActiveLow = chkWorkSwActiveLow.Checked;

            Properties.Settings.Default.setF_IsWorkSwitchEnabled = mf.mc.isWorkSwitchEnabled = chkSelectWorkSwitch.Checked || mf.mc.isSteerControlsManual;            

            Properties.Settings.Default.setF_steerControlsManual = mf.mc.isSteerControlsManual = chkSelectSteerSwitch.Checked;

            //Are auto or manual sections controlled. Manual and Auto buttons, the old manual sets the setting, auto just visual
            Properties.Settings.Default.setF_IsWorkSwitchManual = mf.mc.isWorkSwitchManual = chkSetManualSections.Checked;

            Properties.Settings.Default.Save();
        }

        private void chkRemoteSwitchEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRemoteSwitchEnable.Checked)
            {
                //turning on
                chkSelectSteerSwitch.Checked = false;
                chkSelectWorkSwitch.Checked = true;
                grpControls.Enabled = true;
                grpSwitch.Enabled = true;
                chkSetManualSections.Checked = true;
                chkSetAutoSections.Checked = false;
                chkRemoteSwitchEnable.Image = Properties.Resources.SwitchOn;
                chkWorkSwActiveLow.Checked = true;
                chkWorkSwActiveLow.Enabled = true;
                chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveClosed;
                chkWorkSwActiveLow.Visible = true;
            }
            else
            {
                //turning off
                chkSelectSteerSwitch.Checked = false;
                chkSelectWorkSwitch.Checked = false;
                grpControls.Enabled = false;
                grpSwitch.Enabled = false;
                chkSetManualSections.Checked = false;
                chkSetAutoSections.Checked = false;
                chkRemoteSwitchEnable.Image = Properties.Resources.SwitchOff;
                chkWorkSwActiveLow.Checked = true;
                chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveClosed;
                chkWorkSwActiveLow.Visible = true;
            }
        }

        private void chkSelectWorkSwitch_Click(object sender, EventArgs e)
        {
            chkSelectSteerSwitch.Checked = false;
            chkSelectWorkSwitch.Checked = true;

            chkWorkSwActiveLow.Checked = true;
            if (chkWorkSwActiveLow.Checked) chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveClosed;
            else chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveOpen;
            chkWorkSwActiveLow.Visible = true;
        }

        private void chkSelectSteerSwitch_Click(object sender, EventArgs e)
        {
            chkSelectSteerSwitch.Checked = true;
            chkSelectWorkSwitch.Checked = false;

            chkWorkSwActiveLow.Checked = true;
            chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveClosed;
            chkWorkSwActiveLow.Visible = false;
        }

        private void chkWorkSwActiveLow_Click(object sender, EventArgs e)
        {
            if (chkWorkSwActiveLow.Checked) chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveClosed;
            else chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveOpen;
        }

        private void chkSetAutoSections_Click(object sender, EventArgs e)
        {
            chkSetManualSections.Checked = false;
            chkSetAutoSections.Checked = true;
        }

        private void chkSetManualSections_Click(object sender, EventArgs e)
        {
            chkSetAutoSections.Checked = false;
            chkSetManualSections.Checked = true;
        }
    }
}
