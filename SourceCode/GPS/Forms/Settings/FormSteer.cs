using System;
using System.Drawing;
using System.Windows.Forms; 

namespace AgOpenGPS
{
    public partial class FormSteer : Form
    {

        private readonly FormGPS mf = null;

        private bool toSend = false, isSA = false;
        private int counter = 0, cntr;
        private vec2 startFix;
        private double diameter, steerAngleRight, dist;
        private int windowSizeState = 0;
        private int MaxCounts;

        //Form stuff
        public FormSteer(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.label3.Text = gStr.gsAgressiveness;
            this.label5.Text = gStr.gsOvershootReduction;
            this.Text = gStr.gsAutoSteerConfiguration;
            Size = MinimumSize;
        }

        private void FormSteer_Load(object sender, EventArgs e)
        {
            //WAS Zero, CPD
            hsbarWasOffset.ValueChanged -= hsbarSteerAngleSensorZero_ValueChanged;
            hsbarCountsPerDegree.ValueChanged -= hsbarCountsPerDegree_ValueChanged;

            hsbarWasOffset.Value = Properties.Settings.Default.setAS_wasOffset;
            hsbarCountsPerDegree.Value = Properties.Settings.Default.setAS_countsPerDegree;

            lblCountsPerDegree.Text = hsbarCountsPerDegree.Value.ToString();
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value / (double)(hsbarCountsPerDegree.Value)).ToString("0.00");

            hsbarWasOffset.ValueChanged += hsbarSteerAngleSensorZero_ValueChanged;
            hsbarCountsPerDegree.ValueChanged += hsbarCountsPerDegree_ValueChanged;

            hsbarAckerman.ValueChanged -= hsbarAckerman_ValueChanged;
            hsbarAckerman.Value = Properties.Settings.Default.setAS_ackerman;
            lblAckerman.Text = hsbarAckerman.Value.ToString();
            hsbarAckerman.ValueChanged += hsbarAckerman_ValueChanged;

            //min pwm, kP
            hsbarMinPWM.ValueChanged -= hsbarMinPWM_ValueChanged;
            hsbarProportionalGain.ValueChanged -= hsbarProportionalGain_ValueChanged;

            hsbarMinPWM.Value = Properties.Settings.Default.setAS_minSteerPWM;
            lblMinPWM.Text = hsbarMinPWM.Value.ToString();

            hsbarProportionalGain.Value = Properties.Settings.Default.setAS_Kp;
            lblProportionalGain.Text = hsbarProportionalGain.Value.ToString();

            hsbarMinPWM.ValueChanged += hsbarMinPWM_ValueChanged;
            hsbarProportionalGain.ValueChanged += hsbarProportionalGain_ValueChanged;


            //low steer, high steer
            hsbarLowSteerPWM.ValueChanged -= hsbarLowSteerPWM_ValueChanged;
            hsbarHighSteerPWM.ValueChanged -= hsbarHighSteerPWM_ValueChanged;

            hsbarLowSteerPWM.Value = Properties.Settings.Default.setAS_lowSteerPWM;
            lblLowSteerPWM.Text = hsbarLowSteerPWM.Value.ToString();

            hsbarHighSteerPWM.Value = Properties.Settings.Default.setAS_highSteerPWM;
            lblHighSteerPWM.Text = hsbarHighSteerPWM.Value.ToString();

            hsbarLowSteerPWM.ValueChanged += hsbarLowSteerPWM_ValueChanged;
            hsbarHighSteerPWM.ValueChanged += hsbarHighSteerPWM_ValueChanged;




            hsbarMaxSteerAngle.Value = (Int16)Properties.Vehicle.Default.setVehicle_maxSteerAngle;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();

            mf.vehicle.stanleyDistanceErrorGain = Properties.Vehicle.Default.stanleyDistanceErrorGain;
            hsbarStanleyGain.Value = (Int16)(mf.vehicle.stanleyDistanceErrorGain * 10);
            lblStanleyGain.Text = mf.vehicle.stanleyDistanceErrorGain.ToString();

            mf.vehicle.stanleyHeadingErrorGain = Properties.Vehicle.Default.stanleyHeadingErrorGain;
            hsbarHeadingErrorGain.Value = (Int16)(mf.vehicle.stanleyHeadingErrorGain * 10);
            lblHeadingErrorGain.Text = mf.vehicle.stanleyHeadingErrorGain.ToString();

