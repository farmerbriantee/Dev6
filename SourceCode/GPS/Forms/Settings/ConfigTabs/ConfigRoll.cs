using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigRoll : UserControl2
    {
        private readonly FormGPS mf;

        public ConfigRoll(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigRoll_Load(object sender, EventArgs e)
        {
            lblRollZeroOffset.Text = mf.mc.rollZero.ToString("0.00");
            hsbarRollFilter.Value = (int)(mf.mc.rollFilter * 100);
            cboxDataInvertRoll.Checked = mf.mc.isRollInvert;
        }

        public override void Close()
        {
            Properties.Settings.Default.setIMU_rollFilter = mf.mc.rollFilter = hsbarRollFilter.Value * 0.01;
            Properties.Settings.Default.setIMU_rollZero = mf.mc.rollZero;
            Properties.Settings.Default.setIMU_invertRoll = mf.mc.isRollInvert = cboxDataInvertRoll.Checked;

            Properties.Settings.Default.Save();
        }

        private void hsbarRollFilter_ValueChanged(object sender, EventArgs e)
        {
            lblRollFilterPercent.Text = hsbarRollFilter.Value.ToString();
        }

        private void btnResetIMU_Click(object sender, EventArgs e)
        {
            mf.mc.imuHeading = 99999;
            mf.mc.imuRoll = 88888;
        }

        private void btnRemoveZeroOffset_Click(object sender, EventArgs e)
        {
            mf.mc.rollZero = 0;
            lblRollZeroOffset.Text = "0.00";
        }

        private void btnZeroRoll_Click(object sender, EventArgs e)
        {
            if (mf.mc.imuRoll != 88888)
            {
                mf.mc.imuRoll += mf.mc.rollZero;
                mf.mc.rollZero = mf.mc.imuRoll;
                lblRollZeroOffset.Text = (mf.mc.rollZero).ToString("0.00");
            }
            else
            {
                lblRollZeroOffset.Text = "***";
            }
        }

        private void btnZeroRoll_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_btnZeroRoll, gStr.gsHelp).ShowDialog(this);
        }

        private void btnRemoveZeroOffset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_btnRemoveZeroOffset, gStr.gsHelp).ShowDialog(this);
        }

        private void btnResetIMU_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_btnResetIMU, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxDataInvertRoll_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxDataInvertRoll, gStr.gsHelp).ShowDialog(this);
        }
    }
}
