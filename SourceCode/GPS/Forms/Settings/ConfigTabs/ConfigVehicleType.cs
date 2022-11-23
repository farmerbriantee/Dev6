using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigVehicleType : UserControl2
    {
        private readonly FormGPS mf;

        public ConfigVehicleType(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigVehicleType_Load(object sender, EventArgs e)
        {
            if (mf.vehicle.vehicleType == 0) rbtnTractor.Checked = true;
            else if (mf.vehicle.vehicleType == 1) rbtnHarvester.Checked = true;
            else if (mf.vehicle.vehicleType == 2) rbtn4WD.Checked = true;
        }

        public override void Close()
        {
            if (rbtnTractor.Checked)
            {
                Properties.Vehicle.Default.setVehicle_vehicleType = mf.vehicle.vehicleType = 0;
            }
            else if (rbtnHarvester.Checked)
            {
                Properties.Vehicle.Default.setVehicle_vehicleType = mf.vehicle.vehicleType = 1;
            }
            else if (rbtn4WD.Checked)
            {
                Properties.Vehicle.Default.setVehicle_vehicleType = mf.vehicle.vehicleType = 2;
            }

            Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna = mf.vehicle.isPivotBehindAntenna = mf.vehicle.vehicleType != 2;
            Properties.Vehicle.Default.setVehicle_isSteerAxleAhead = mf.vehicle.isSteerAxleAhead = mf.vehicle.vehicleType != 1;
            mf.vehicle.updateVBO = true;
            Properties.Vehicle.Default.Save();
        }
    }
}
