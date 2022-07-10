using System;
using System.Drawing;
using System.Windows.Forms; 

namespace AgOpenGPS
{
    public partial class FormToolSteer : Form
    {
        private readonly FormGPS mf = null;

        private double AntennaOffset, AntennaHeight;
        private bool toSend = false;
        private int windowSizeState = 0;
        private int counter=0;

        //Form stuff
        public FormToolSteer(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
            this.Text = gStr.gsAutoSteerConfiguration;
            Size = MinimumSize;
        }

        private void FormSteer_Load(object sender, EventArgs e)
        {
            AntennaHeight = mf.tool.AntennaHeight;
            nudToolAntennaHeight.Text = (AntennaHeight * glm.mToUser).ToString("0");

            AntennaOffset = mf.tool.AntennaOffset;
            nudToolAntennaOffset.Text = (AntennaOffset * glm.mToUser).ToString("0");

            cboxToolOnlyGPS.Checked = mf.tool.isSteering;

            //WAS Zero, CPD
            hsbarWasOffset.ValueChanged -= hsbarSteerAngleSensorZero_ValueChanged;
            hsbarCountsPerDegree.ValueChanged -= hsbarCountsPerDegree_ValueChanged;

            hsbarWasOffset.Value = Properties.Vehicle.Default.Tool_wasOffset;
            hsbarCountsPerDegree.Value = Properties.Vehicle.Default.Tool_wasCounts;

            lblCountsPerDegree.Text = hsbarCountsPerDegree.Value.ToString();

            hsbarWasOffset.ValueChanged += hsbarSteerAngleSensorZero_ValueChanged;
            hsbarCountsPerDegree.ValueChanged += hsbarCountsPerDegree_ValueChanged;


            //min pwm, kP
            hsbarMinPWM.ValueChanged -= hsbarMinPWM_ValueChanged;
            hsbarProportionalGain.ValueChanged -= hsbarProportionalGain_ValueChanged;

            hsbarMinPWM.Value = Properties.Vehicle.Default.Tool_MinPWM;
            lblMinPWM.Text = hsbarMinPWM.Value.ToString();

            hsbarProportionalGain.Value = Properties.Vehicle.Default.Tool_P; 
            lblProportionalGain.Text = hsbarProportionalGain.Value.ToString();

            hsbarMinPWM.ValueChanged += hsbarMinPWM_ValueChanged;
            hsbarProportionalGain.ValueChanged += hsbarProportionalGain_ValueChanged;


            //low steer, high steer
            hsbarWindupLimit.ValueChanged -= hsbarWindupLimit_ValueChanged;
            hsbarHighSteerPWM.ValueChanged -= hsbarHighSteerPWM_ValueChanged;

            hsbarWindupLimit.Value = Properties.Vehicle.Default.Tool_windupLimit;
            lblWindupLimit.Text = hsbarWindupLimit.Value.ToString();

            hsbarHighSteerPWM.Value = Properties.Vehicle.Default.Tool_HighPWM;
            lblHighSteerPWM.Text = hsbarHighSteerPWM.Value.ToString();

            hsbarWindupLimit.ValueChanged += hsbarWindupLimit_ValueChanged;
            hsbarHighSteerPWM.ValueChanged += hsbarHighSteerPWM_ValueChanged;

            //max steer, sidehill comp, integral
            hsbarMaxSteerAngle.ValueChanged -= hsbarMaxSteerAngle_ValueChanged;
            //hsbarSideHillComp.ValueChanged -= hsbarSideHillComp_ValueChanged;
            hsbarIntegral.ValueChanged -= hsbarIntegral_ValueChanged;

            hsbarMaxSteerAngle.Value = (Int16)Properties.Vehicle.Default.Tool_maxSteerAngle;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();

            //hsbarSideHillComp.Value = (int)(Properties.Vehicle.Default.setTool_sideHillComp);
            //lblSideHillComp.Text = hsbarSideHillComp.Value.ToString();

            hsbarIntegral.Value = (int)(Properties.Vehicle.Default.Tool_I);
            lblPureIntegral.Text = hsbarIntegral.Value.ToString();

            hsbarMaxSteerAngle.ValueChanged += hsbarMaxSteerAngle_ValueChanged;
            //hsbarSideHillComp.ValueChanged += hsbarSideHillComp_ValueChanged;
            hsbarIntegral.ValueChanged += hsbarIntegral_ValueChanged;

            //make sure free drive is off
            btnFreeDrive.Image = Properties.Resources.SteerDriveOff;
            mf.vehicle.isInFreeDriveMode = false;
            btnToolDistanceDown.Enabled = false;
            btnToolDistanceUp.Enabled = false;
            //hSBarFreeDrive.Value = 0;
            mf.vehicle.driveFreeToolDistance = 0;

            toSend = false;

            int sett = Properties.Vehicle.Default.setArdToolSteer_setting0;

            if ((sett & 1) == 0) chkInvertWAS.Checked = false;
            else chkInvertWAS.Checked = true;

            if ((sett & 2) == 0) chkSteerInvertRelays.Checked = false;
            else chkSteerInvertRelays.Checked = true;

            if ((sett & 4) == 0) chkInvertSteer.Checked = false;
            else chkInvertSteer.Checked = true;

            if ((sett & 8) == 0) cboxConv.Text = "Differential";
            else cboxConv.Text = "Single";

            if ((sett & 16) == 0) cboxMotorDrive.Text = "IBT2";
            else cboxMotorDrive.Text = "Cytron";

            if ((sett & 32) == 32) cboxDanfoss.Checked = true;
            else cboxDanfoss.Checked = false;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            double actAng = mf.mc.toolActualDistance * 5;  //Fixx
            if (actAng > 0)
            {
                if (actAng > 49) actAng = 49;
                SetProgressNoAnimation(pbarRight, (int)actAng);
                pbarLeft.Value = 0;
            }
            else
            {
                if (actAng < -49) actAng = -49;
                pbarRight.Value = 0;
                SetProgressNoAnimation(pbarLeft, (int)-actAng);
            }

            lblToolDistanceSet.Text = mf.vehicle.driveFreeToolDistance.ToString("0.0");
            lblToolDistanceActual.Text = mf.mc.toolActualDistance.ToString("0.0");
            lblActualSteerAngleUpper.Text = lblToolDistanceActual.Text;

            double err = (mf.mc.toolActualDistance - mf.vehicle.driveFreeToolDistance);
            lblError.Text = Math.Abs(err).ToString("0.0");
            if (err > 0) lblError.ForeColor = Color.Red;
            else lblError.ForeColor = Color.DarkGreen;

            lblPWMDisplay.Text = mf.mc.pwmDisplay.ToString();
            counter++;
            if (toSend && counter > 4)
            {
                //Fixx
                Properties.Vehicle.Default.Tool_maxSteerAngle = mf.p_232.pgn[mf.p_232.maxSteer] = unchecked((byte)hsbarMaxSteerAngle.Value);
                Properties.Vehicle.Default.Tool_wasCounts = mf.p_232.pgn[mf.p_232.wasCounts] = unchecked((byte)hsbarCountsPerDegree.Value);

                Properties.Vehicle.Default.Tool_wasOffset = mf.p_232.pgn[mf.p_232.wasOffset] = unchecked((byte)hsbarWasOffset.Value);

                Properties.Vehicle.Default.Tool_HighPWM = mf.p_232.pgn[mf.p_232.highPWM] = unchecked((byte)hsbarHighSteerPWM.Value);
                Properties.Vehicle.Default.Tool_windupLimit = mf.p_232.pgn[mf.p_232.windup] = unchecked((byte)hsbarWindupLimit.Value);
                Properties.Vehicle.Default.Tool_P = mf.p_232.pgn[mf.p_232.P] = unchecked((byte)hsbarProportionalGain.Value);
                Properties.Vehicle.Default.Tool_MinPWM = mf.p_232.pgn[mf.p_232.minPWM] = unchecked((byte)hsbarMinPWM.Value);
                
                Properties.Vehicle.Default.Tool_I = mf.p_232.pgn[mf.p_232.I] = unchecked((byte)hsbarIntegral.Value);

                mf.SendPgnToLoop(mf.p_232.pgn);
                toSend = false;
                counter = 0;
            }

            if (hsbarMinPWM.Value > hsbarWindupLimit.Value) lblMinPWM.ForeColor = Color.OrangeRed;
            else lblMinPWM.ForeColor = SystemColors.ControlText;

        }

