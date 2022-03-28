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
            minTurnRadius = Properties.Vehicle.Default.setVehicle_minTurningRadius;
            nudMinTurnRadius.Text = (minTurnRadius * mf.mToUser).ToString("0");

            wheelbase = Properties.Vehicle.Default.setVehicle_wheelbase;
            nudWheelbase.Text = (wheelbase * mf.mToUser).ToString("0");

            vehicleTrack = Properties.Vehicle.Default.setVehicle_trackWidth;
            nudVehicleTrack.Text = (vehicleTrack * mf.mToUser).ToString("0");

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
            Properties.Vehicle.Default.Save();
        }

        private void nudVehicleTrack_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudVehicleTrack, ref vehicleTrack, 0.5, 20, 0, true, mf.mToUser, mf.userToM);
        }

        private void nudMinTurnRadius_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudMinTurnRadius, ref minTurnRadius, 0.5, 100, 0, true, mf.mToUser, mf.userToM);
        }

        private void nudWheelbase_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudWheelbase, ref wheelbase, 0.5, 20, 0, true, mf.mToUser, mf.userToM);
        }
    }
}
