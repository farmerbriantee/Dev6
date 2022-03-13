//Please, if you use this, share the improvements

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Properties;
using System.Globalization;
using System.IO;

//C:\Program Files(x86)\Arduino\hardware\tools\avr / bin / avrdude - CC:\Program Files(x86)\Arduino\hardware\tools\avr / etc / avrdude.conf 
//- v - patmega328p - carduino - PCOM3 - b57600 - D - Uflash:w: C: \Users\FarmPC\AppData\Local\Temp\arduino_build_448484 / Autosteer_UDP_20.ino.hex:i

namespace AgOpenGPS
{
    public enum btnStates { Off, Auto, On, Remote }

    public partial class FormGPS
    {
        //colors for sections and field background
        public byte flagColor = 0;

        //how many cm off line per big pixel
        public int lightbarCmPerPixel;

        //polygon mode for section drawing
        public bool isDrawPolygons;

        public CFeatureSettings featureSettings = new CFeatureSettings();

        public Color sectionColor;
        public Color fieldColorDay;
        public Color fieldColorNight;
        public Color vehicleColor;

        public double vehicleOpacity;
        public byte vehicleOpacityByte;
        public bool isVehicleImage;

        //Is it in 2D or 3D, metric or imperial, display lightbar, display grid etc
        public bool isMetric = true, isLightbarOn = true;
        public bool isUTurnAlwaysOn, isSpeedoOn, isAutoDayNight, isSideGuideLines = true;
        public bool isSkyOn = true, isTextureOn = true;
        public bool isDay = true, isDayTime = true;
        public bool isKeyboardOn = true;

        public bool isUTurnOn = true, isLateralOn = true;

        //master Manual and Auto, 3 states possible
        public btnStates autoBtnState = btnStates.Off;

        public int[] customColorsList = new int[16];

        //sunrise sunset
        public DateTime sunrise = DateTime.Now;
        public DateTime sunset = DateTime.Now;

        public bool isFlashOnOff = false;

        //makes nav panel disappear after 6 seconds
        private int navPanelCounter = 0;

        public uint sentenceCounter = 0;