        private void FormSteer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Vehicle.Default.Tool_antennaHeight = mf.tool.AntennaHeight = AntennaHeight;
            Properties.Vehicle.Default.Tool_antennaOffset = mf.tool.AntennaOffset = AntennaOffset;


            mf.vehicle.isInFreeDriveMode = false;

            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();

            //save current vehicle
            SettingsIO.ExportAll(mf.vehiclesDirectory + mf.vehicleFileName + ".XML");
        }

        private void nudToolAntennaHeight_Click(object sender, EventArgs e)
        {
            nudToolAntennaHeight.KeypadToButton(ref AntennaHeight, 0.0, 10.0, 0, glm.mToUser, glm.userToM);
        }

        private void nudToolAntennaOffset_Click(object sender, EventArgs e)
        {
            nudToolAntennaOffset.KeypadToButton(ref AntennaOffset, -5.0, 5.0, 0, glm.mToUser, glm.userToM);
        }

        public void SetProgressNoAnimation(ProgressBar pb, int value)
        {
            // To get around the progressive animation, we need to move the 
            // progress bar backwards.
            if (value == pb.Maximum)
            {
                // Special case as value can't be set greater than Maximum.
                pb.Maximum = value + 1;     // Temporarily Increase Maximum
                pb.Value = value + 1;       // Move past
                pb.Maximum = value;         // Reset maximum
            }
            else
            {
                pb.Value = value + 1;       // Move past
            }
            pb.Value = value;               // Move to correct value
        }

