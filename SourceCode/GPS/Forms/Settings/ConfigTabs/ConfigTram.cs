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
            lblTramWidthUnits.Text = glm.unitsFtM;
            tramWidth = mf.tram.tramWidth;

            nudTramWidth.Text = (tramWidth * glm.mToUserBig).ToString("0.00");

            cboxTramOnBackBuffer.Checked = mf.tram.isTramOnBackBuffer;
        }

        public override void Close()
        {
            Properties.Settings.Default.setTram_isTramOnBackBuffer = mf.tram.isTramOnBackBuffer = cboxTramOnBackBuffer.Checked;

            Properties.Settings.Default.setTram_tramWidth = mf.tram.tramWidth = tramWidth;
            mf.tram.isOuter = ((int)(tramWidth / mf.tool.toolWidth + 0.5)) % 2 == 0;
            mf.tool.updateVBO = true;

            Properties.Settings.Default.Save();
        }

        private void nudTramWidth_Click(object sender, EventArgs e)
        {
            nudTramWidth.KeypadToButton(ref tramWidth, 0.5, 50, 2, glm.mToUserBig, glm.userBigToM);
        }

        private void cboxTramOnBackBuffer_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxTramOnBackBuffer, gStr.gsHelp).ShowDialog(this);
        }

        private void nudTramWidth_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudTramWidth, gStr.gsHelp).ShowDialog(this);
        }
    }
}
