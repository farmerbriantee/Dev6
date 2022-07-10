using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigHitch : UserControl2
    {
        private readonly FormGPS mf;

        double trailingHitchLength, drawbarLength, tankHitch;

        public ConfigHitch(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigHitch_Load(object sender, EventArgs e)
        {
            drawbarLength = Math.Abs(mf.tool.hitchLength);
            nudDrawbarLength.Text = (drawbarLength * glm.mToUser).ToString("0");
            trailingHitchLength = Math.Abs(mf.tool.toolTrailingHitchLength);
            nudTrailingHitchLength.Text = (trailingHitchLength * glm.mToUser).ToString("0");
            tankHitch = Math.Abs(mf.tool.toolTankTrailingHitchLength);
            nudTankHitch.Text = (tankHitch * glm.mToUser).ToString("0");

            if (mf.tool.isToolFrontFixed)
            {
                nudTrailingHitchLength.Visible = false;
                nudDrawbarLength.Visible = true;
                nudTankHitch.Visible = false;

                nudTrailingHitchLength.Left = 0;
                nudDrawbarLength.Left = 342;
                nudTankHitch.Left = 0;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageFront;
            }
            else if (mf.tool.isToolTBT)
            {
                nudTrailingHitchLength.Visible = true;
                nudDrawbarLength.Visible = true;
                nudTankHitch.Visible = true;

                nudTrailingHitchLength.Left = 152;
                nudDrawbarLength.Left = 644;
                nudTankHitch.Left = 433;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageTBT;
            }
            else if (mf.tool.isToolRearFixed)
            {
                nudTrailingHitchLength.Visible = false;
                nudDrawbarLength.Visible = true;
                nudTankHitch.Visible = false;

                nudTrailingHitchLength.Left = 0;
                nudDrawbarLength.Left = 220;
                nudTankHitch.Left = 0;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageRear;
            }
            else if (mf.tool.isToolTrailing)
            {
                nudTrailingHitchLength.Visible = true;
                nudDrawbarLength.Visible = true;
                nudTankHitch.Visible = false;

                nudTrailingHitchLength.Left = 290;
                nudDrawbarLength.Left = 575;
                nudTankHitch.Left = 0;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageTrailing;
            }
        }

        public override void Close()
        {
            Properties.Vehicle.Default.Tool_TrailingHitchLength = mf.tool.toolTrailingHitchLength = -trailingHitchLength;
            Properties.Vehicle.Default.Tool_TankTrailingHitchLength = mf.tool.toolTankTrailingHitchLength = -tankHitch;

            mf.tool.hitchLength = drawbarLength;
            if (!Properties.Vehicle.Default.Tool_isToolFront)
            {
                mf.tool.hitchLength *= -1;
            }
            Properties.Vehicle.Default.setVehicle_hitchLength = mf.tool.hitchLength;


            Properties.Vehicle.Default.Save();
        }

        private void nudTrailingHitchLength_Click(object sender, EventArgs e)
        {
            nudTrailingHitchLength.KeypadToButton(ref trailingHitchLength, 0.01, 30, 0, glm.mToUser, glm.userToM);
        }

        private void nudDrawbarLength_Click(object sender, EventArgs e)
        {
            nudDrawbarLength.KeypadToButton(ref drawbarLength, 0.0, 30, 0, glm.mToUser, glm.userToM);
        }

        private void nudTankHitch_Click(object sender, EventArgs e)
        {
            nudTankHitch.KeypadToButton(ref tankHitch, 0.01, 30, 0, glm.mToUser, glm.userToM);
        }
    }
}