            hsbarIntegral.Value = (int)(Properties.Vehicle.Default.stanleyIntegralGainAB * 100);
            lblIntegralPercent.Text = ((int)(mf.vehicle.stanleyIntegralGainAB * 100)).ToString();

            hsbarIntegralPurePursuit.Value = (int)(Properties.Vehicle.Default.purePursuitIntegralGainAB * 100);
            lblPureIntegral.Text = ((int)(mf.vehicle.purePursuitIntegralGain * 100)).ToString();

            hsbarSideHillComp.Value = (int)(Properties.Settings.Default.setAS_sideHillComp * 100);


            //nudIntDistance.Value = (int)(Properties.Vehicle.Default.stanleyIntegralDistanceAwayTriggerAB * 100);

            mf.vehicle.goalPointLookAhead = Properties.Vehicle.Default.setVehicle_goalPointLookAhead;
            hsbarLookAhead.Value = (Int16)(mf.vehicle.goalPointLookAhead * 10);
            lblLookAhead.Text = mf.vehicle.goalPointLookAhead.ToString();

            hsbarLookAheadMult.Value = (Int16)(mf.vehicle.goalPointLookAheadMult * 10);
            lblLookAheadMult.Text = mf.vehicle.goalPointLookAheadMult.ToString();

            //make sure free drive is off
            btnFreeDrive.Image = Properties.Resources.SteerDriveOff;
            mf.vehicle.isInFreeDriveMode = false;
            btnSteerAngleDown.Enabled = false;
            btnSteerAngleUp.Enabled = false;
            //hSBarFreeDrive.Value = 0;
            mf.vehicle.driveFreeSteerAngle = 0;

            toSend = false;

            int sett = Properties.Vehicle.Default.setArdSteer_setting0;

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

            if ((sett & 32) == 32) cboxSteerEnable.Text = "Switch";
            else if ((sett & 64) == 64) cboxSteerEnable.Text = "Button";
            else cboxSteerEnable.Text = "None";

            if ((sett & 128) == 0) cboxEncoder.Checked = false;
            else cboxEncoder.Checked = true;

            MaxCounts = Properties.Vehicle.Default.setArdSteer_maxPulseCounts;
            nudMaxCounts.Text = MaxCounts.ToString();
            hsbarSensor.Value = (int)Properties.Vehicle.Default.setArdSteer_maxPulseCounts;
            lblhsbarSensor.Text = ((int)((double)hsbarSensor.Value * 0.3921568627)).ToString() + "%";

            sett = Properties.Vehicle.Default.setArdSteer_setting1;

            if ((sett & 1) == 0) cboxDanfoss.Checked = false;
            else cboxDanfoss.Checked = true;

            if ((sett & 2) == 0) cboxPressureSensor.Checked = false;
            else cboxPressureSensor.Checked = true;

            if ((sett & 4) == 0) cboxCurrentSensor.Checked = false;
            else cboxCurrentSensor.Checked = true;

