using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigHitch : UserControl2
    {
        private readonly FormGPS mf;

        double tankHitchLength, trailingAxleLength, trailingHitchLength, tankAxleLength, hitchLength;

        public ConfigHitch(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigHitch_Load(object sender, EventArgs e)
        {
            hitchLength = Math.Abs(mf.tool.hitchLength);
            nudHitchLength.Text = (hitchLength * glm.mToUser).ToString("0");

            tankAxleLength = Math.Abs(mf.tool.TankAxleLength);
            nudTankAxleLength.Text = (tankAxleLength * glm.mToUser).ToString("0");

            tankHitchLength = Math.Abs(mf.tool.TankHitchLength);
            nudTankHitchLength.Text = (tankHitchLength * glm.mToUser).ToString("0");

            trailingAxleLength = Math.Abs(mf.tool.TrailingAxleLength);
            nudTrailingAxleLength.Text = (trailingAxleLength * glm.mToUser).ToString("0");

            trailingHitchLength = Math.Abs(mf.tool.TrailingHitchLength);
            nudTrailingHitchLength.Text = (trailingHitchLength * glm.mToUser).ToString("0");

            if (mf.tool.isToolFrontFixed)
            {
                nudTrailingHitchLength.Visible = false;
                nudTrailingAxleLength.Visible = false;
                nudTankHitchLength.Visible = false;
                nudTankAxleLength.Visible = false;
                nudHitchLength.Visible = true;

                nudHitchLength.Left = 350;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageFront;
            }
            else if (mf.tool.isToolTBT)
            {
                nudTrailingHitchLength.Visible = true;
                nudTrailingAxleLength.Visible = true;
                nudTankHitchLength.Visible = true;
                nudTankAxleLength.Visible = true;
                nudHitchLength.Visible = true;

                nudTrailingHitchLength.Left = 50;
                nudTrailingAxleLength.Left = 200;
                nudTankHitchLength.Left = 350;
                nudTankAxleLength.Left = 500;
                nudHitchLength.Left = 650;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageTBT;
            }
            else if (mf.tool.isToolRearFixed)
            {
                nudTrailingHitchLength.Visible = false;
                nudTrailingAxleLength.Visible = false;
                nudTankHitchLength.Visible = false;
                nudTankAxleLength.Visible = false;
                nudHitchLength.Visible = true;

                nudHitchLength.Left = 250;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageRear;
            }
            else if (mf.tool.isToolTrailing)
            {
                nudTrailingHitchLength.Visible = true;
                nudTrailingAxleLength.Visible = true;
                nudTankHitchLength.Visible = false;
                nudTankAxleLength.Visible = false;
                nudHitchLength.Visible = true;

                nudTrailingHitchLength.Left = 70;
                nudTrailingAxleLength.Left = 320;
                nudHitchLength.Left = 580;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageTrailing;
            }
        }

        public override void Close()
        {
            mf.tool.hitchLength = hitchLength;
            if (!Properties.Vehicle.Default.Tool_isToolFront)
            {
                mf.tool.hitchLength *= -1;
            }
            Properties.Vehicle.Default.setVehicle_hitchLength = mf.tool.hitchLength;

            Properties.Vehicle.Default.Tool_TankTrailingAxleLength = mf.tool.TankAxleLength = -tankAxleLength;
            Properties.Vehicle.Default.Tool_TankTrailingHitchLength = mf.tool.TankHitchLength = -tankHitchLength;

            Properties.Vehicle.Default.Tool_TrailingAxleLength = mf.tool.TrailingAxleLength = -trailingAxleLength;
            Properties.Vehicle.Default.Tool_TrailingHitchLength = mf.tool.TrailingHitchLength = -trailingHitchLength;
            mf.vehicle.updateVBO = true;
            mf.tool.updateVBO = true;
            Properties.Vehicle.Default.Save();
        }

        private void nudHitchLength_Click(object sender, EventArgs e)
        {
            nudHitchLength.KeypadToButton(ref hitchLength, 0, 30, 0, glm.mToUser, glm.userToM);
        }

        private void nudTankAxleLength_Click(object sender, EventArgs e)
        {
            nudTankAxleLength.KeypadToButton(ref tankAxleLength, 0, 30, 0, glm.mToUser, glm.userToM);
        }

        private void nudTankHitch_Click(object sender, EventArgs e)
        {
            nudTankHitchLength.KeypadToButton(ref tankHitchLength, -tankAxleLength, 30, 0, glm.mToUser, glm.userToM);
        }

        private void nudTrailingAxleLength_Click(object sender, EventArgs e)
        {
            nudTrailingAxleLength.KeypadToButton(ref trailingAxleLength, 0, 30, 0, glm.mToUser, glm.userToM);
        }

        private void nudTrailingHitchLength_Click(object sender, EventArgs e)
        {
            nudTrailingHitchLength.KeypadToButton(ref trailingHitchLength, -trailingAxleLength, 30, 0, glm.mToUser, glm.userToM);
        }
    }
}
