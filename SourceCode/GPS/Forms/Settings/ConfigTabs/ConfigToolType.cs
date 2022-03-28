using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigToolType : UserControl2
    {
        private readonly FormGPS mf;

        public ConfigToolType(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigAttachStyle_Load(object sender, EventArgs e)
        {
            rbtnFixedRear.Checked = Properties.Vehicle.Default.setTool_isToolRearFixed;
            rbtnTBT.Checked = Properties.Vehicle.Default.setTool_isToolTBT;
            rbtnTrailing.Checked = Properties.Vehicle.Default.setTool_isToolTrailing;
            rbtnFront.Checked = Properties.Vehicle.Default.setTool_isToolFront;
        }

        public override void Close()
        {
            Properties.Vehicle.Default.setTool_isToolFront = mf.tool.isToolFrontFixed = rbtnFront.Checked;
            Properties.Vehicle.Default.setTool_isToolTBT = mf.tool.isToolTBT = rbtnTBT.Checked;
            Properties.Vehicle.Default.setTool_isToolTrailing = mf.tool.isToolTrailing = rbtnTrailing.Checked;
            Properties.Vehicle.Default.setTool_isToolRearFixed = mf.tool.isToolRearFixed = rbtnFixedRear.Checked;
            
            if (Properties.Vehicle.Default.setTool_isToolFront == mf.tool.hitchLength < 0)
                mf.tool.hitchLength *= -1;
            Properties.Vehicle.Default.setVehicle_hitchLength = mf.tool.hitchLength;

            Properties.Vehicle.Default.Save();
        }
    }
}
