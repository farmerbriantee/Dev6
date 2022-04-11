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

            /////////////////////////////////////////////////////////   333333333333333  ////////////////////////////////////////
            //every 3 second update status
            if (threeSecondCounter++ > 10)
            {
                threeSecondCounter = 0;

                updateZoomWindow = true;

                if (this.Controls.Contains(pictureboxStart))
                {
                    this.Controls.Remove(pictureboxStart);
                    pictureboxStart.Dispose();
                }

                //check to make sure the grid is big enough
                worldManager.checkZoomWorldGrid(pivotAxlePos.northing, pivotAxlePos.easting);

                //hide the NAv panel in 6  secs
                if (panelNavigation.Visible)
                {
                    if (navPanelCounter-- < 1) panelNavigation.Visible = false;
                    lblHz.Text = fixUpdateHz + " ~ " + (frameTime.ToString("0.0")) + " " + FixQuality;
                }

                if (bnd.bndList.Count > 0)
                {
                    lblFieldStatus.Text = fd.BoundaryArea + "   " +
                                          fd.WorkedAreaRemain + "    " + fd.TimeTillFinished
                                          + "  " + fd.WorkedAreaRemainPercentage + "      "
                                          + fd.WorkedArea;
                }
                else
                    lblFieldStatus.Text = fd.WorkedArea;

                lblTopData.Text = (tool.toolWidth * mToUserBig).ToString("0.00") + unitsFtM + " - " + vehicleFileName;
                lblFix.Text = FixQuality;
                lblAge.Text = pn.age.ToString("0.0");

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

                lbludpWatchCounts.Text = udpWatchCounts.ToString();

                //save nmea log file
                if (isLogNMEA) FileSaveNMEA();

            }//end every 3 seconds

            //every second update all status ///////////////////////////   1 1 1 1 1 1 ////////////////////////////

            if (oneSecondCounter++ > 2)
            {
                oneSecondCounter = 0;

                //counter used for saving field in background
                secondsCounter++;
                dayNightCounter++;

                if (gyd.CurrentGMode != Mode.None)
                {
                    lblInty.Text = gyd.inty.ToString("0.000");

                    if (gyd.CurrentGMode == Mode.Contour)
                        btnEditAB.Text = "";
                    else
                        btnEditAB.Text = ((int)(gyd.moveDistance * 100)).ToString();
                }
                else
                {
                    lblInty.Text = "";
                    btnEditAB.Text = "";
                }

                distanceToolBtn.Text = (fd.distanceUser * mToUserBig).ToString("0") + unitsFtM + "\r\n" + (fd.workedAreaTotalUser * m2ToUser).ToString("0.00");

                //statusbar flash red undefined headland
                if (mc.isOutOfBounds && BackColor != Color.Tomato)
                {
                    BackColor = Color.Tomato;
                }
                else if (!mc.isOutOfBounds && BackColor == Color.Tomato)
                {
                    if (isDay)
                        BackColor = Properties.Settings.Default.setDisplay_colorDayFrame;
                    else
                        BackColor = Properties.Settings.Default.setDisplay_colorNightFrame;
                }
            }

            //every half of a second update all status  ////////////////    0.5  0.5   0.5    0.5    /////////////////
            if (oneHalfSecondCounter++ > 0)
            {
                oneHalfSecondCounter = 0;

                isFlashOnOff = !isFlashOnOff;

                lblSpeed.Text = (pn.avgSpeed * KMHToUser).ToString("0.0");
            }

            //every fifth second update  ///////////////////////////   FIFTH Fifth ////////////////////////////

            btnAutoSteerConfig.Text = SetSteerAngle + "\r\n" + ActualSteerAngle;

            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;
        }//wait till timer fires again.  

        private void IsBetweenSunriseSunset(double lat, double lon)
        {
            CSunTimes.Instance.CalculateSunRiseSetTimes(pn.latitude, pn.longitude, DateTime.Today, ref sunrise, ref sunset);
            //isDay = (DateTime.Now.Ticks < sunset.Ticks && DateTime.Now.Ticks > sunrise.Ticks);
        }

        public void LoadSettings()
        {
            isMetric = Settings.Default.setMenu_isMetric;

            SetUserScales();

            SetFeaturesOnOff();

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
            worldManager.isGridOn = Settings.Default.setMenu_isGridOn;
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

            SetAutoSteerText();

            if (bnd.isHeadlandOn) btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
            else btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;

            btnChangeMappingColor.BackColor = sectionColor;

            //is rtk on?
            isRTK = Properties.Settings.Default.setGPS_isRTK;
            isRTK_KillAutosteer = Properties.Settings.Default.setGPS_isRTK_KillAutoSteer;

            pn.ageAlarm = Properties.Settings.Default.setGPS_ageAlarm;
            pn.headingTrueDualOffset = Properties.Settings.Default.setGPS_dualHeadingOffset;
            pn.isToolSteering = Properties.Vehicle.Default.setTool_isToolSteering;

            guidanceLookAheadTime = Properties.Settings.Default.setAS_guidanceLookAheadTime;

            gyd.sideHillCompFactor = Properties.Settings.Default.setAS_sideHillComp;

            ahrs = new CAHRS();

            //Set width of section and positions for each section
            SectionSetPosition();

            LineUpManualBtns();

            //fast or slow section update
            isFastSections = Properties.Vehicle.Default.setSection_isFast;

            gyd.rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
            cboxpRowWidth.SelectedIndex = gyd.rowSkipsWidth - 1;
            gyd.Set_Alternate_skips();

            //workswitch stuff
            mc.isWorkSwitchEnabled = Settings.Default.setF_IsWorkSwitchEnabled;
            mc.isWorkSwitchActiveLow = Settings.Default.setF_IsWorkSwitchActiveLow;
            mc.isWorkSwitchManual = Settings.Default.setF_IsWorkSwitchManual;
            mc.isSteerControlsManual = Settings.Default.setF_steerControlsManual;

            minFixStepDist = Settings.Default.setF_minFixStep;

            fd.workedAreaTotalUser = Settings.Default.setF_UserTotalArea;

            gyd.uTurnSmoothing = Settings.Default.setAS_uTurnSmoothing;

            //load the lightbar resolution
            lightbarCmPerPixel = Properties.Settings.Default.setDisplay_lightbarCmPerPixel;

            //Stanley guidance
            isStanleyUsed = Properties.Vehicle.Default.setVehicle_isStanleyUsed;
            btnStanleyPure.Image = isStanleyUsed ? Resources.ModeStanley : Resources.ModePurePursuit;

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
        }

        public void SetFeaturesOnOff()
        {
            tramLinesMenuField.Visible = Properties.Settings.Default.setFeatures.isTramOn;
            headlandToolStripMenuItem.Visible = Properties.Settings.Default.setFeatures.isHeadlandOn;
            topFieldViewToolStripMenuItem.Checked = Properties.Settings.Default.setMenu_isOGLZoomOn == 1;

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
        }

        public void SetAutoSteerText()
        {
            if (Properties.Settings.Default.setAS_isAutoSteerAutoOn) btnAutoSteer.Text = "R";
            else btnAutoSteer.Text = "M";
        }

        public void SetUserScales()
        {

            if (isMetric)
            {
                userToM = 0.01;//cm to m
                mToUser = 100.0;//m to cm

                mToUserBig = 1.0;//m to m
                userBigToM = 1.0;//m to m
                KMHToUser = 1.0;//Km/H to Km/H
                userToKMH = 1.0;//Km/H to Km/H

                m2ToUser = 0.0001;//m2 to Ha

                unitsFtM = " m";
                unitsInCm = " cm";
                unitsHaAc = " Ha";
            }
            else
            {
                userToM = 0.0254;//inches to meters
                mToUser = 39.3701;//meters to inches

                mToUserBig = 3.28084;//meters to feet
                userBigToM = 0.3048;//feet to meters
                KMHToUser = 0.62137;//Km/H to mph
                userToKMH = 1.60934;//mph to Km/H

                m2ToUser = 0.000247105;//m2 to Acres

                unitsInCm = " in";
                unitsFtM = " ft";
                unitsHaAc = " Ac";
            }
        }

        private void ZoomByMouseWheel(object sender, MouseEventArgs e)
        {
            SetZoom(Math.Sign(-e.Delta));
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
            menuStrip1.Left = this.Padding.Left;
            menuStrip1.Top = this.Padding.Top;
            lblAge.Top = this.Padding.Top;
            label1.Top = this.Padding.Top;
            lblFix.Top = 40 + this.Padding.Top;

            lblTopData.Top = this.Padding.Top;
            lblCurveLineName.Top = this.Padding.Top;
            lblCurrentField.Top = 17 + this.Padding.Top;
            lblFieldStatus.Top = 34 + this.Padding.Top;

            panelCaptionBar.Top = this.Padding.Top;
            panelCaptionBar.Left = Width - 360 - this.Padding.Right;

            panelMain.Top = 60 + this.Padding.Top;
            panelMain.Left = this.Padding.Left;
            panelMain.Width = Width - this.Padding.Horizontal;
            panelMain.Height = Height - 60 - this.Padding.Vertical;

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
                panelSim.Top = oglMain.Height - 53;
                panelSim.Left = 70 + oglMain.Width / 2 - 300;
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

            Settings.Default.setDisplay_camPitch = worldManager.camPitch;
            Properties.Settings.Default.setDisplay_camZoom = worldManager.zoomValue;

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
                if (gyd.CurrentGMode == Mode.AB || gyd.CurrentGMode == Mode.Curve)
                {
                    if (point.Y < 90 && point.Y > 30)
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

                            if (!gyd.isYouTurnTriggered)
                            {
                                gyd.isYouTurnRight = !gyd.isYouTurnRight;
                                gyd.ResetCreatedYouTurn();
                            }
                            else
                                btnAutoYouTurn.PerformClick();

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
                                gyd.BuildManualYouTurn(false);
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
                                gyd.BuildManualYouTurn(true);
                                return;
                            }
                        }
                    }

                    if (point.Y < 150 && point.Y > 90)
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
                        SetZoom(5);
                        return;
                    }

                    //++
                    if (point.Y < 90)
                    {
                        SetZoom(-5);
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
        public string GPSHeading { get { return glm.toDegrees(fixHeading).ToString("0.0") + "\u00B0"; } }

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
        public string SetSteerAngle { get { return ((double)(guidanceLineSteerAngle) * 0.01).ToString("0.0"); } }
        public string ActualSteerAngle { get { return ((mc.actualSteerAngleDegrees) ).ToString("0.0") ; } }

        public string FixOffset { get { return ((pn.fixOffset.easting * mToUser).ToString("0") + ", " + (pn.fixOffset.northing * mToUser).ToString("0")); } }

        #endregion properties 
    }//end class
}//end namespace