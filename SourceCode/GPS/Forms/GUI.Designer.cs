//Please, if you use this, share the improvements

using AgOpenGPS.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

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

        public Color sectionColor;
        public Color fieldColorDay;
        public Color fieldColorNight;
        public Color vehicleColor;

        public double vehicleOpacity;
        public byte vehicleOpacityByte;
        public bool isVehicleImage;

        //Is it in 2D or 3D, metric or imperial, display lightbar, display grid etc
        public bool isSpeedoOn, isAutoDayNight, isSideGuideLines = true;
        public bool isSkyOn = true, isTextureOn = true, isLightbarOn = true;
        public bool isDay = true, isDayTime = true;

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
                if (panelNavigation.Visible && navPanelCounter-- < 1)
                {
                    panelNavigation.Visible = false;
                }

                if (bnd.bndList.Count > 0)
                {
                    lblFieldStatus.Text = bnd.BoundaryArea + "   " +
                                          bnd.WorkedAreaRemain + "    " + bnd.TimeTillFinished
                                          + "  " + bnd.WorkedAreaRemainPercentage + "      "
                                          + bnd.WorkedArea;
                }
                else
                    lblFieldStatus.Text = bnd.WorkedArea;

                lblTopData.Text = (tool.toolWidth * glm.mToUserBig).ToString("0.00") + glm.unitsFtM + " - " + vehicleFileName;
                lblFix.Text = FixQuality;
                lblAge.Text = "Age: " + mc.age.ToString("0.0");
                lblHz.Text = HzTime.ToString("0.0") + " ~ " + frameTime.ToString("0.0");

                if (isJobStarted)
                {
                    lblCurrentField.Text = "Field: " + displayFieldName;

                    if (gyd.currentGuidanceLine != null)
                    {
                        string text = gyd.currentGuidanceLine.mode.ToString();
                        lblCurveLineName.Text = text.Substring(0, text.Length > 3 ? 3 : text.Length) + "-" + gyd.currentGuidanceLine.Name.Trim();
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

                distanceToolBtn.Text = (bnd.distanceUser * glm.mToUserBig).ToString("0") + glm.unitsFtM + "\r\n" + (bnd.workedAreaTotalUser * glm.m2ToUser).ToString("0.00");

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

                lblSpeed.Text = (mc.avgSpeed * glm.KMHToUser).ToString("0.0");
            }

            //every fifth second update  ///////////////////////////   FIFTH Fifth ////////////////////////////

            btnAutoSteerConfig.Text = SetSteerAngle + "\r\n" + ActualSteerAngle;
        }//wait till timer fires again.  

        private void IsBetweenSunriseSunset(double lat, double lon)
        {
            CSunTimes.Instance.CalculateSunRiseSetTimes(mc.latitude, mc.longitude, DateTime.Today, ref sunrise, ref sunset);
            //isDay = (DateTime.Now.Ticks < sunset.Ticks && DateTime.Now.Ticks > sunrise.Ticks);
        }

        public void LoadSettings()
        {
            //set the language to last used
            SetLanguage(Properties.Settings.Default.setF_culture);

            //get the fields directory, if not exist, create
            fieldsDirectory = baseDirectory + "Fields\\";
            string dir = Path.GetDirectoryName(fieldsDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            //get the fields directory, if not exist, create
            vehiclesDirectory = baseDirectory + "Vehicles\\";
            dir = Path.GetDirectoryName(vehiclesDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            //make sure current field directory exists, null if not
            currentFieldDirectory = Properties.Settings.Default.setF_CurrentDir;

            glm.SetUserScales(Settings.Default.setMenu_isMetric);

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
            glm.isKeyboardOn = Settings.Default.setDisplay_isKeyboardOn;

            panelNavigation.Location = new System.Drawing.Point(90, 100);

            vehicleOpacity = ((double)(Properties.Settings.Default.setDisplay_vehicleOpacity) * 0.01);
            vehicleOpacityByte = (byte)(255 * ((double)(Properties.Settings.Default.setDisplay_vehicleOpacity) * 0.01));
            isVehicleImage = Properties.Settings.Default.setDisplay_isVehicleImage;

            //grab the current vehicle filename - make sure it exists
            vehicleFileName = Vehicle.Default.setVehicle_vehicleName;

            SetSimStatus(Settings.Default.setMenu_isSimulatorOn);

            //set the flag mark button to red dot
            btnFlag.Image = Properties.Resources.FlagRed;

            SetAutoSteerText();

            if (bnd.isHeadlandOn) btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
            else btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;

            btnChangeMappingColor.BackColor = sectionColor;

            //is rtk on?
            isRTK = Properties.Settings.Default.setGPS_isRTK;
            isRTK_KillAutosteer = Properties.Settings.Default.setGPS_isRTK_KillAutoSteer;

            mc.ageAlarm = Properties.Settings.Default.setGPS_ageAlarm;
            mc.headingTrueDualOffset = Properties.Settings.Default.setGPS_dualHeadingOffset;

            guidanceLookAheadTime = Properties.Settings.Default.setAS_guidanceLookAheadTime;

            vehicle.LoadSettings();
            tool.LoadSettings();
            mc.LoadSettings();

            vehicle.updateVBO = true;
            tool.updateVBO = true;

            LineUpManualBtns();

            gyd.uTurnSmoothing = Settings.Default.setAS_uTurnSmoothing;
            gyd.rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
            cboxpRowWidth.SelectedIndex = gyd.rowSkipsWidth - 1;
            gyd.Set_Alternate_skips();
            

            minFixStepDist = Settings.Default.setF_minFixStep;

            bnd.workedAreaTotalUser = Settings.Default.setF_UserTotalArea;


            //load the lightbar resolution
            lightbarCmPerPixel = Properties.Settings.Default.setDisplay_lightbarCmPerPixel;

            //Stanley guidance
            isStanleyUsed = Properties.Vehicle.Default.setVehicle_isStanleyUsed;
            btnStanleyPure.Image = isStanleyUsed ? Resources.ModeStanley : Resources.ModePurePursuit;

            //night mode
            isDay = !Properties.Settings.Default.setDisplay_isDayMode;
            SwapDayNightMode();
        }

        public void SetFeaturesOnOff()
        {
            topFieldViewToolStripMenuItem.Checked = Properties.Settings.Default.setMenu_isOGLZoomOn == 1;

            tramLinesMenuField.Visible = Properties.Settings.Default.setDisplayFeature_Tram;
            headlandToolStripMenuItem.Visible = Properties.Settings.Default.setDisplayFeature_Headland;
            boundariesToolStripMenuItem.Visible = Properties.Settings.Default.setDisplayFeature_Boundary;
            toolStripBtnMakeBndContour.Visible = Properties.Settings.Default.setDisplayFeature_BoundaryContour;

            toolStripBtnField.Image = isJobStarted && Properties.Settings.Default.setDisplayFeature_SimpleCloseField ? Properties.Resources.JobClose : Properties.Resources.JobActive;

            // = Properties.Settings.Default.setDisplayFeature_ABSmooth;
            deleteContourPathsToolStripMenuItem.Visible = Properties.Settings.Default.setDisplayFeature_HideContour;
            webcamToolStrip.Visible = Properties.Settings.Default.setDisplayFeature_Webcam;
            offsetFixToolStrip.Visible = Properties.Settings.Default.setDisplayFeature_OffsetFix;

            btnStanleyPure.Visible = Properties.Settings.Default.setDisplayFeature_SteerMode;
            btnStartAgIO.Visible = Properties.Settings.Default.setDisplayFeature_AgIO;

            btnContour.Visible = Properties.Settings.Default.setDisplayFeature_Contour;
            btnAutoYouTurn.Visible = Properties.Settings.Default.setDisplayFeature_YouTurn;
            btnSectionOffAutoOn.Visible = Properties.Settings.Default.setDisplayFeature_AutoSection;
            btnManualOffOn.Visible = Properties.Settings.Default.setDisplayFeature_ManualSection;
            btnCycleLines.Visible = Properties.Settings.Default.setDisplayFeature_CycleLines;
            btnABLine.Visible = Properties.Settings.Default.setDisplayFeature_ABLine;
            btnCurve.Visible = Properties.Settings.Default.setDisplayFeature_Curve;
            btnRecPath.Visible = Properties.Settings.Default.setDisplayFeature_RecPath;
            btnAutoSteer.Visible = Properties.Settings.Default.setDisplayFeature_AutoSteer;
            isUTurnOn = Properties.Settings.Default.setDisplayFeature_UTurn;
            isLateralOn = Properties.Settings.Default.setDisplayFeature_Lateral;
        }

        public void SetAutoSteerText()
        {
            if (Properties.Settings.Default.setAS_isAutoSteerAutoOn) btnAutoSteer.Text = "R";
            else btnAutoSteer.Text = "M";
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

        //line up section On Off Auto buttons based on how many there are
        public void LineUpManualBtns()
        {
            int top = oglMain.Height + 30;

            int oglButtonWidth = oglMain.Width * 3 / 4;
            int buttonWidth = oglButtonWidth / tool.numOfSections;
            if (buttonWidth > 400) buttonWidth = 400;

            Size size = new System.Drawing.Size(buttonWidth, 25);
            int Left = (75 + oglMain.Width / 2) - (tool.numOfSections * size.Width) / 2;
            
            if (glm.isSimEnabled)
            {
                panelSim.Top = oglMain.Height - 40;
                panelSim.Left = 60 + oglMain.Width / 2 - 275;
            }

            //turn section buttons all On
            for (int j = 0; j < tool.sections.Count; j++)
            {
                tool.sections[j].button.Top = top;
                tool.sections[j].button.Left = Left + size.Width * j;
                tool.sections[j].button.Size = size;
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

            Settings.Default.setF_UserTotalArea = bnd.workedAreaTotalUser;

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
                                new FormHelp(gStr.h_lblSwapDirectionCancel, gStr.gsHelp).ShowDialog(this);
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
                                new FormHelp(gStr.h_lblManualTurnCancelTouch, gStr.gsHelp).ShowDialog(this);
                                ResetHelpBtn();
                                return;
                            }

                            if (gyd.isYouTurnTriggered)
                            {
                                gyd.ResetYouTurn();
                            }
                            else
                            {
                                gyd.BuildManualYouTurn(false);
                                return;
                            }
                        }

                        if (point.X > middle && point.X < middle + 140 && isUTurnOn)
                        {
                            if (isTT)
                            {
                                new FormHelp(gStr.h_lblManualTurnCancelTouch, gStr.gsHelp).ShowDialog(this);
                                ResetHelpBtn();
                                return;
                            }

                            if (gyd.isYouTurnTriggered)
                            {
                                gyd.ResetYouTurn();
                            }
                            else
                            {
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
                                new FormHelp(gStr.h_lblLateralTurnTouch, gStr.gsHelp).ShowDialog(this);
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
                                new FormHelp(gStr.h_lblLateralTurnTouch, gStr.gsHelp).ShowDialog(this);
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

                if (mc.headingTrueDual == double.MaxValue && point.X > centerLeft - 40 && point.X < centerLeft + 40 && point.Y > centerUp - 60 && point.Y < centerUp + 60)
                {
                    if (isTT)
                    {
                        new FormHelp(gStr.h_lblVehicleDirectionResetTouch, gStr.gsHelp).ShowDialog(this);    
                        ResetHelpBtn();
                        return;
                    }

                    Array.Clear(stepFixPts, 0, stepFixPts.Length);
                    isFirstHeadingSet = false;
                    isReverse = false;
                    this.TimedMessageBox(2000, "Reset Direction", "Drive Forward > 1.5 kmh");
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
                        new FormHelp(gStr.h_lblSteerCircleTouch, gStr.gsHelp).ShowDialog(this);
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

        public string SatsTracked { get { return Convert.ToString(mc.satellitesTracked); } }
        public string HDOP { get { return Convert.ToString(mc.hdop); } }
        public string GPSHeading { get { return glm.toDegrees(fixHeading).ToString("0.0") + "\u00B0"; } }

        public string FixQuality
        {
            get
            {
                if (glm.isSimEnabled)
                    return "Sim: ";
                else if (mc.fixQuality == 0) return "Invalid: ";
                else if (mc.fixQuality == 1) return "GPS single: ";
                else if (mc.fixQuality == 2) return "DGPS : ";
                else if (mc.fixQuality == 3) return "PPS : ";
                else if (mc.fixQuality == 4) return "RTK fix: ";
                else if (mc.fixQuality == 5) return "Float: ";
                else if (mc.fixQuality == 6) return "Estimate: ";
                else if (mc.fixQuality == 7) return "Man IP: ";
                else if (mc.fixQuality == 8) return "Sim: ";
                else return "Unknown: ";
            }
        }

        public string GyroInDegrees
        {
            get
            {
                if (mc.imuHeading != 99999)
                    return Math.Round(mc.imuHeading, 1) + "\u00B0";
                else return "-";
            }
        }
        public string RollInDegrees
        {
            get
            {
                if (mc.imuRoll != 88888)
                    return Math.Round((mc.imuRoll), 1) + "\u00B0";
                else return "-";
            }
        }
        public string SetSteerAngle { get { return ((double)(guidanceLineSteerAngle) * 0.01).ToString("0.0"); } }
        public string ActualSteerAngle { get { return ((mc.actualSteerAngleDegrees) ).ToString("0.0") ; } }

        public string FixOffset { get { return ((worldManager.fixOffset.easting * glm.mToUser).ToString("0") + ", " + (worldManager.fixOffset.northing * glm.mToUser).ToString("0")); } }

        #endregion properties 
    }//end class
}//end namespace