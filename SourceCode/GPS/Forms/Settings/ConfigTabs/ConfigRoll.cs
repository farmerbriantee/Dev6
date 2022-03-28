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
            lblRollZeroOffset.Text = Properties.Settings.Default.setIMU_rollZero.ToString("0.00");
            hsbarRollFilter.Value = (int)(Properties.Settings.Default.setIMU_rollFilter * 100);
            cboxDataInvertRoll.Checked = Properties.Settings.Default.setIMU_invertRoll;
        }

        public override void Close()
        {
            Properties.Settings.Default.setIMU_rollFilter = mf.ahrs.rollFilter = hsbarRollFilter.Value * 0.01;
            Properties.Settings.Default.setIMU_rollZero = mf.ahrs.rollZero;
            Properties.Settings.Default.setIMU_invertRoll = mf.ahrs.isRollInvert = cboxDataInvertRoll.Checked;

            Properties.Settings.Default.Save();
        }

        private void hsbarRollFilter_ValueChanged(object sender, EventArgs e)
        {
            lblRollFilterPercent.Text = hsbarRollFilter.Value.ToString();
        }

        private void btnResetIMU_Click(object sender, EventArgs e)
        {
            mf.ahrs.imuHeading = 99999;
            mf.ahrs.imuRoll = 88888;
        }

        private void btnRemoveZeroOffset_Click(object sender, EventArgs e)
        {
            mf.ahrs.rollZero = 0;
            lblRollZeroOffset.Text = "0.00";
        }

        private void btnZeroRoll_Click(object sender, EventArgs e)
        {
            if (mf.ahrs.imuRoll != 88888)
            {
                mf.ahrs.imuRoll += mf.ahrs.rollZero;
                mf.ahrs.rollZero = mf.ahrs.imuRoll;
                lblRollZeroOffset.Text = (mf.ahrs.rollZero).ToString("0.00");
            }
            else
            {
                lblRollZeroOffset.Text = "***";
            }
        }

        private void btnZeroRoll_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_btnZeroRoll, gStr.gsHelp);
        }

        private void btnRemoveZeroOffset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_btnRemoveZeroOffset, gStr.gsHelp);
        }

        private void btnResetIMU_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_btnResetIMU, gStr.gsHelp);
        }

        private void cboxDataInvertRoll_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_cboxDataInvertRoll, gStr.gsHelp);
        }
    }
}