        //Timer triggers at 125 msec
        private void tmrWatchdog_tick(object sender, EventArgs e)
        {
            //Check for a newline char, if none then just return
            if (++sentenceCounter > 20)
            {
                ShowNoGPSWarning();
                return;
            }

            if (threeSecondCounter++ >= 12)
            {
                threeSecondCounter = 0;
                threeSeconds++;
            }
            if (oneSecondCounter++ >= 4)
            {
                oneSecondCounter = 0;
                oneSecond++;
            }
            if (oneHalfSecondCounter++ >= 2)
            {
                oneHalfSecondCounter = 0;
                oneHalfSecond++;
            }
            if (oneFifthSecondCounter++ >= 0)
            {
                oneFifthSecondCounter = 0;
                oneFifthSecond++;
            }

            /////////////////////////////////////////////////////////   333333333333333  ////////////////////////////////////////
            //every 3 second update status
            if (displayUpdateThreeSecondCounter != threeSeconds)
            {
                //reset the counter
                displayUpdateThreeSecondCounter = threeSeconds;

                //check to make sure the grid is big enough
                worldGrid.checkZoomWorldGrid(pn.fix.northing, pn.fix.easting);

                if (panelNavigation.Visible)
                    lblHz.Text = fixUpdateHz + " ~ " + (frameTime.ToString("N1")) + " " + FixQuality;

                if (isMetric)
                {
                    //fieldStatusStripText.Text = fd.WorkedAreaRemainHectares + "\r\n"+
                    //                               fd.WorkedAreaRemainPercentage +"\r\n" +
                    //                               fd.TimeTillFinished + "\r\n" +
                    //                               fd.WorkRateHectares;
                    if (bnd.bndList.Count > 0)
                        lblFieldStatus.Text = fd.AreaBoundaryLessInnersHectares + "   " +
                                              fd.WorkedAreaRemainHectares  + "    " + fd.TimeTillFinished 
                                              + "  " + fd.WorkedAreaRemainPercentage+"      "
                                              +fd.WorkedHectares ;
                    else
                        lblFieldStatus.Text = fd.WorkedHectares;

                }
                else //imperial
                {
                    if (bnd.bndList.Count > 0)
                        lblFieldStatus.Text = fd.AreaBoundaryLessInnersAcres + "   " + fd.WorkedAreaRemainAcres + "   " + 
                                           fd.TimeTillFinished + "  " + fd.WorkedAreaRemainPercentage + "      " +
                                            fd.WorkedAcres;
                    else
                        lblFieldStatus.Text = fd.WorkedAcres;
                }

                //hide the NAv panel in 6  secs
                if (panelNavigation.Visible)
                {
                    if (navPanelCounter-- < 1) panelNavigation.Visible = false;
                }


                lblTopData.Text = (tool.toolWidth * m2FtOrM).ToString("N2") + unitsFtM + " - " + vehicleFileName;
                lblFix.Text = FixQuality;
                lblAge.Text = pn.age.ToString("N1");

                if (isJobStarted)
                {
                    lblCurrentField.Text = "Field: " + displayFieldName;

                    if (gyd.currentGuidanceLine != null)
                    {
                        lblCurveLineName.Text = (gyd.currentGuidanceLine.mode.HasFlag(Mode.AB) ? "AB-" : "Cur-") + gyd.currentGuidanceLine.Name.Trim();
                    }
                    else lblCurveLineName.Text = string.Empty;
                }
                else
                {
                    lblCurveLineName.Text = lblCurrentField.Text = string.Empty;
                }

                if (isJobStarted)
                {
                    if (gyd.isBtnABLineOn || gyd.isBtnCurveOn)
                    {
                        if (!btnEditAB.Visible)
                        {
                            //btnMakeLinesFromBoundary.Visible = true;
                            btnEditAB.Visible = true;
                            btnSnapToPivot.Visible = true;
                            cboxpRowWidth.Visible = true;
                            btnYouSkipEnable.Visible = true;
                        }
                    }
                    else
                    {
                        if (btnEditAB.Visible)
                        {
                            //btnMakeLinesFromBoundary.Visible = false;
                            btnEditAB.Visible = false;
                            btnSnapToPivot.Visible = false;
                            cboxpRowWidth.Visible = false;
                            btnYouSkipEnable.Visible = false;
                        }
                    }
                }

                lbludpWatchCounts.Text = udpWatchCounts.ToString();

                //save nmea log file
                if (isLogNMEA) FileSaveNMEA();

            }//end every 3 seconds

            //every second update all status ///////////////////////////   1 1 1 1 1 1 ////////////////////////////
            if (displayUpdateOneSecondCounter != oneSecond)
            {
                //reset the counter
                displayUpdateOneSecondCounter = oneSecond;

                //counter used for saving field in background
                minuteCounter++;
                tenMinuteCounter++;

                if (gyd.isBtnCurveOn || gyd.isBtnABLineOn || gyd.isContourBtnOn)
                {
                    lblInty.Text = gyd.inty.ToString("N3");

                    if (!gyd.isContourBtnOn)
                        btnEditAB.Text = ((int)(gyd.moveDistance * 100)).ToString();
                }

                //the main formgps window
                if (isMetric)  //metric or imperial
                {
                    //status strip values
                    distanceToolBtn.Text = fd.DistanceUserMeters + "\r\n" + fd.WorkedUserHectares;

                }
                else  //Imperial Measurements
                {
                    //acres on the master section soft control and sections
                    //status strip values
                    distanceToolBtn.Text = fd.DistanceUserFeet + "\r\n" + fd.WorkedUserAcres;
                }

                //statusbar flash red undefined headland
                if (mc.isOutOfBounds && panelSim.BackColor == Color.Transparent
                    || !mc.isOutOfBounds && panelSim.BackColor == Color.Tomato)
                {
                    if (!mc.isOutOfBounds)
                    {
                        panelSim.BackColor = Color.Transparent;
                    }
                    else
                    {
                        panelSim.BackColor = Color.Tomato;
                    }
                }
            }

            //every half of a second update all status  ////////////////    0.5  0.5   0.5    0.5    /////////////////
            if (displayUpdateHalfSecondCounter != oneHalfSecond)
            {
                //reset the counter
                displayUpdateHalfSecondCounter = oneHalfSecond;

                isFlashOnOff = !isFlashOnOff;

                //AutoSteerAuto button enable - Ray Bear inspired code - Thx Ray!
                //if (isJobStarted && ahrs.isAutoSteerAuto &&
                //    (ABLine.isBtnABLineOn || ct.isContourBtnOn || curve.isBtnCurveOn))
                //{
                //    if (mc.steerSwitchValue == 0)
                //    {
                //        if (!isAutoSteerBtnOn) btnAutoSteer.PerformClick();
                //    }
                //    else
                //    {
                //        if (isAutoSteerBtnOn) btnAutoSteer.PerformClick();
                //    }
                //}
                //// Extension added 29.12.2021 (Othmar Ehrhardt):
                //// If no AB line or path is activated, the work switch has no function and can be used to
                //// control the play button of the Record path feature:
                //else if(panelDrag.Visible && ahrs.isAutoSteerAuto)
                //{
                //    // No AB line activated, the autosteer button can be used to control the play button:
                //    if (isAutoSteerBtnOn && !recPath.isDrivingRecordedPath) btnPathGoStop.PerformClick();
                //    else if(recPath.isDrivingRecordedPath) btnPathGoStop.PerformClick();
                //}

                //Make sure it is off when it should
                if ((!gyd.isBtnABLineOn && !gyd.isContourBtnOn && !gyd.isBtnCurveOn && isAutoSteerBtnOn)
                    ) btnAutoSteer.PerformClick();

                //the main formgps window
                if (isMetric)  //metric or imperial
                {
                    lblSpeed.Text = SpeedKPH;
                    //btnContour.Text = XTE; //cross track error

                }
                else  //Imperial Measurements
                {
                    lblSpeed.Text = SpeedMPH;
                    //btnContour.Text = InchXTE; //cross track error
                }


            } //end every 1/2 second

            //every fifth second update  ///////////////////////////   FIFTH Fifth ////////////////////////////
            if (displayUpdateOneFifthCounter != oneFifthSecond)
            {
                //reset the counter
                displayUpdateOneFifthCounter = oneFifthSecond;

                btnAutoSteerConfig.Text = SetSteerAngle + "\r\n" + ActualSteerAngle;

                secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

                //integralStatusLeftSide.Text = "I: " + gyd.inty.ToString("N3");

                //lblAV.Text = ABLine.angVel.ToString("N3");
            }
        }//wait till timer fires again.  