            if (cboxEncoder.Checked)
            {
                cboxPressureSensor.Checked = false;
                cboxCurrentSensor.Checked = false;
                label61.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = true;
                pbarSensor.Visible = false;
                hsbarSensor.Visible = false;
                lblhsbarSensor.Visible = false;
                label61.Text = gStr.gsEncoderCounts;
            }
            else if (cboxPressureSensor.Checked)
            {
                cboxEncoder.Checked = false;
                cboxCurrentSensor.Checked = false;
                label61.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = true;
                hsbarSensor.Visible = true;
                lblhsbarSensor.Visible = true;

                label61.Text = "Off at %";
            }
            else if (cboxCurrentSensor.Checked)
            {
                cboxPressureSensor.Checked = false;
                cboxEncoder.Checked = false;
                label61.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = true;
                hsbarSensor.Visible = true;
                lblhsbarSensor.Visible = true;

                label61.Text = "Off at %";
            }
            else
            {
                cboxPressureSensor.Checked = false;
                cboxCurrentSensor.Checked = false;
                cboxEncoder.Checked = false;
                label61.Visible = false;
                lblPercentFS.Visible = false;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = false;
                hsbarSensor.Visible = false;
                lblhsbarSensor.Visible = false;
                return;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            if (isSA)
            {
                //bool isSame = Math.PI - Math.Abs(Math.Abs(cH - mf.gpsHeading) - Math.PI) < (glm.PIBy2+0.1);
                dist = glm.Distance(startFix, mf.pivotAxlePos);
                cntr++;
                if (dist > diameter)
                {
                    diameter = dist;
                    cntr = 0;
                }
                lblDiameter.Text = diameter.ToString("0.00") + " m";

                if (cntr > 9)
                {
                    steerAngleRight = Math.Atan(mf.vehicle.wheelbase / (diameter / 2));
                    steerAngleRight = glm.toDegrees(steerAngleRight);
                    //steerAngleLeft = Math.Atan(mf.vehicle.wheelbase / (diameter / 2 ));
                    //steerAngleLeft = glm.toDegrees(steerAngleLeft);

                    lblCalcSteerAngleInner.Text = steerAngleRight.ToString("0.0") + "\u00B0";
                    //lblCalcSteerAngleOuter.Text = steerAngleLeft.ToString("0.0") + "\u00B0";
                    lblDiameter.Text = diameter.ToString("0.00") + " m";
                    btnStartSA.Image = Properties.Resources.BoundaryRecord;
                    isSA = false;
                }
            }

            double actAng = mf.mc.actualSteerAngleDegrees * 5;
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

            lblSteerAngle.Text = mf.SetSteerAngle;
            lblSteerAngleActual.Text = mf.mc.actualSteerAngleDegrees.ToString("0.0") + "\u00B0";
            lblActualSteerAngleUpper.Text = lblSteerAngleActual.Text;
            double err = (mf.mc.actualSteerAngleDegrees - mf.guidanceLineSteerAngle * 0.01);
            lblError.Text = Math.Abs(err).ToString("0.0") + "\u00B0";
            if (err > 0) lblError.ForeColor = Color.Red;
            else lblError.ForeColor = Color.DarkGreen;

            lblPWMDisplay.Text = mf.mc.pwmDisplay.ToString();
            counter++;
            if (toSend && counter > 4)
            {
                Properties.Settings.Default.setAS_countsPerDegree = mf.p_252.pgn[mf.p_252.countsPerDegree] = unchecked((byte)hsbarCountsPerDegree.Value);
                Properties.Settings.Default.setAS_ackerman = mf.p_252.pgn[mf.p_252.ackerman] = unchecked((byte)hsbarAckerman.Value);

                Properties.Settings.Default.setAS_wasOffset = hsbarWasOffset.Value;
                mf.p_252.pgn[mf.p_252.wasOffsetHi] = unchecked((byte)(hsbarWasOffset.Value >> 8));
                mf.p_252.pgn[mf.p_252.wasOffsetLo] = unchecked((byte)(hsbarWasOffset.Value));

                Properties.Settings.Default.setAS_highSteerPWM = mf.p_252.pgn[mf.p_252.highPWM] = unchecked((byte)hsbarHighSteerPWM.Value);
                Properties.Settings.Default.setAS_lowSteerPWM = mf.p_252.pgn[mf.p_252.lowPWM] = unchecked((byte)hsbarLowSteerPWM.Value);
                Properties.Settings.Default.setAS_Kp = mf.p_252.pgn[mf.p_252.gainProportional] = unchecked((byte)hsbarProportionalGain.Value);
                Properties.Settings.Default.setAS_minSteerPWM = mf.p_252.pgn[mf.p_252.minPWM] = unchecked((byte)hsbarMinPWM.Value);

                Properties.Settings.Default.Save();
                Properties.Vehicle.Default.Save();

                mf.SendPgnToLoop(mf.p_252.pgn);
                toSend = false;
                counter = 0;
            }

            if (hsbarMinPWM.Value > hsbarLowSteerPWM.Value) lblMinPWM.ForeColor = Color.OrangeRed;
            else lblMinPWM.ForeColor = SystemColors.ControlText;

            
            if (mf.mc.sensorData != -1)
            {
                if (mf.mc.sensorData < 0 || mf.mc.sensorData > 255) mf.mc.sensorData = 0;
                SetProgressNoAnimation(pbarSensor, mf.mc.sensorData);
                lblPercentFS.Text = ((int)((double)mf.mc.sensorData * 0.3921568627)).ToString() + "%";
            }
        }

