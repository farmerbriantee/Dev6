using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigDimensions : UserControl2
    {
        private readonly FormGPS mf;

        private double vehicleTrack, wheelbase, minTurnRadius;

        public ConfigDimensions(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigDimensions_Load(object sender, EventArgs e)
        {
            minTurnRadius = mf.vehicle.minTurningRadius;
            nudMinTurnRadius.Text = (minTurnRadius * glm.mToUser).ToString("0");

            wheelbase = mf.vehicle.wheelbase;
            nudWheelbase.Text = (wheelbase * glm.mToUser).ToString("0");

            vehicleTrack = mf.vehicle.trackWidth;
            nudVehicleTrack.Text = (vehicleTrack * glm.mToUser).ToString("0");

            if (mf.vehicle.vehicleType == 0) pictureBox1.Image = Properties.Resources.RadiusWheelBase;
            else if (mf.vehicle.vehicleType == 1) pictureBox1.Image = Properties.Resources.RadiusWheelBaseHarvester;
            else if (mf.vehicle.vehicleType == 2) pictureBox1.Image = Properties.Resources.RadiusWheelBase4WD;
        }

        public override void Close()
        {
            Properties.Vehicle.Default.setVehicle_trackWidth = mf.vehicle.trackWidth = vehicleTrack;
            Properties.Vehicle.Default.setVehicle_minTurningRadius = mf.vehicle.minTurningRadius = minTurnRadius;
            Properties.Vehicle.Default.setVehicle_wheelbase = mf.vehicle.wheelbase = wheelbase;

            mf.tram.halfWheelTrack = mf.vehicle.trackWidth * 0.5;
            mf.vehicle.updateVBO = true;
            mf.tool.updateVBO = true;
            Properties.Vehicle.Default.Save();
        }

        private void nudVehicleTrack_Click(object sender, EventArgs e)
        {
            nudVehicleTrack.KeypadToButton(ref vehicleTrack, 0.5, 20, 0, glm.mToUser, glm.userToM);
        }

        private void nudMinTurnRadius_Click(object sender, EventArgs e)
        {
            nudMinTurnRadius.KeypadToButton(ref minTurnRadius, 0.5, 100, 0, glm.mToUser, glm.userToM);
        }

        private void nudWheelbase_Click(object sender, EventArgs e)
        {
            nudWheelbase.KeypadToButton(ref wheelbase, 0.5, 20, 0, glm.mToUser, glm.userToM);
        }
    }
}