        private void IsBetweenSunriseSunset(double lat, double lon)
        {
            CSunTimes.Instance.CalculateSunRiseSetTimes(pn.latitude, pn.longitude, DateTime.Today, ref sunrise, ref sunset);
            //isDay = (DateTime.Now.Ticks < sunset.Ticks && DateTime.Now.Ticks > sunrise.Ticks);
        }

        public void LoadSettings()
        {            //metric settings

            CheckSettingsNotNull();

            isMetric = Settings.Default.setMenu_isMetric;

            tramLinesMenuField.Visible = Properties.Settings.Default.setFeatures.isTramOn;
            headlandToolStripMenuItem.Visible = Properties.Settings.Default.setFeatures.isHeadlandOn;

            boundariesToolStripMenuItem.Visible = Properties.Settings.Default.setFeatures.isBoundaryOn;
            //toolStripBtnMakeBndContour.Visible = Properties.Settings.Default.setFeatures.isBndContourOn;
            recordedPathStripMenu.Visible = Properties.Settings.Default.setFeatures.isRecPathOn;
            deleteContourPathsToolStripMenuItem.Visible = Properties.Settings.Default.setFeatures.isHideContourOn;
            webcamToolStrip.Visible = Properties.Settings.Default.setFeatures.isWebCamOn;
            offsetFixToolStrip.Visible = Properties.Settings.Default.setFeatures.isOffsetFixOn;
            btnContour.Visible = Properties.Settings.Default.setFeatures.isContourOn;
            btnAutoYouTurn.Visible = Properties.Settings.Default.setFeatures.isYouTurnOn;
            btnStanleyPure.Visible = Properties.Settings.Default.setFeatures.isSteerModeOn;
            btnStartAgIO.Visible = Properties.Settings.Default.setFeatures.isAgIOOn;

            btnAutoSteer.Visible = Properties.Settings.Default.setFeatures.isAutoSteerOn;
            btnCycleLines.Visible = Properties.Settings.Default.setFeatures.isCycleLinesOn;
            btnManualOffOn.Visible = Properties.Settings.Default.setFeatures.isManualSectionOn;
            btnSectionOffAutoOn.Visible = Properties.Settings.Default.setFeatures.isAutoSectionOn;
            btnABLine.Visible = Properties.Settings.Default.setFeatures.isABLineOn;
            btnCurve.Visible = Properties.Settings.Default.setFeatures.isCurveOn;

            isUTurnOn = Properties.Settings.Default.setFeatures.isUTurnOn;
            isLateralOn = Properties.Settings.Default.setFeatures.isLateralOn;

            if (isMetric)
            {
                inchOrCm2m = 0.01;
                m2InchOrCm = 100.0;

                m2FtOrM = 1.0;
                ftOrMtoM = 1.0;

                inOrCm2Cm = 1.0;
                cm2CmOrIn = 1.0;

                unitsFtM = " m";
                unitsInCm = " cm";
            }
            else
            {
                inchOrCm2m = glm.in2m;
                m2InchOrCm = glm.m2in;

                m2FtOrM = glm.m2ft;
                ftOrMtoM = glm.ft2m;

                inOrCm2Cm = 2.54;
                cm2CmOrIn = 0.3937;


                unitsInCm = " in";
                unitsFtM = " ft";
            }

            udpWatchLimit = Properties.Settings.Default.SetGPS_udpWatchMsec;
            startSpeed = Vehicle.Default.setVehicle_startSpeed;

            sectionColor = Settings.Default.setDisplay_colorSectionsDay;
            fieldColorDay = Properties.Settings.Default.setDisplay_colorFieldDay;
            fieldColorNight = Settings.Default.setDisplay_colorFieldNight;

            vehicleColor = Settings.Default.setDisplay_colorVehicle;

            //load the string of custom colors
            string[] words = Properties.Settings.Default.setDisplay_customColors.Split(',');
            for (int i = 0; i < 16; i++)
            {
                customColorsList[i] = int.Parse(words[i], CultureInfo.InvariantCulture);
            }

            isSkyOn = Settings.Default.setMenu_isSkyOn;
            isTextureOn = Settings.Default.setDisplay_isTextureOn;
            worldGrid.isGridOn = Settings.Default.setMenu_isGridOn;
            isSpeedoOn = Settings.Default.setMenu_isSpeedoOn;
            isAutoDayNight = Settings.Default.setDisplay_isAutoDayNight;
            isSideGuideLines = Settings.Default.setMenu_isSideGuideLines;
            isLightbarOn = Settings.Default.setMenu_isLightbarOn;
            isKeyboardOn = Settings.Default.setDisplay_isKeyboardOn;

            panelNavigation.Location = new System.Drawing.Point(90, 100);
            panelDrag.Location = new System.Drawing.Point(87, 268);

            vehicleOpacity = ((double)(Properties.Settings.Default.setDisplay_vehicleOpacity) * 0.01);
            vehicleOpacityByte = (byte)(255 * ((double)(Properties.Settings.Default.setDisplay_vehicleOpacity) * 0.01));
            isVehicleImage = Properties.Settings.Default.setDisplay_isVehicleImage;

            string directoryName = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            //grab the current vehicle filename - make sure it exists
            vehicleFileName = Vehicle.Default.setVehicle_vehicleName;

            simulatorOnToolStripMenuItem.Checked = Settings.Default.setMenu_isSimulatorOn;
            if (simulatorOnToolStripMenuItem.Checked)
            {
                panelSim.Visible = true;
                timerSim.Enabled = true;
            }
            else
            {
                panelSim.Visible = false;
                timerSim.Enabled = false;
            }

            fixUpdateTime = 1 / (double)fixUpdateHz;

            //set the flag mark button to red dot
            btnFlag.Image = Properties.Resources.FlagRed;


            if (Properties.Settings.Default.setAS_isAutoSteerAutoOn) btnAutoSteer.Text = "R";
            else btnAutoSteer.Text = "M";

            if (bnd.isHeadlandOn) btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
            else btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;

            btnChangeMappingColor.BackColor = sectionColor;
            btnChangeMappingColor.Text = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);

