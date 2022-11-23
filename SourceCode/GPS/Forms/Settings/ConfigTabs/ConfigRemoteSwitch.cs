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
            chkSelectWorkSwitch.Checked = mf.mc.isWorkSwitchEnabled;

            chkSetManualSections.Checked = mf.mc.isWorkSwitchManual;
            chkSetAutoSections.Checked = !chkSetManualSections.Checked;

            chkWorkSwActiveLow.Checked = mf.mc.isWorkSwitchActiveLow;
            chkWorkSwActiveLow.Image = chkWorkSwActiveLow.Checked ? Properties.Resources.SwitchActiveClosed : Properties.Resources.SwitchActiveOpen;


            chkSelectSteerSwitch.Checked = mf.mc.isSteerSwitchEnabled;

            chkSetManualSectionsSteer.Checked = mf.mc.isSteerSwitchManual;
            chkSetAutoSectionsSteer.Enabled = !chkSetManualSectionsSteer.Checked;

            cboxAutoSteerAuto.Checked = mf.mc.isAutoSteerAuto;
            if (cboxAutoSteerAuto.Checked)
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOn;
                cboxAutoSteerAuto.Text = "        Remote";
            }
            else
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOff;
                cboxAutoSteerAuto.Text = "        " + gStr.gsManual;
            }


            chkWorkSwActiveLow.Enabled = chkSetManualSections.Enabled = chkSetAutoSections.Enabled = chkSelectWorkSwitch.Checked;
            chkSetManualSectionsSteer.Enabled = chkSetAutoSectionsSteer.Enabled = chkSelectSteerSwitch.Checked;
        }

        public override void Close()
        {
            //is work switch enabled
            mf.mc.isWorkSwitchEnabled = Properties.Settings.Default.setF_IsWorkSwitchEnabled = chkSelectWorkSwitch.Checked;

            mf.mc.isWorkSwitchManual = Properties.Settings.Default.setF_IsWorkSwitchManual = chkSetManualSections.Checked;
            mf.mc.isWorkSwitchActiveLow = Properties.Settings.Default.setF_IsWorkSwitchActiveLow = chkWorkSwActiveLow.Checked;


            mf.mc.isSteerSwitchEnabled = Properties.Settings.Default.setF_IsSteerSwitchEnabled = chkSelectSteerSwitch.Checked;

            mf.mc.isSteerSwitchManual = Properties.Settings.Default.setF_steerControlsManual = chkSetManualSectionsSteer.Checked;
            mf.mc.isAutoSteerAuto = Properties.Settings.Default.setAS_isAutoSteerAutoOn = cboxAutoSteerAuto.Checked;
            mf.SetAutoSteerText();

            Properties.Settings.Default.Save();
        }

        private void chkSelectWorkSwitch_Click(object sender, EventArgs e)
        {
            chkWorkSwActiveLow.Enabled = chkSetManualSections.Enabled = chkSetAutoSections.Enabled = chkSelectWorkSwitch.Checked;
        }

        private void chkSetManualSections_Click(object sender, EventArgs e)
        {
            chkSetManualSections.Checked = true;
            chkSetAutoSections.Checked = false;
        }

        private void chkSetAutoSections_Click(object sender, EventArgs e)
        {
            chkSetManualSections.Checked = false;
            chkSetAutoSections.Checked = true;
        }

        private void chkWorkSwActiveLow_Click(object sender, EventArgs e)
        {
            chkWorkSwActiveLow.Image = chkWorkSwActiveLow.Checked ? Properties.Resources.SwitchActiveClosed : Properties.Resources.SwitchActiveOpen;
        }

        private void chkSelectSteerSwitch_Click(object sender, EventArgs e)
        {
            chkSetManualSectionsSteer.Enabled = chkSetAutoSectionsSteer.Enabled = chkSelectSteerSwitch.Checked;
        }

        private void chkSetManualSectionsSteer_Click(object sender, EventArgs e)
        {
            chkSetManualSectionsSteer.Checked = true;
            chkSetAutoSectionsSteer.Checked = false;
        }

        private void chkSetAutoSectionsSteer_Click(object sender, EventArgs e)
        {
            chkSetManualSectionsSteer.Checked = false;
            chkSetAutoSectionsSteer.Checked = true;
        }

        private void cboxAutoSteerAuto_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxAutoSteerAuto, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxAutoSteerAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxAutoSteerAuto.Checked)
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOn;
                cboxAutoSteerAuto.Text = "        Remote";
            }
            else
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOff;
                cboxAutoSteerAuto.Text = "        " + gStr.gsManual;
            }
        }
    }
}