        /// <summary>
        /// Sets the progress bar value, without using 'Windows Aero' animation.
        /// This is to work around a known WinForms issue where the progress bar 
        /// is slow to update. 
        /// </summary>
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

        private void FormSteer_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.vehicle.isInFreeDriveMode = false;

            Properties.Vehicle.Default.setVehicle_goalPointLookAhead = mf.vehicle.goalPointLookAhead;
            Properties.Vehicle.Default.stanleyHeadingErrorGain = mf.vehicle.stanleyHeadingErrorGain;
            Properties.Vehicle.Default.stanleyDistanceErrorGain = mf.vehicle.stanleyDistanceErrorGain;
            Properties.Vehicle.Default.stanleyIntegralGainAB = mf.vehicle.stanleyIntegralGainAB;
            Properties.Vehicle.Default.purePursuitIntegralGainAB = mf.vehicle.purePursuitIntegralGain;
            Properties.Vehicle.Default.setVehicle_goalPointLookAheadMult = mf.vehicle.goalPointLookAheadMult;
            Properties.Vehicle.Default.setVehicle_maxSteerAngle = mf.vehicle.maxSteerAngle;

            Properties.Settings.Default.setAS_countsPerDegree = mf.p_252.pgn[mf.p_252.countsPerDegree] = unchecked((byte)hsbarCountsPerDegree.Value);
            Properties.Settings.Default.setAS_ackerman = mf.p_252.pgn[mf.p_252.ackerman] = unchecked((byte)hsbarAckerman.Value);

            Properties.Settings.Default.setAS_wasOffset = hsbarWasOffset.Value;
            mf.p_252.pgn[mf.p_252.wasOffsetHi] = unchecked((byte)(hsbarWasOffset.Value >> 8));
            mf.p_252.pgn[mf.p_252.wasOffsetLo] = unchecked((byte)(hsbarWasOffset.Value));

            Properties.Settings.Default.setAS_highSteerPWM = mf.p_252.pgn[mf.p_252.highPWM] = unchecked((byte)hsbarHighSteerPWM.Value);
            Properties.Settings.Default.setAS_lowSteerPWM = mf.p_252.pgn[mf.p_252.lowPWM] = unchecked((byte)hsbarLowSteerPWM.Value);
            Properties.Settings.Default.setAS_Kp = mf.p_252.pgn[mf.p_252.gainProportional] = unchecked((byte)hsbarProportionalGain.Value);
            Properties.Settings.Default.setAS_minSteerPWM = mf.p_252.pgn[mf.p_252.minPWM] = unchecked((byte)hsbarMinPWM.Value);

            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();

            //save current vehicle
            SettingsIO.ExportAll(mf.vehiclesDirectory + mf.vehicleFileName + ".XML");
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