            //is rtk on?
            isRTK = Properties.Settings.Default.setGPS_isRTK;
            isRTK_KillAutosteer = Properties.Settings.Default.setGPS_isRTK_KillAutoSteer;

            pn.ageAlarm = Properties.Settings.Default.setGPS_ageAlarm;
            pn.headingTrueDualOffset = Properties.Settings.Default.setGPS_dualHeadingOffset;

            isAngVelGuidance = Properties.Settings.Default.setAS_isAngVelGuidance;

            guidanceLookAheadTime = Properties.Settings.Default.setAS_guidanceLookAheadTime;

            gyd.sideHillCompFactor = Properties.Settings.Default.setAS_sideHillComp;

            //ahrs.isReverseOn = Properties.Settings.Default.setIMU_isReverseOn;
            //ahrs.reverseComp = Properties.Settings.Default.setGPS_reverseComp;
            //ahrs.forwardComp = Properties.Settings.Default.setGPS_forwardComp;

            ahrs = new CAHRS();

            //Set width of section and positions for each section
            SectionSetPosition();

            LineUpManualBtns();

            //fast or slow section update
            isFastSections = Properties.Vehicle.Default.setSection_isFast;

            gyd.rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
            cboxpRowWidth.SelectedIndex = gyd.rowSkipsWidth - 1;
            gyd.Set_Alternate_skips();

