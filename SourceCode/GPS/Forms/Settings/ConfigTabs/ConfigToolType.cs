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
            rbtnFixedRear.Checked = mf.tool.isToolRearFixed;
            rbtnTBT.Checked = mf.tool.isToolTBT;
            rbtnTrailing.Checked = mf.tool.isToolTrailing && !mf.tool.isToolTBT;
            rbtnFront.Checked = mf.tool.isToolFrontFixed;
        }

        public override void Close()
        {
            Properties.Vehicle.Default.Tool_isToolFront = mf.tool.isToolFrontFixed = rbtnFront.Checked;
            Properties.Vehicle.Default.Tool_isTBT = mf.tool.isToolTBT = rbtnTBT.Checked;
            Properties.Vehicle.Default.Tool_isTrailing = mf.tool.isToolTrailing = rbtnTrailing.Checked || rbtnTBT.Checked;
            Properties.Vehicle.Default.Tool_isToolRearFixed = mf.tool.isToolRearFixed = rbtnFixedRear.Checked;
            
            if (mf.tool.isToolFrontFixed == mf.tool.hitchLength < 0)
                mf.tool.hitchLength *= -1;
            Properties.Vehicle.Default.setVehicle_hitchLength = mf.tool.hitchLength;

            Properties.Vehicle.Default.Save();
        }
    }
}
