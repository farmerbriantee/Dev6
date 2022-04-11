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
            drawbarLength = Math.Abs(Properties.Vehicle.Default.setVehicle_hitchLength);
            nudDrawbarLength.Text = (drawbarLength * mf.mToUser).ToString("0");
            trailingHitchLength = Math.Abs(Properties.Vehicle.Default.setTool_toolTrailingHitchLength);
            nudTrailingHitchLength.Text = (trailingHitchLength * mf.mToUser).ToString("0");
            tankHitch = Math.Abs(Properties.Vehicle.Default.setVehicle_tankTrailingHitchLength);
            nudTankHitch.Text = (tankHitch * mf.mToUser).ToString("0");

            if (Properties.Vehicle.Default.setTool_isToolFront)
            {
                nudTrailingHitchLength.Visible = false;
                nudDrawbarLength.Visible = true;
                nudTankHitch.Visible = false;

                nudTrailingHitchLength.Left = 0;
                nudDrawbarLength.Left = 342;
                nudTankHitch.Left = 0;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageFront;
            }
            else if (Properties.Vehicle.Default.setTool_isToolTBT)
            {
                nudTrailingHitchLength.Visible = true;
                nudDrawbarLength.Visible = true;
                nudTankHitch.Visible = true;

                nudTrailingHitchLength.Left = 152;
                nudDrawbarLength.Left = 644;
                nudTankHitch.Left = 433;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageTBT;
            }
            else if (Properties.Vehicle.Default.setTool_isToolRearFixed)
            {
                nudTrailingHitchLength.Visible = false;
                nudDrawbarLength.Visible = true;
                nudTankHitch.Visible = false;

                nudTrailingHitchLength.Left = 0;
                nudDrawbarLength.Left = 220;
                nudTankHitch.Left = 0;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageRear;
            }
            else if (Properties.Vehicle.Default.setTool_isToolTrailing)
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
            Properties.Vehicle.Default.setTool_toolTrailingHitchLength = mf.tool.toolTrailingHitchLength = -trailingHitchLength;
            Properties.Vehicle.Default.setVehicle_tankTrailingHitchLength = mf.tool.toolTankTrailingHitchLength = -tankHitch;

            mf.tool.hitchLength = drawbarLength;
            if (!Properties.Vehicle.Default.setTool_isToolFront)
            {
                mf.tool.hitchLength *= -1;
            }
            Properties.Vehicle.Default.setVehicle_hitchLength = mf.tool.hitchLength;


            Properties.Vehicle.Default.Save();
        }

        private void nudTrailingHitchLength_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudTrailingHitchLength, ref trailingHitchLength, 0.01, 30, 0, mf.mToUser, mf.userToM);
        }

        private void nudDrawbarLength_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudDrawbarLength, ref drawbarLength, 0.0, 30, 0, mf.mToUser, mf.userToM);
        }

        private void nudTankHitch_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudTankHitch, ref tankHitch, 0.01, 30, 0, mf.mToUser, mf.userToM);
        }
    }
}