        #region Gain
        private void hsbarMinPWM_ValueChanged(object sender, EventArgs e)
        {
            lblMinPWM.Text = unchecked((byte)hsbarMinPWM.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarProportionalGain_ValueChanged(object sender, EventArgs e)
        {
            lblProportionalGain.Text = unchecked((byte)hsbarProportionalGain.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarWindupLimit_ValueChanged(object sender, EventArgs e)
        {
            lblWindupLimit.Text = unchecked((byte)hsbarWindupLimit.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarHighSteerPWM_ValueChanged(object sender, EventArgs e)
        {
            if (hsbarWindupLimit.Value > hsbarHighSteerPWM.Value) hsbarWindupLimit.Value = hsbarHighSteerPWM.Value;
            lblHighSteerPWM.Text = unchecked((byte)hsbarHighSteerPWM.Value).ToString();
            toSend = true;
            counter = 0;
        }
        #endregion

        #region Steer
        private void hsbarMaxSteerAngle_ValueChanged(object sender, EventArgs e)
        {
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarCountsPerDegree_ValueChanged(object sender, EventArgs e)
        {
            lblCountsPerDegree.Text = unchecked((byte)hsbarCountsPerDegree.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarSteerAngleSensorZero_ValueChanged(object sender, EventArgs e)
        {
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value - 127).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarIntegral_ValueChanged(object sender, EventArgs e)
        {
            lblPureIntegral.Text = hsbarIntegral.Value.ToString();
            toSend = true;
            counter = 0;
        }

        //private void hsbarSideHillComp_ValueChanged(object sender, EventArgs e)
        //{
        //    lblSideHillComp.Text = hsbarSideHillComp.Value.ToString();
        //    toSend = true;
        //    counter = 0;
        //}

        #endregion

        private void expandWindow_Click(object sender, EventArgs e)
        {
            if (windowSizeState++ > 1) windowSizeState = 0;
            if (windowSizeState == 1) this.Size = new System.Drawing.Size(411, 652);
            else if (windowSizeState == 2) this.Size = new System.Drawing.Size(908, 652);
            else if (windowSizeState == 0) this.Size = new System.Drawing.Size(411, 521);

        }

        private void EnableAlert_Click(object sender, EventArgs e)
        {
            pboxSendSteer.Visible = true;
        }

        private void btnSendSteerConfigPGN_Click(object sender, EventArgs e)
        {
            SaveSettings();
            mf.SendPgnToLoop(mf.p_231.pgn);
            pboxSendSteer.Visible = false;

            mf.TimedMessageBox(1000, gStr.gsMachinePort, "Settings Sent To Tool Steer Module");
        }

        private void SaveSettings()
        {
            byte value = 0x00;

            if (chkInvertWAS.Checked) value |= 0x01;
            if (chkSteerInvertRelays.Checked) value |= 0x02;
            if (chkInvertSteer.Checked) value |= 0x04;
            if (cboxConv.Text == "Single") value |= 0x08;
            if (cboxMotorDrive.Text == "Cytron") value |= 0x10;
            if (cboxDanfoss.Checked) value |= 0x20;

            Properties.Vehicle.Default.setArdToolSteer_setting0 = mf.p_231.pgn[mf.p_231.set0] = value;
            Properties.Vehicle.Default.Save();

            pboxSendSteer.Visible = false;
        }


        #region Free Drive
        private void btnFreeDrive_Click(object sender, EventArgs e)
        {
            if (mf.vehicle.isInFreeToolDriveMode)
            {
                //turn OFF free drive mode
                btnFreeDrive.Image = Properties.Resources.SteerDriveOff;
                btnFreeDrive.BackColor = Color.FromArgb(50, 50, 70);
                mf.vehicle.isInFreeToolDriveMode = false;
                btnToolDistanceDown.Enabled = false;
                btnToolDistanceUp.Enabled = false;
                //hSBarFreeDrive.Value = 0;
                mf.vehicle.driveFreeToolDistance = 0;
            }
            else
            {
                //turn ON free drive mode
                btnFreeDrive.Image = Properties.Resources.SteerDriveOn;
                btnFreeDrive.BackColor = Color.LightGreen;
                mf.vehicle.isInFreeToolDriveMode = true;
                btnToolDistanceDown.Enabled = true;
                btnToolDistanceUp.Enabled = true;
                //hSBarFreeDrive.Value = 0;
                mf.vehicle.driveFreeToolDistance = 0;
                lblToolDistanceSet.Text = "0";
            }
        }

        private void btnFreeDriveZero_Click(object sender, EventArgs e)
        {
            if (mf.vehicle.driveFreeToolDistance == 0)
                mf.vehicle.driveFreeToolDistance = 5;
            else mf.vehicle.driveFreeToolDistance = 0;
        }


        private void btnSteerAngleUp_MouseDown(object sender, MouseEventArgs e)
        {
            mf.vehicle.driveFreeToolDistance++;
            if (mf.vehicle.driveFreeToolDistance > 40) mf.vehicle.driveFreeToolDistance = 40;
        }

        private void btnSteerAngleDown_MouseDown(object sender, MouseEventArgs e)
        {
            mf.vehicle.driveFreeToolDistance--;
            if (mf.vehicle.driveFreeToolDistance < -40) mf.vehicle.driveFreeToolDistance = -40;
        }
        #endregion

        private void cboxToolOnlyGPS_Click(object sender, EventArgs e)
        {
            Properties.Vehicle.Default.Tool_isTooLSteering = mf.tool.isSteering = cboxToolOnlyGPS.Checked;
            Properties.Settings.Default.Save();
        }

        #region Help

        private void hsbarWasOffset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarWasOffset, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarCountsPerDegree_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarCountsPerDegree, gStr.gsHelp).ShowDialog(this);

        }

        private void hsbarMaxSteerAngle_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarMaxSteerAngle, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarProportionalGain_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarProportionalGain, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarHighSteerPWM_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarHighSteerPWM, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarLowSteerPWM_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarLowSteerPWM, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarMinPWM_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarMinPWM, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarIntegralPurePursuit_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarIntegralPurePursuit, gStr.gsHelp).ShowDialog(this);
        }

        private void btnFreeDrive_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_btnFreeDrive, gStr.gsHelp).ShowDialog(this);
        }

        private void btnSteerAngleDown_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_btnSteerAngleDown, gStr.gsHelp).ShowDialog(this);
        }

        private void btnSteerAngleUp_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_btnSteerAngleUp, gStr.gsHelp).ShowDialog(this);
        }

        private void btnFreeDriveZero_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_btnFreeDriveZero, gStr.gsHelp).ShowDialog(this);
        }

        private void chkInvertWAS_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkInvertWAS, gStr.gsHelp).ShowDialog(this);
        }

        private void chkInvertSteer_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkInvertSteer, gStr.gsHelp).ShowDialog(this);
        }

        private void chkSteerInvertRelays_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkSteerInvertRelays, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxDanfoss_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxDanfoss, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxMotorDrive_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxMotorDrive, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxConv_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxConv, gStr.gsHelp).ShowDialog(this);
        }

        private void pboxSendSteer_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_pboxSendSteer, gStr.gsHelp).ShowDialog(this);
        }

        #endregion

    }
}
