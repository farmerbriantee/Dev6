using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigAntenna : UserControl2
    {
        private readonly FormGPS mf;

        private double antennaHeight, antennaPivot, antennaOffset;

        public ConfigAntenna(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigAntenna_Load(object sender, EventArgs e)
        {
            antennaHeight = Properties.Vehicle.Default.setVehicle_antennaHeight;
            nudAntennaHeight.Text = (antennaHeight * glm.mToUser).ToString("0");

            antennaPivot = Properties.Vehicle.Default.setVehicle_antennaPivot;
            nudAntennaPivot.Text = (antennaPivot * glm.mToUser).ToString("0");
            
            antennaOffset = Properties.Vehicle.Default.setVehicle_antennaOffset;
            nudAntennaOffset.Text = (antennaOffset * glm.mToUser).ToString("0");

            if (Properties.Vehicle.Default.setVehicle_vehicleType == 0)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaTractor;
            else if (Properties.Vehicle.Default.setVehicle_vehicleType == 1)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaHarvester;
            else if (Properties.Vehicle.Default.setVehicle_vehicleType == 2)
                pboxAntenna.BackgroundImage = Properties.Resources.Antenna4WD;
        }

        public override void Close()
        {
            Properties.Vehicle.Default.setVehicle_antennaPivot = mf.vehicle.antennaPivot = antennaPivot;
            Properties.Vehicle.Default.setVehicle_antennaHeight = mf.vehicle.antennaHeight = antennaHeight;
            Properties.Vehicle.Default.setVehicle_antennaOffset = mf.vehicle.antennaOffset = antennaOffset;

            Properties.Vehicle.Default.Save();
        }

        private void nudAntennaPivot_Click(object sender, EventArgs e)
        {
            nudAntennaPivot.KeypadToButton(ref antennaPivot, -10, 10, 0, glm.mToUser, glm.userToM);
        }

        private void nudAntennaHeight_Click(object sender, EventArgs e)
        {
            nudAntennaHeight.KeypadToButton(ref antennaHeight, 0, 10, 0, glm.mToUser, glm.userToM);
        }

        private void nudAntennaOffset_Click(object sender, EventArgs e)
        {
            nudAntennaOffset.KeypadToButton(ref antennaOffset, -5, 5, 0, glm.mToUser, glm.userToM);
        }
    }
}