            DisableYouTurnButtons();

            //which heading source is being used
            headingFromSource = Settings.Default.setGPS_headingFromWhichSource;

            //workswitch stuff
            mc.isWorkSwitchEnabled = Settings.Default.setF_IsWorkSwitchEnabled;
            mc.isWorkSwitchActiveLow = Settings.Default.setF_IsWorkSwitchActiveLow;
            mc.isWorkSwitchManual = Settings.Default.setF_IsWorkSwitchManual;
            mc.isSteerControlsManual = Settings.Default.setF_steerControlsManual;

            minFixStepDist = Settings.Default.setF_minFixStep;

            fd.workedAreaTotalUser = Settings.Default.setF_UserTotalArea;

            gyd.uTurnSmoothing = Settings.Default.setAS_uTurnSmoothing;

            tool.halfToolWidth = (tool.toolWidth - tool.toolOverlap) / 2.0;

            //load the lightbar resolution
            lightbarCmPerPixel = Properties.Settings.Default.setDisplay_lightbarCmPerPixel;

            //Stanley guidance
            isStanleyUsed = Properties.Vehicle.Default.setVehicle_isStanleyUsed;
            if (isStanleyUsed)
            {
                btnStanleyPure.Image = Resources.ModeStanley;
            }
            else
            {
                btnStanleyPure.Image = Resources.ModePurePursuit;
            }

            Location = Settings.Default.setWindow_Location;
            Size = Settings.Default.setWindow_Size;

            if (Properties.Settings.Default.setDisplay_isStartFullScreen)
                this.WindowState = FormWindowState.Maximized;

            isTramOnBackBuffer = Properties.Settings.Default.setTram_isTramOnBackBuffer;


            //night mode
            isDay = !Properties.Settings.Default.setDisplay_isDayMode;
            SwapDayNightMode();

            if (!Properties.Settings.Default.setDisplay_isTermsAccepted)
            {
                using (var form = new Form_First())
                {
                    if (form.ShowDialog(this) != DialogResult.OK)
                    {
                        Close();
                    }
                }
            }

            FixPanelsAndMenus();
            camera.camSetDistance = camera.zoomValue * camera.zoomValue * -1;
            SetZoom();
        }

