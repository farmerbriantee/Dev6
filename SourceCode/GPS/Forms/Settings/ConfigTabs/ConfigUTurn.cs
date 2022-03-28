using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigUTurn : UserControl2
    {
        private readonly FormGPS mf;

        private int uTurnSmoothing, youTurnStartOffset;
        private double DistanceFromBoundary;

        public ConfigUTurn(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigUTurn_Load(object sender, EventArgs e)
        {
            youTurnStartOffset = mf.gyd.youTurnStartOffset;
            lblDistance.Text = Math.Abs(youTurnStartOffset * mf.mToUserBig).ToString("0") + mf.unitsFtM;

            uTurnSmoothing = mf.gyd.uTurnSmoothing;
            lblSmoothing.Text = uTurnSmoothing.ToString();

            DistanceFromBoundary = Properties.Vehicle.Default.set_youTurnDistanceFromBoundary;
            nudTurnDistanceFromBoundary.Text = (DistanceFromBoundary * mf.mToUserBig).ToString("0.00");

            lblFtMUTurn.Text = mf.unitsFtM;
        }

        public override void Close()
        {
            if (mf.gyd.uTurnSmoothing != uTurnSmoothing || mf.gyd.youTurnStartOffset != youTurnStartOffset)
            {
                Properties.Settings.Default.setAS_uTurnSmoothing = mf.gyd.uTurnSmoothing = uTurnSmoothing;
                Properties.Vehicle.Default.set_youTurnExtensionLength = mf.gyd.youTurnStartOffset = youTurnStartOffset;
                mf.gyd.ResetCreatedYouTurn();
            }

            if (mf.gyd.uturnDistanceFromBoundary != DistanceFromBoundary)
            {
                Properties.Vehicle.Default.set_youTurnDistanceFromBoundary = mf.gyd.uturnDistanceFromBoundary = DistanceFromBoundary;
                mf.bnd.BuildTurnLines();
            }

            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();
        }

        private void btnTurnSmoothingUp_Click(object sender, EventArgs e)
        {
            uTurnSmoothing += 2;
            if (uTurnSmoothing > 18) uTurnSmoothing = 18;
            lblSmoothing.Text = uTurnSmoothing.ToString();
        }

        private void btnTurnSmoothingDown_Click(object sender, EventArgs e)
        {
            uTurnSmoothing -= 2;
            if (uTurnSmoothing < 4) uTurnSmoothing = 4;
            lblSmoothing.Text = uTurnSmoothing.ToString();
        }

        private void btnDistanceUp_Click(object sender, EventArgs e)
        {
            if (youTurnStartOffset < 50)
                youTurnStartOffset++;
            else
                youTurnStartOffset = 50;

            lblDistance.Text = (youTurnStartOffset * mf.mToUserBig).ToString("0") + mf.unitsFtM;
        }

        private void btnDistanceDn_Click(object sender, EventArgs e)
        {
            if (youTurnStartOffset > 3)
                youTurnStartOffset--;
            else
                youTurnStartOffset = 3;

            lblDistance.Text = (youTurnStartOffset * mf.mToUserBig).ToString("0") + mf.unitsFtM;
        }

        private void nudTurnDistanceFromBoundary_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudTurnDistanceFromBoundary, ref DistanceFromBoundary, 0, 100, 2, true, mf.mToUserBig, mf.userBigToM);
        }

        private void nudTurnDistanceFromBoundary_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudTurnDistanceFromBoundary, gStr.gsHelp);
        }

        private void lblDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_lblUTurnLegDistance, gStr.gsHelp);
        }

        private void lblSmoothing_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_lblUTurnSmoothing, gStr.gsHelp);
        }
    }
}