        private void hsbarLowSteerPWM_ValueChanged(object sender, EventArgs e)
        {
            if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarHighSteerPWM.Value = hsbarLowSteerPWM.Value;
            lblLowSteerPWM.Text = unchecked((byte)hsbarLowSteerPWM.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarHighSteerPWM_ValueChanged(object sender, EventArgs e)
        {
            if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarLowSteerPWM.Value = hsbarHighSteerPWM.Value;
            lblHighSteerPWM.Text = unchecked((byte)hsbarHighSteerPWM.Value).ToString();
            toSend = true;
            counter = 0;
        }
        #endregion

        #region Steer

        private void hsbarAckerman_ValueChanged(object sender, EventArgs e)
        {
            lblAckerman.Text = unchecked((byte)hsbarAckerman.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarMaxSteerAngle_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.maxSteerAngle = hsbarMaxSteerAngle.Value;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();
        }

        private void hsbarCountsPerDegree_ValueChanged(object sender, EventArgs e)
        {
            lblCountsPerDegree.Text = unchecked((byte)hsbarCountsPerDegree.Value).ToString();
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value / (double)(hsbarCountsPerDegree.Value)).ToString("0.00");
            toSend = true;
            counter = 0;
        }

        private void hsbarSteerAngleSensorZero_ValueChanged(object sender, EventArgs e)
        {
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value / (double)(hsbarCountsPerDegree.Value)).ToString("0.00");
            toSend = true;
            counter = 0;
        }

        private void btnZeroWAS_Click(object sender, EventArgs e)
        {
            int offset = (int)(hsbarCountsPerDegree.Value * -mf.mc.actualSteerAngleDegrees + hsbarWasOffset.Value);
            if (Math.Abs(offset) > 3900) mf.TimedMessageBox(2000, "Exceeded Range", "Excessive Steer Angle - Cannot Zero");
            else
            {
                hsbarWasOffset.Value += (int)(hsbarCountsPerDegree.Value * -mf.mc.actualSteerAngleDegrees);
            }
        }

        private void btnWASZeroReset_Click(object sender, EventArgs e)
        {
            hsbarWasOffset.Value = 0;
        }

        private void btnStartSA_Click(object sender, EventArgs e)
        {
            if (!isSA)
            {
                isSA = true;
                startFix = mf.pivotAxlePos;
                dist = 0;
                diameter = 0;
                cntr = 0;
                btnStartSA.Image = Properties.Resources.boundaryStop;
                lblDiameter.Text = "0";
                lblCalcSteerAngleInner.Text = "Drive Steady";
                //lblCalcSteerAngleOuter.Text = "Consistent Steering Angle!!";
            }
            else
            {
                isSA = false;
                lblCalcSteerAngleInner.Text = "0.0" + "\u00B0";
                //lblCalcSteerAngleOuter.Text = "0.0" + "\u00B0";
                btnStartSA.Image = Properties.Resources.BoundaryRecord;
            }

        }

        #endregion

        # region Stanley

        private void hsbarStanleyGain_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyDistanceErrorGain = hsbarStanleyGain.Value * 0.1;
            lblStanleyGain.Text = mf.vehicle.stanleyDistanceErrorGain.ToString();
        }

        private void hsbarHeadingErrorGain_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyHeadingErrorGain = hsbarHeadingErrorGain.Value * 0.1;
            lblHeadingErrorGain.Text = mf.vehicle.stanleyHeadingErrorGain.ToString();
        }