        private void ZoomByMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                if (camera.zoomValue <= 20) camera.zoomValue += camera.zoomValue * 0.06;
                else camera.zoomValue += camera.zoomValue * 0.02;
                if (camera.zoomValue > 120) camera.zoomValue = 120;
                camera.camSetDistance = camera.zoomValue * camera.zoomValue * -1;
                SetZoom();
            }
            else
            {
                if (camera.zoomValue <= 20)
                { if ((camera.zoomValue -= camera.zoomValue * 0.06) < 6.0) camera.zoomValue = 6.0; }
                else { if ((camera.zoomValue -= camera.zoomValue * 0.02) < 6.0) camera.zoomValue = 6.0; }

                camera.camSetDistance = camera.zoomValue * camera.zoomValue * -1;
                SetZoom();
            }
        }

        public void SwapDayNightMode()
        {
            isDay = !isDay;
            if (isDay)
            {
                btnDayNightMode.Image = Properties.Resources.WindowNightMode;

                this.BackColor = Settings.Default.setDisplay_colorDayFrame;
                foreach (Control c in this.Controls)
                {
                    c.ForeColor = Settings.Default.setDisplay_colorTextDay;
                }
            }
            else //nightmode
            {
                btnDayNightMode.Image = Properties.Resources.WindowDayMode;
                this.BackColor = Properties.Settings.Default.setDisplay_colorNightFrame;

                foreach (Control c in this.Controls)
                {
                    c.ForeColor = Settings.Default.setDisplay_colorTextNight;
                }
            }
            btnAutoSteerConfig.ForeColor = Color.Black;
            btnEditAB.ForeColor = Color.Black;

            Properties.Settings.Default.setDisplay_isDayMode = isDay;
            Properties.Settings.Default.Save();
        }

        private void FixPanelsAndMenus()
        {
            panelAB.Size = new System.Drawing.Size(780 + ((Width - 900) / 2), 64);
            panelAB.Location = new Point((Width - 900) / 3 + 64, this.Height - 66);

            if (!isJobStarted)
            {
                oglMain.Left = 75;
                oglMain.Width = this.Width - statusStripLeft.Width - 22; //22
                oglMain.Height = this.Height - 62;
            }
            else
            {
                oglMain.Left = 75;
                oglMain.Width = this.Width - statusStripLeft.Width - 84; //22
                oglMain.Height = this.Height - 120;
            }

            LineUpManualBtns();
        }

        //line up section On Off Auto buttons based on how many there are
        public void LineUpManualBtns()
        {
            int top = oglMain.Height - 31;

            int oglButtonWidth = oglMain.Width * 3 / 4;
            int buttonWidth = oglButtonWidth / tool.numOfSections;
            if (buttonWidth > 400) buttonWidth = 400;

            Size size = new System.Drawing.Size(buttonWidth, 25);
            int Left = (75 + oglMain.Width / 2) - (tool.numOfSections * size.Width) / 2;
            
            if (panelSim.Visible == true)
            {
                panelSim.Top = oglMain.Height + 4;
                panelSim.Left = 75 + oglMain.Width / 2 - 300;
                panelSim.Width = 600;
            }

            //turn section buttons all On
            for (int j = 0; j < MAXSECTIONS; j++)
            {
                section[j].button.Top = top;
                section[j].button.Left = Left + size.Width * j;
                section[j].button.Size = size;
                section[j].button.Visible = tool.numOfSections > j;
            }
        }

        public void SaveFormGPSWindowSettings()
        {
            //save window settings
            if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.setWindow_Location = Location;
                Settings.Default.setWindow_Size = Size;
            }
            else
            {
                Settings.Default.setWindow_Location = RestoreBounds.Location;
                Settings.Default.setWindow_Size = RestoreBounds.Size;
            }

            Settings.Default.setDisplay_camPitch = camera.camPitch;
            Properties.Settings.Default.setDisplay_camZoom = camera.zoomValue;

            Settings.Default.setF_UserTotalArea = fd.workedAreaTotalUser;

            //Settings.Default.setDisplay_panelSnapLocation = panelSnap.Location;
            Settings.Default.setDisplay_panelSimLocation = panelSim.Location;

            Settings.Default.Save();
        }

        public string FindDirection(double heading)
        {
            if (heading < 0) heading += glm.twoPI;

            heading = glm.toDegrees(heading);

            if (heading > 337.5 || heading < 22.5)
            {
                return (" " +  gStr.gsNorth + " ");
            }
            if (heading > 22.5 && heading < 67.5)
            {
                return (" " +  gStr.gsN_East + " ");
            }
            if (heading > 67.5 && heading < 111.5)
            {
                return (" " +  gStr.gsEast + " ");
            }
            if (heading > 111.5 && heading < 157.5)
            {
                return (" " +  gStr.gsS_East + " ");
            }
            if (heading > 157.5 && heading < 202.5)
            {
                return (" " +  gStr.gsSouth + " ");
            }
            if (heading > 202.5 && heading < 247.5)
            {
                return (" " +  gStr.gsS_West + " ");
            }
            if (heading > 247.5 && heading < 292.5)
            {
                return (" " +  gStr.gsWest + " ");
            }
            if (heading > 292.5 && heading < 337.5)
            {
                return (" " +  gStr.gsN_West + " ");
            }
            return (" ?? ");
        }

        //Mouse Clicks 
        private void oglMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //0 at bottom for opengl, 0 at top for windows, so invert Y value
                Point point = oglMain.PointToClient(Cursor.Position);

                if (point.Y < 90 && point.Y > 30 && (gyd.isBtnABLineOn || gyd.isBtnCurveOn))
                {

                    int middle = oglMain.Width / 2 + oglMain.Width / 5;
                    if (point.X > middle - 80 && point.X < middle + 80)
                    {
                        if (isTT)
                        {
                            MessageBox.Show(gStr.h_lblSwapDirectionCancel, gStr.gsHelp);
                            ResetHelpBtn();
                            return;
                        }
                        SwapDirection();
                        return;
                    }

                    //manual uturn triggering
                    middle = oglMain.Width / 2 - oglMain.Width / 4;
                    if (point.X > middle - 140 && point.X < middle && isUTurnOn)
                    {
                        if (isTT)
                        {
                            MessageBox.Show(gStr.h_lblManualTurnCancelTouch, gStr.gsHelp);
                            ResetHelpBtn();
                            return;
                        }

                        if (gyd.isYouTurnTriggered)
                        {
                            gyd.ResetYouTurn();
                        }
                        else
                        {
                            gyd.isYouTurnTriggered = true;
                            gyd.BuildManualYouTurn(false, true);
                            return;
                        }
                    }

                    if (point.X > middle && point.X < middle + 140 && isUTurnOn)
                    {
                        if (isTT)
                        {
                            MessageBox.Show(gStr.h_lblManualTurnCancelTouch, gStr.gsHelp);
                            ResetHelpBtn();
                            return;
                        }

                        if (gyd.isYouTurnTriggered)
                        {
                            gyd.ResetYouTurn();
                        }
                        else
                        {
                            gyd.isYouTurnTriggered = true;
                            gyd.BuildManualYouTurn(true, true);
                            return;
                        }
                    }
                }

                if (point.Y < 150 && point.Y > 90 && (gyd.isBtnABLineOn || gyd.isBtnCurveOn))
                {
                    int middle = oglMain.Width / 2 - oglMain.Width / 4;
                    if (point.X > middle - 140 && point.X < middle && isLateralOn)
                    {
                        if (isTT)
                        {
                            MessageBox.Show(gStr.h_lblLateralTurnTouch, gStr.gsHelp);
                            ResetHelpBtn();
                            return;
                        }

                        gyd.BuildManualYouLateral(false);
                        return;
                    }

                    if (point.X > middle && point.X < middle + 140 && isLateralOn)
                    {
                        if (isTT)
                        {
                            MessageBox.Show(gStr.h_lblLateralTurnTouch, gStr.gsHelp);
                            ResetHelpBtn();
                            return;
                        }

                        gyd.BuildManualYouLateral(true);
                        return;
                    }
                }

                //vehicle direcvtion reset
                int centerLeft = oglMain.Width / 2;
                int centerUp = oglMain.Height / 2;

                if (point.X > centerLeft - 40 && point.X < centerLeft + 40 && point.Y > centerUp - 60 && point.Y < centerUp + 60)
                {
                    if (isTT)
                    {
                        MessageBox.Show(gStr.h_lblVehicleDirectionResetTouch, gStr.gsHelp);        
                        ResetHelpBtn();
                        return;
                    }


                    Array.Clear(stepFixPts, 0, stepFixPts.Length);
                    isFirstHeadingSet = false;
                    isReverse = false;
                    TimedMessageBox(2000, "Reset Direction", "Drive Forward > 1.5 kmh");
                    return;
                }

                //prevent flag selection if flag form is up
                Form fc = Application.OpenForms["FormFlags"];
                if (fc != null)
                {
                    fc.Focus();
                    return;
                }

                if (point.X > oglMain.Width - 80)
                {
                    //---
                    if (point.Y < 180 && point.Y > 90)
                    {
                        if (camera.zoomValue <= 20) camera.zoomValue += camera.zoomValue * 0.2;
                        else camera.zoomValue += camera.zoomValue * 0.1;
                        if (camera.zoomValue > 180) camera.zoomValue = 180;
                        camera.camSetDistance = camera.zoomValue * camera.zoomValue * -1;
                        SetZoom();
                        return;
                    }

                    //++
                    if (point.Y < 90)
                    {
                        if (camera.zoomValue <= 20)
                        { if ((camera.zoomValue -= camera.zoomValue * 0.2) < 6.0) camera.zoomValue = 6.0; }
                        else { if ((camera.zoomValue -= camera.zoomValue * 0.1) < 6.0) camera.zoomValue = 6.0; }

                        camera.camSetDistance = camera.zoomValue * camera.zoomValue * -1;
                        SetZoom();
                        return;
                    }
                }

                //check for help touch on steer circle
                if (isTT)
                {
                    int sizer = oglMain.Height / 9;
                    if(point.Y > oglMain.Height-sizer && point.X > oglMain.Width - sizer)
                    {
                        MessageBox.Show(gStr.h_lblSteerCircleTouch, gStr.gsHelp);
                        ResetHelpBtn();
                        return;
                    }
                }

                mouseX = point.X;
                mouseY = oglMain.Height - point.Y;
                leftMouseDownOnOpenGL = true;
            }

            ResetHelpBtn();
        }
        private void oglZoom_MouseClick(object sender, MouseEventArgs e)
        {
            if ((sender as Control).IsDragging()) return;

            if (oglZoom.Width == 180)
            {
                oglZoom.Width = 300;
                oglZoom.Height = 300;
            }

            else if (oglZoom.Width == 300)
            {
                oglZoom.Width = 180;
                oglZoom.Height = 180;
            }
        } 
        
        //Function to delete flag
        public void DeleteSelectedFlag()
        {
            //delete selected flag and set selected to none
            flagPts.RemoveAt(flagNumberPicked - 1);
            flagNumberPicked = 0;
        }

        public void EnableYouTurnButtons()
        {
            gyd.ResetCreatedYouTurn();

            gyd.isYouTurnBtnOn = false;
            btnAutoYouTurn.Enabled = true;

            btnAutoYouTurn.Image = Properties.Resources.YouTurnNo;
        }

        public void DisableYouTurnButtons()
        {
            //btnAutoYouTurn.Enabled = false;

            gyd.isYouTurnBtnOn = false;
            btnAutoYouTurn.Image = Properties.Resources.YouTurnNo;
            gyd.ResetCreatedYouTurn();
        }

        private void ShowNoGPSWarning()
        {
            //update main window
            sentenceCounter = 300;
            oglMain.MakeCurrent();
            oglMain.Refresh();
        }

        #region Properties // ---------------------------------------------------------------------

        public string Latitude { get { return Convert.ToString(Math.Round(pn.latitude, 7)); } }
        public string Longitude { get { return Convert.ToString(Math.Round(pn.longitude, 7)); } }

        public string SatsTracked { get { return Convert.ToString(pn.satellitesTracked); } }
        public string HDOP { get { return Convert.ToString(pn.hdop); } }
        public string Heading { get { return Convert.ToString(Math.Round(glm.toDegrees(fixHeading), 1)) + "\u00B0"; } }
        public string GPSHeading { get { return (Math.Round(glm.toDegrees(gpsHeading), 1)) + "\u00B0"; } }
        public string FixQuality
        {
            get
            {
                if (timerSim.Enabled)
                    return "Sim: ";
                else if (pn.fixQuality == 0) return "Invalid: ";
                else if (pn.fixQuality == 1) return "GPS single: ";
                else if (pn.fixQuality == 2) return "DGPS : ";
                else if (pn.fixQuality == 3) return "PPS : ";
                else if (pn.fixQuality == 4) return "RTK fix: ";
                else if (pn.fixQuality == 5) return "Float: ";
                else if (pn.fixQuality == 6) return "Estimate: ";
                else if (pn.fixQuality == 7) return "Man IP: ";
                else if (pn.fixQuality == 8) return "Sim: ";
                else return "Unknown: ";
            }
        }

        public string GyroInDegrees
        {
            get
            {
                if (ahrs.imuHeading != 99999)
                    return Math.Round(ahrs.imuHeading, 1) + "\u00B0";
                else return "-";
            }
        }
        public string RollInDegrees
        {
            get
            {
                if (ahrs.imuRoll != 88888)
                    return Math.Round((ahrs.imuRoll), 1) + "\u00B0";
                else return "-";
            }
        }
        public string SetSteerAngle { get { return ((double)(guidanceLineSteerAngle) * 0.01).ToString("N1"); } }
        public string ActualSteerAngle { get { return ((mc.actualSteerAngleDegrees) ).ToString("N1") ; } }

        //Metric and Imperial Properties
        public string SpeedMPH
        {
            get
            {
                return Convert.ToString(Math.Round(avgSpeed*0.62137, 1));
            }
        }
        public string SpeedKPH
        {
            get
            {
                return Convert.ToString(Math.Round(avgSpeed, 1));
            }
        }

        public string FixOffset { get { return (pn.fixOffset.easting.ToString("N2") + ", " + pn.fixOffset.northing.ToString("N2")); } }
        public string FixOffsetInch { get { return ((pn.fixOffset.easting*glm.m2in).ToString("N0")+ ", " + (pn.fixOffset.northing*glm.m2in).ToString("N0")); } }

        public string Altitude { get { return Convert.ToString(Math.Round(pn.altitude,1)); } }
        public string AltitudeFeet { get { return Convert.ToString((Math.Round((pn.altitude * 3.28084),1))); } }
        public string DistPivotM
        {
            get
            {
                if (distancePivotToTurnLine > 0 )
                    return ((int)(distancePivotToTurnLine)) + " m";
                else return "--";
            }
        }
        public string DistPivotFt
        {
            get
            {
                if (distancePivotToTurnLine > 0 ) return (((int)(glm.m2ft * (distancePivotToTurnLine))) + " ft");
                else return "--";
            }
        }

        #endregion properties 
    }//end class
}//end namespace