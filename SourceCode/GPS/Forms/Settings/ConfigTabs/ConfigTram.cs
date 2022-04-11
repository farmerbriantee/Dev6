using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigTram : UserControl2
    {
        private readonly FormGPS mf;

        double tramWidth;

        public ConfigTram(Form callingForm)
        {
            mf = callingForm as FormGPS;
            
            InitializeComponent();
        }

        private void ConfigTram_Load(object sender, EventArgs e)
        {
            lblTramWidthUnits.Text = mf.unitsFtM;
            tramWidth = Properties.Settings.Default.setTram_tramWidth;

            nudTramWidth.Text = (tramWidth * mf.mToUserBig).ToString("0.00");

            cboxTramOnBackBuffer.Checked = Properties.Settings.Default.setTram_isTramOnBackBuffer;
        }

        public override void Close()
        {
            Properties.Settings.Default.setTram_isTramOnBackBuffer = mf.isTramOnBackBuffer = cboxTramOnBackBuffer.Checked;

            Properties.Settings.Default.setTram_tramWidth = mf.tram.tramWidth = tramWidth;
            mf.tram.isOuter = ((int)(tramWidth / mf.tool.toolWidth + 0.5)) % 2 == 0;

            Properties.Settings.Default.Save();
        }

        private void nudTramWidth_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudTramWidth, ref tramWidth, 0.5, 50, 2, mf.mToUserBig, mf.userBigToM);
        }

        private void cboxTramOnBackBuffer_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_cboxTramOnBackBuffer, gStr.gsHelp);
        }

        private void nudTramWidth_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudTramWidth, gStr.gsHelp);
        }
    }
}