        private void hsbarIntegral_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyIntegralGainAB = hsbarIntegral.Value * 0.01;
            lblIntegralPercent.Text = hsbarIntegral.Value.ToString();
        }

        #endregion

        #region Pure
        private void hsbarIntegralPurePursuit_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.purePursuitIntegralGain = hsbarIntegralPurePursuit.Value * 0.01;
            lblPureIntegral.Text = hsbarIntegralPurePursuit.Value.ToString();
        }

        private void hsbarSideHillComp_ValueChanged(object sender, EventArgs e)
        {
            double deg = hsbarSideHillComp.Value;
            deg *= 0.01;
            lblSideHillComp.Text = (deg.ToString("0.00") + "\u00B0");
            Properties.Settings.Default.setAS_sideHillComp = deg;
            mf.mc.sideHillCompFactor = deg;
        }


        private void hsbarLookAhead_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAhead = hsbarLookAhead.Value * 0.1;
            lblLookAhead.Text = mf.vehicle.goalPointLookAhead.ToString();
            //mf.AutoSteerSettingsOutToPort();
        }

        private void hsbarLookAheadMult_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadMult = hsbarLookAheadMult.Value * 0.1;
            lblLookAheadMult.Text = mf.vehicle.goalPointLookAheadMult.ToString();
        }

        private void expandWindow_Click(object sender, EventArgs e)
        {
            if (windowSizeState++ > 1) windowSizeState = 0;
            if (windowSizeState == 1) this.Size = new System.Drawing.Size(378, 627);
            else if (windowSizeState == 2) this.Size = new System.Drawing.Size(908, 627);
            else if (windowSizeState == 0) this.Size = new System.Drawing.Size(378, 462);

        }

        private void nudMaxCounts_Click(object sender, EventArgs e)
        {
            nudMaxCounts.KeypadToButton(ref MaxCounts, 0, 255);
        }

        private void EnableAlert_Click(object sender, EventArgs e)
        {
            pboxSendSteer.Visible = true;

            if (sender is CheckBox checkbox)
            {
                if (checkbox.Name == "cboxEncoder" || checkbox.Name == "cboxPressureSensor"
                    || checkbox.Name == "cboxCurrentSensor")
                {
                    if (!checkbox.Checked)
                    {
                        cboxPressureSensor.Checked = false;
                        cboxCurrentSensor.Checked = false;
                        cboxEncoder.Checked = false;
                        label61.Visible = false;
                        lblPercentFS.Visible = false;
                        nudMaxCounts.Visible = false;
                        pbarSensor.Visible = false;
                        hsbarSensor.Visible = false;
                        lblhsbarSensor.Visible = false;
                        return;
                    }

                    if (checkbox == cboxPressureSensor)
                    {
                        cboxEncoder.Checked = false;
                        cboxCurrentSensor.Checked = false;
                        label61.Visible = true;
                        lblPercentFS.Visible = true;
                        nudMaxCounts.Visible = false;
                        pbarSensor.Visible = true;
                        label61.Text = "Off at %";
                        hsbarSensor.Visible = true;
                        lblhsbarSensor.Visible = true;

                    }

                    else if (checkbox == cboxCurrentSensor)
                    {
                        cboxPressureSensor.Checked = false;
                        cboxEncoder.Checked = false;
                        label61.Visible = true;
                        lblPercentFS.Visible = true;
                        nudMaxCounts.Visible = false;
                        hsbarSensor.Visible = true;
                        pbarSensor.Visible = true;
                        label61.Text = "Off at %";
                        lblhsbarSensor.Visible = true;
                    }
                    else if (checkbox == cboxEncoder)
                    {
                        cboxPressureSensor.Checked = false;
                        cboxCurrentSensor.Checked = false;
                        label61.Visible = true;
                        lblPercentFS.Visible = false;
                        nudMaxCounts.Visible = true;
                        pbarSensor.Visible = false;
                        hsbarSensor.Visible = false;
                        lblhsbarSensor.Visible = false;
                        label61.Text = gStr.gsEncoderCounts;
                    }
                }
            }
        }

        private void btnSendSteerConfigPGN_Click(object sender, EventArgs e)
        {
            SaveSettings();
            mf.SendPgnToLoop(mf.p_251.pgn);
            pboxSendSteer.Visible = false;

            mf.TimedMessageBox(1000, gStr.gsAutoSteerPort, "Settings Sent To Steer Module");

        }

        private void SaveSettings()
        {
            byte value = 0;

            if (chkInvertWAS.Checked) value |= 0x01;
            if (chkSteerInvertRelays.Checked) value |= 0x02;
            if (chkInvertSteer.Checked) value |= 0x04;
            if (cboxConv.Text == "Single") value |= 0x08;
            if (cboxMotorDrive.Text == "Cytron") value |= 0x10;
            if (cboxSteerEnable.Text == "Switch") value |= 0x20;
            if (cboxSteerEnable.Text == "Button") value |= 0x40;
            if (cboxEncoder.Checked) value |= 0x80;

            Properties.Vehicle.Default.setArdSteer_setting0 = value;
            Properties.Vehicle.Default.setArdMac_isDanfoss = cboxDanfoss.Checked;

            if (cboxCurrentSensor.Checked || cboxPressureSensor.Checked)
            {
                Properties.Vehicle.Default.setArdSteer_maxPulseCounts = (byte)hsbarSensor.Value;
            }
            else
            {
                Properties.Vehicle.Default.setArdSteer_maxPulseCounts = (byte)MaxCounts;
            }

            // Settings1
            value = 0x00;

            if (cboxDanfoss.Checked) value |= 0x01;
            if (cboxPressureSensor.Checked) value |= 0x02;
            if (cboxCurrentSensor.Checked) value |= 0x04;

            Properties.Vehicle.Default.setArdSteer_setting1 = value;

            Properties.Vehicle.Default.Save();

            mf.p_251.pgn[mf.p_251.set0] = Properties.Vehicle.Default.setArdSteer_setting0;
            mf.p_251.pgn[mf.p_251.set1] = Properties.Vehicle.Default.setArdSteer_setting1;
            mf.p_251.pgn[mf.p_251.maxPulse] = Properties.Vehicle.Default.setArdSteer_maxPulseCounts;
            mf.p_251.pgn[mf.p_251.minSpeed] = 5; //0.5 kmh

            pboxSendSteer.Visible = false;
        }

        private void hsbarSensor_Scroll(object sender, ScrollEventArgs e)
        {
            pboxSendSteer.Visible = true;
            lblhsbarSensor.Text = ((int)((double)hsbarSensor.Value * 0.3921568627)).ToString() + "%";
        }

        #endregion

        #region Free Drive
        private void btnFreeDrive_Click(object sender, EventArgs e)
        {
            if (mf.vehicle.isInFreeDriveMode)
            {
                //turn OFF free drive mode
                btnFreeDrive.Image = Properties.Resources.SteerDriveOff;
                btnFreeDrive.BackColor = Color.FromArgb(50, 50, 70);
                mf.vehicle.isInFreeDriveMode = false;
                btnSteerAngleDown.Enabled = false;
                btnSteerAngleUp.Enabled = false;
                //hSBarFreeDrive.Value = 0;
                mf.vehicle.driveFreeSteerAngle = 0;
            }
            else
            {
                //turn ON free drive mode
                btnFreeDrive.Image = Properties.Resources.SteerDriveOn;
                btnFreeDrive.BackColor = Color.LightGreen;
                mf.vehicle.isInFreeDriveMode = true;
                btnSteerAngleDown.Enabled = true;
                btnSteerAngleUp.Enabled = true;
                //hSBarFreeDrive.Value = 0;
                mf.vehicle.driveFreeSteerAngle = 0;
                lblSteerAngle.Text = "0";
            }
        }

        private void btnFreeDriveZero_Click(object sender, EventArgs e)
        {
            if (mf.vehicle.driveFreeSteerAngle == 0)
                mf.vehicle.driveFreeSteerAngle = 5;
            else mf.vehicle.driveFreeSteerAngle = 0;
            //hSBarFreeDrive.Value = mf.driveFreeSteerAngle;
        }


        private void btnSteerAngleUp_MouseDown(object sender, MouseEventArgs e)
        {
            mf.vehicle.driveFreeSteerAngle++;
            if (mf.vehicle.driveFreeSteerAngle > 40) mf.vehicle.driveFreeSteerAngle = 40;
        }

        private void btnSteerAngleDown_MouseDown(object sender, MouseEventArgs e)
        {
            mf.vehicle.driveFreeSteerAngle--;
            if (mf.vehicle.driveFreeSteerAngle < -40) mf.vehicle.driveFreeSteerAngle = -40;
        }
        #endregion

        #region Help
        private void btnZeroWAS_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_btnZeroWAS, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarWasOffset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarWasOffset, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarCountsPerDegree_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarCountsPerDegree, gStr.gsHelp).ShowDialog(this);

        }

        private void hsbarAckerman_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarAckerman, gStr.gsHelp).ShowDialog(this);
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

        private void hsbarStanleyGain_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarStanleyGain, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarHeadingErrorGain_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarHeadingErrorGain, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarIntegral_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarIntegral, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarLookAhead_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarLookAhead, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarLookAheadMult_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarLookAheadMult, gStr.gsHelp).ShowDialog(this);
        }

        private void hsbarSideHillComp_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_hsbarSideHillComp, gStr.gsHelp).ShowDialog(this);
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

        private void btnStartSA_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.h_btnStartSA, gStr.gsHelp).ShowDialog(this);
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

        private void cboxEncoder_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxEncoder, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxPressureSensor_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxPressureSensor, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxCurrentSensor_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxCurrentSensor, gStr.gsHelp).ShowDialog(this);
        }

        private void nudMaxCounts_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudMaxCounts, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxMotorDrive_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxMotorDrive, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxConv_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxConv, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxSteerEnable_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxSteerEnable, gStr.gsHelp).ShowDialog(this);
        }

        private void pboxSendSteer_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_pboxSendSteer, gStr.gsHelp).ShowDialog(this);
        }

        private void btnSendSteerConfigPGN_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_btnSendSteerConfigPGN, gStr.gsHelp).ShowDialog(this);
        }

        #endregion

    }
}
