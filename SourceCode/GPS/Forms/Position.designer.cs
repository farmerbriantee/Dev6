//Please, if you use this, share the improvements

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        //very first fix to setup grid etc
        public bool isFirstFixPositionSet = false, isGPSPositionInitialized = false, isFirstHeadingSet = false,
            isReverse = false, isSuperSlow = false, isDriveIn = false;

        //string to record fixes for elevation maps
        public StringBuilder sbFix = new StringBuilder();

        // autosteer variables for sending serial
        public double guidanceLineDistanceOff = double.NaN, guidanceLineDistanceOffTool = double.NaN;

        public short guidanceLineSteerAngle;

        //guidance line look ahead
        public double guidanceLookAheadTime = 2;

        public vec2 pivotAxlePos = new vec2(0, 0);
        public vec2 steerAxlePos = new vec2(0, 0);
        public vec2 hitchPos = new vec2(0, 0);

        //history
        public vec2 prevSectionPos = new vec2(0, 0);

        //headings
        public double fixHeading = 0.0, sinH, cosH;

        public double FixHeading
        {
            set
            {
                fixHeading = value;

                if (fixHeading < 0) fixHeading += glm.twoPI;
                else if (fixHeading > glm.twoPI) fixHeading -= glm.twoPI;

                cosH = Math.Cos(fixHeading);
                sinH = Math.Sin(fixHeading);
            }
        }



        //how far travelled since last section was added, section points
        public double sectionTriggerStepDistance = 0;

        //Everything is so wonky at the start
        int startCounter = 0;

        //individual points for the flags in a list
        public List<CFlag> flagPts = new List<CFlag>();
        
        public double previousSpeed;//for average speed
        public int crossTrackError;

        //youturn
        public double distancePivotToTurnLine = -4444;

        //IMU 
        public double rollCorrectionDistance = 0;
        public double imuGPS_Offset, imuCorrected;

        //step position - slow speed spinner killer
        private const int totalFixSteps = 20;
        public vecFix2Fix[] stepFixPts = new vecFix2Fix[totalFixSteps];
        public double minFixStepDist = 1, startSpeed = 0.5;

        public bool isRTK, isRTK_KillAutosteer;

        public double uncorrectedEastingGraph = 0;
        public double correctionDistanceGraph = 0;

        public void UpdateFixPosition()
        {
            //Measure the frequency of the GPS updates
            rawHz = ((double)System.Diagnostics.Stopwatch.Frequency) / (double)swHz.ElapsedTicks;
            //start the watch and time till it finishes
            swHz.Reset();
            swHz.Start();

            if (rawHz > 100) rawHz = 100;
            if (rawHz < 0.5) rawHz = 0.5;
            //simple comp filter
            HzTime = 0.97 * HzTime + 0.03 * rawHz;

            if (glm.isSimEnabled && Debugger.IsAttached)
                HzTime = 10;

            startCounter++;
            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            if (!isGPSPositionInitialized)
            {
                InitializeFirstFewGPSPositions();
                return;
            }
            vec2 oldpivotAxlePos = new vec2(pivotAxlePos.easting, pivotAxlePos.northing);

            if (vehicleGPSWatchdog < 11)
            {
                //calculate current heading only when moving, otherwise use last
                if (mc.headingTrueDual == double.MaxValue && !isFirstHeadingSet) //set in steer settings, Stanley
                {
                    if (Math.Abs(mc.avgSpeed) >= 1.5)
                    {
                        if (stepFixPts[2].isSet == 0)
                        {
                            //this is the first position no roll or offset correction
                            if (stepFixPts[0].isSet == 0)
                            {
                                stepFixPts[0].easting = mc.fix.easting;
                                stepFixPts[0].northing = mc.fix.northing;
                                stepFixPts[0].isSet = 1;
                                return;
                            }

                            //and the second
                            if (stepFixPts[1].isSet == 0)
                            {
                                for (int i = totalFixSteps - 1; i > 0; i--) stepFixPts[i] = stepFixPts[i - 1];
                                stepFixPts[0].easting = mc.fix.easting;
                                stepFixPts[0].northing = mc.fix.northing;
                                stepFixPts[0].isSet = 1;
                                return;
                            }

                            //the critcal moment for checking initial direction/heading.
                            for (int i = totalFixSteps - 1; i > 0; i--) stepFixPts[i] = stepFixPts[i - 1];
                            stepFixPts[0].easting = mc.fix.easting;
                            stepFixPts[0].northing = mc.fix.northing;
                            stepFixPts[0].isSet = 1;

                            FixHeading = Math.Atan2(mc.fix.easting - stepFixPts[2].easting, mc.fix.northing - stepFixPts[2].northing);

                            //now we have a heading, fix the first 3
                            if (vehicle.antennaOffset != 0)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    stepFixPts[i].easting -= cosH * vehicle.antennaOffset;
                                    stepFixPts[i].northing += sinH * vehicle.antennaOffset;
                                }
                            }

                            if (mc.imuRoll != 88888)
                            {
                                //change for roll to the right is positive times -1
                                rollCorrectionDistance = Math.Tan(glm.toRadians((mc.imuRoll))) * vehicle.antennaHeight;

                                for (int i = 0; i < 3; i++)
                                {
                                    stepFixPts[i].easting -= cosH * rollCorrectionDistance;
                                    stepFixPts[i].northing += sinH * rollCorrectionDistance;
                                }
                            }

                            //get the distance from first to 2nd point, update fix with new offset/roll
                            stepFixPts[0].distance = glm.Distance(stepFixPts[1], stepFixPts[0]);
                            mc.fix.easting = stepFixPts[0].easting;
                            mc.fix.northing = stepFixPts[0].northing;

                            isFirstHeadingSet = true;
                            this.TimedMessageBox(2000, "Direction Reset", "Forward is Set");
                        }
                    }
                    else
                    {
                        double distance = glm.Distance(oldpivotAxlePos, mc.fix);
                        pivotAxlePos = mc.fix;
                        mc.avgSpeed = (mc.avgSpeed * 0.8) + (distance * 3.6 * 0.2) * HzTime;

                        return;
                    }
                }
                else
                {
                    if (mc.headingTrueDual != double.MaxValue)
                    {
                        isFirstHeadingSet = true;
                        FixHeading = glm.toRadians(mc.headingTrueDual);
                    }

                    if (vehicle.antennaOffset != 0)
                    {
                        mc.fix.easting -= cosH * vehicle.antennaOffset;
                        mc.fix.northing += sinH * vehicle.antennaOffset;
                    }

                    uncorrectedEastingGraph = mc.fix.easting;

                    //originalEasting = pn.fix.easting;
                    if (mc.imuRoll != 88888 && vehicle.antennaHeight > 0)
                    {
                        //change for roll to the right is positive times -1
                        rollCorrectionDistance = Math.Sin(glm.toRadians((mc.imuRoll))) * vehicle.antennaHeight;
                        correctionDistanceGraph = rollCorrectionDistance;

                        mc.fix.easting -= cosH * rollCorrectionDistance;
                        mc.fix.northing += sinH * rollCorrectionDistance;
                    }

                    //initializing all done
                    if (mc.headingTrueDual != double.MaxValue || Math.Abs(mc.avgSpeed) > startSpeed)
                    {
                        isSuperSlow = false;

                        //save current fix and distance and set as valid
                        for (int i = totalFixSteps - 1; i > 0; i--) stepFixPts[i] = stepFixPts[i - 1];
                        stepFixPts[0].easting = mc.fix.easting;
                        stepFixPts[0].northing = mc.fix.northing;
                        stepFixPts[0].distance = glm.Distance(stepFixPts[0], stepFixPts[1]);
                        stepFixPts[0].isSet = 1;

                        if (stepFixPts[3].isSet == 1)
                        {
                            //find back the fix to fix distance, then heading
                            double dist = 0;
                            for (int i = 1; i < totalFixSteps; i++)
                            {
                                if (stepFixPts[i].isSet == 0)
                                {
                                    break;
                                }
                                dist += stepFixPts[i - 1].distance;

                                if (dist > (mc.headingTrueDual == double.MaxValue ? minFixStepDist : 0.3))
                                {
                                    //most recent heading
                                    double newHeading = Math.Atan2(mc.fix.easting - stepFixPts[i].easting,
                                                            mc.fix.northing - stepFixPts[i].northing);
                                    if (newHeading < 0) newHeading += glm.twoPI;

                                    //what is angle between the last valid heading before stopping and one just now
                                    double delta = Math.Abs(Math.PI - Math.Abs(Math.Abs(newHeading - fixHeading) - Math.PI));

                                    //ie change in direction
                                    if (mc.isReverseOn && delta > glm.PIBy2)
                                    {
                                        isReverse = true;
                                        newHeading += Math.PI;
                                        if (newHeading < 0) newHeading += glm.twoPI;
                                        else if (newHeading >= glm.twoPI) newHeading -= glm.twoPI;
                                    }
                                    else
                                        isReverse = false;

                                    if (mc.headingTrueDual == double.MaxValue)
                                    {
                                        FixHeading = newHeading - glm.toRadians(vehicle.antennaPivot / 1
                                                * mc.actualSteerAngleDegrees * (isReverse ? mc.reverseComp : mc.forwardComp));

                                        // IMU Fusion with heading correction, add the correction
                                        if (mc.imuHeading != 99999)
                                        {
                                            //current gyro angle in radians
                                            double imuHeading = (glm.toRadians(mc.imuHeading));

                                            //Difference between the IMU heading and the GPS heading
                                            double gyroDelta = (imuHeading + imuGPS_Offset) - fixHeading;
                                            //double gyroDelta = Math.Abs(Math.PI - Math.Abs(Math.Abs(imuHeading + gyroCorrection) - gpsHeading) - Math.PI);

                                            if (gyroDelta < 0) gyroDelta += glm.twoPI;
                                            else if (gyroDelta > glm.twoPI) gyroDelta -= glm.twoPI;

                                            //calculate delta based on circular data problem 0 to 360 to 0, clamp to +- 2 Pi
                                            if (gyroDelta >= -glm.PIBy2 && gyroDelta <= glm.PIBy2) gyroDelta *= -1.0;
                                            else
                                            {
                                                if (gyroDelta > glm.PIBy2) { gyroDelta = glm.twoPI - gyroDelta; }
                                                else { gyroDelta = (glm.twoPI + gyroDelta) * -1.0; }
                                            }
                                            if (gyroDelta > glm.twoPI) gyroDelta -= glm.twoPI;
                                            else if (gyroDelta < -glm.twoPI) gyroDelta += glm.twoPI;

                                            if (Math.Abs(mc.avgSpeed) > startSpeed)
                                            {
                                                if (isReverse)
                                                    imuGPS_Offset += (gyroDelta * (0.01));
                                                else
                                                    imuGPS_Offset += (gyroDelta * (mc.fusionWeight));
                                            }
                                            else
                                            {
                                                imuGPS_Offset += (gyroDelta * (0.2));
                                            }

                                            if (imuGPS_Offset > glm.twoPI) imuGPS_Offset -= glm.twoPI;
                                            else if (imuGPS_Offset < 0) imuGPS_Offset += glm.twoPI;

                                            //determine the Corrected heading based on gyro and GPS
                                            imuCorrected = imuHeading + imuGPS_Offset;

                                            if (imuCorrected < 0) imuCorrected += glm.twoPI;
                                            else if (imuCorrected > glm.twoPI) imuCorrected -= glm.twoPI;

                                            //use imu as heading when going slow
                                            fixHeading = imuCorrected;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    //slow speed
                    else
                    {
                        isSuperSlow = true;

                        if (stepFixPts[0].isSet == 1)
                        {
                            //going back and forth clear out the array so only 1 value
                            for (int i = 0; i < stepFixPts.Length; i++)
                            {
                                stepFixPts[i].easting = 0;
                                stepFixPts[i].northing = 0;
                                stepFixPts[i].isSet = 0;
                                stepFixPts[i].distance = 0;
                            }
                        }
                    }
                }

                worldManager.SmoothCam(fixHeading);

                //positions and headings 
                CalculatePositionHeading();
            }
            else if (toolGPSWatchdog < 11)
            {
                //for TEST DUAL only
                FixHeading = tool.toolHeading = glm.toRadians(mc.headingTrueDualTool);

                tool.WorkPos = new vec2(mc.fixTool.easting, mc.fixTool.northing);

                //offset based on settings
                if (tool.AntennaOffset != 0)
                {
                    tool.WorkPos.easting -= cosH * tool.AntennaOffset;
                    tool.WorkPos.northing += sinH * tool.AntennaOffset;
                }

                if (mc.imuRollTool != 88888 && tool.AntennaHeight != 0)
                {
                    //change for roll to the right is positive times -1
                    rollCorrectionDistance = Math.Sin(glm.toRadians((mc.imuRollTool))) * tool.AntennaHeight;

                    tool.WorkPos.easting -= cosH * rollCorrectionDistance;
                    tool.WorkPos.northing += sinH * rollCorrectionDistance;
                }

                steerAxlePos = pivotAxlePos = new vec2(tool.WorkPos.easting, tool.WorkPos.northing);

                tool.toolSteerShift = 0;

                worldManager.SmoothCam(tool.toolHeading);
            }
            else
            {
                return;
            }

            double distanceCurrentStepFix = glm.Distance(oldpivotAxlePos, pivotAxlePos);

            if (mc.speed == double.MaxValue)
            {
                mc.avgSpeed = (mc.avgSpeed * 0.8) + (distanceCurrentStepFix * 3.6 * 0.2) * HzTime;
            }
            else
            {
                mc.avgSpeed = (mc.avgSpeed * 0.5) + (mc.speed * 0.5);
                mc.speed = double.MaxValue;
            }

            //calc distance travelled since last GPS fix
            if (mc.avgSpeed > 1)
            {
                if ((bnd.distanceUser += distanceCurrentStepFix) > 3000) bnd.distanceUser = 0;//userDistance can be reset
            }

            //calculate lookahead at full speed, no sentence misses
            tool.CalculateSectionLookAhead();

            //To prevent drawing high numbers of triangles, determine and test before drawing vertex
            double sectionTriggerDistance = glm.Distance(pivotAxlePos, prevSectionPos);

            //test if travelled far enough for new boundary point
            if (bnd.isOkToAddPoints && (bnd.bndBeingMadePts.points.Count == 0 || glm.Distance(pivotAxlePos, bnd.bndBeingMadePts.points[bnd.bndBeingMadePts.points.Count - 1]) > 1))
            {
                AddBoundaryPoint();
            }

            if (mc.panicStopSpeed > 0 && (previousSpeed - mc.avgSpeed) > mc.panicStopSpeed)
            {
                setBtnAutoSteer(false);
            }
            previousSpeed = mc.avgSpeed;

            #region AutoSteer

            //reset the values
            guidanceLineDistanceOffTool = guidanceLineDistanceOff = double.NaN;

            gyd.GetCurrentGuidanceLine(pivotAxlePos, steerAxlePos, fixHeading);

            byte value = 0x00;
            if (isAutoSteerBtnOn && !double.IsNaN(guidanceLineDistanceOff)) value |= 0x01;
            if (gyd.isYouTurnTriggered) value |= 0x02;
            if (gyd.isYouTurnRight) value |= 0x04;

            if (mc.isOutOfBounds) value |= 0x20;
            if (vehicle.isInFreeDriveMode) value |= 0x40;
            if (vehicle.isInFreeToolDriveMode) value |= 0x80;

            p_254.pgn[p_254.status] = value;
            p_233.pgn[p_233.status] = value;

            //convert to cm from mm and divide by 2 - lightbar
            int distanceX2;

            if (double.IsNaN(guidanceLineDistanceOff))
                distanceX2 = 255;
            else
            {
                distanceX2 = (int)(guidanceLineDistanceOff * 50);

                if (distanceX2 < -127) distanceX2 = -127;
                else if (distanceX2 > 127) distanceX2 = 127;
                distanceX2 += 127;
            }

            p_254.pgn[p_254.lineDistance] = unchecked((byte)distanceX2);
            p_254.pgn[p_254.speedHi] = unchecked((byte)((int)(Math.Abs(mc.avgSpeed) * 10.0) >> 8));
            p_254.pgn[p_254.speedLo] = unchecked((byte)((int)(Math.Abs(mc.avgSpeed) * 10.0)));

            if (vehicle.isInFreeDriveMode) //Drive button is on
            {
                guidanceLineSteerAngle = (short)(vehicle.driveFreeSteerAngle * 100);
            }
            p_254.pgn[p_254.steerAngleHi] = unchecked((byte)(guidanceLineSteerAngle >> 8));
            p_254.pgn[p_254.steerAngleLo] = unchecked((byte)(guidanceLineSteerAngle));

            //speed for tool
            p_233.pgn[p_233.speed] = unchecked((byte)(Math.Abs(mc.avgSpeed) * 10.0));

            short toolxte;
            if (vehicle.isInFreeToolDriveMode)
            {
                toolxte = (short)(vehicle.driveFreeToolDistance * 100);
            }
            else
            {
                toolxte = (short)Math.Round(guidanceLineDistanceOffTool * 1000.0, MidpointRounding.AwayFromZero);
            }

            //Tool XTE
            p_233.pgn[p_233.highXTE] = unchecked((byte)(toolxte >> 8));
            p_233.pgn[p_233.lowXTE] = unchecked((byte)(toolxte));

            //Vehicle XTE       Convert from meters to millimeters
            short vehiclexte = (short)Math.Round(guidanceLineDistanceOff * 1000.0, MidpointRounding.AwayFromZero);
            p_233.pgn[p_233.highVehXTE] = unchecked((byte)(vehiclexte >> 8));
            p_233.pgn[p_233.lowVehXTE] = unchecked((byte)(vehiclexte));

            //out serial to autosteer module  //indivdual classes load the distance and heading deltas 
            if (vehicleGPSWatchdog < 11)
                SendPgnToLoop(p_254.pgn);

            if (tool.isSteering)
                SendPgnToLoop(p_233.pgn);

            //for average cross track error
            if (!double.IsNaN(guidanceLineDistanceOff))
            {
                crossTrackError = (int)((double)crossTrackError * 0.90 + Math.Abs((double)guidanceLineDistanceOff) * 0.1);
            }

            #endregion

            #region Youturn

            //reset the fault distance to an appropriate weird number
            // -4444 means cross trac error too high
            distancePivotToTurnLine = -4444;

            //always force out of bounds and change only if in bounds after proven so
            mc.isOutOfBounds = true;

            //if an outer boundary is set, then apply critical stop logic
            if (bnd.bndList.Count > 0)
            {
                //Are we inside outer and outside inner all turn boundaries, no turn creation problems
                if (bnd.IsPointInsideFenceArea(pivotAxlePos) && !gyd.isTurnCreationTooClose && !gyd.isTurnCreationNotCrossingError)
                {
                    lastSecondInField = secondsSinceStart;
                    //reset critical stop for bounds violation
                    mc.isOutOfBounds = false;

                    //do the auto youturn logic if everything is on.
                    if (gyd.isYouTurnBtnOn && isAutoSteerBtnOn && !gyd.isYouTurnTriggered && vehicleGPSWatchdog < 11)
                    {
                        //if we are too much off track > 1.3m, kill the diagnostic creation, start again
                        if (crossTrackError > 1.3)
                            gyd.ResetCreatedYouTurn();
                        else
                        {
                            //now check to make sure we are not in an inner turn boundary - drive thru is ok
                            if (gyd.youTurnPhase < 0)
                                gyd.youTurnPhase++;
                            else if (gyd.youTurnPhase < 254)
                                gyd.FindTurnPoints(pivotAxlePos);
                            else if (gyd.youTurnPhase == 254)
                            {
                                gyd.ytList.points.SmoothAB(gyd.uTurnSmoothing);
                                gyd.youTurnPhase = 255;
                            }
                            else if (gyd.ytList.points.Count > 1) //wait to trigger the actual turn since its made and waiting
                            {
                                //distance from current pivot to first point of youturn pattern
                                distancePivotToTurnLine = pivotAxlePos.FindDistanceToSegment(gyd.ytList.points[0], gyd.ytList.points[1], out double time);

                                if (distancePivotToTurnLine < 20.0 && distancePivotToTurnLine >= 18.0 && !sounds.isBoundAlarming)
                                {
                                    if (sounds.isTurnSoundOn) sounds.sndBoundaryAlarm.Play();
                                    sounds.isBoundAlarming = true;
                                }
                                //if we are close enough to pattern, trigger.
                                if (distancePivotToTurnLine <= 10.0 && time >= 0)
                                {
                                    gyd.YouTurnTrigger();
                                    sounds.isBoundAlarming = false;
                                }
                            }
                            else gyd.TurnCreationWidthError = true;
                        }
                    } // end of isInWorkingArea
                }
                // here is stop logic for out of bounds - in an inner or out the outer turn border.
                else
                {
                    mc.isOutOfBounds = true;
                    if (gyd.isYouTurnBtnOn)
                    {
                        gyd.ResetCreatedYouTurn();
                        sim.stepDistance = 0;
                    }
                }
            }
            else
            {
                mc.isOutOfBounds = false;
            }

            if (isDriveIn && secondsSinceStart - lastSecondInField >= 5.0)
            {
                lastSecondInField = secondsSinceStart - 3;
                /*
                for (int i = 0; i < Fields.Count; i++)
                {
                    if (Fields[i].Eastingmin <= pivotAxlePos.easting && Fields[i].Eastingmax >= pivotAxlePos.easting && Fields[i].Northingmin <= pivotAxlePos.northing && Fields[i].Northingmax >= pivotAxlePos.northing)
                    {
                        if (Fields[i].points.IsPointInPolygon(pivotAxlePos) && (!isJobStarted || currentFieldDirectory != Fields[i].Dir))
                        {
                            lastSecondInField = secondsSinceStart;
                            CloseCurrentJob(false, 1);
                            currentFieldDirectory = Fields[i].Dir;
                            FileOpenField();
                            break;
                        }
                    }
                }
                */
            }

            #endregion

            //draw the section control window off screen buffer
            if (isJobStarted && (tool.isFastSections || bbCounter++ > 0))
            {
                bbCounter = 0;
                oglBack.Refresh();
                SendPgnToLoop(p_239.pgn);

                //Determine if sections want to be on or off
                tool.ProcessSectionOnOffRequests();
            }

            //section on off and points, contour points
            if (sectionTriggerDistance > sectionTriggerStepDistance && isJobStarted)
            {
                AddSectionOrContourPathPoints();

                //grab fix and elevation
                if (isLogElevation) sbFix.Append(mc.fix.easting.ToString("0.00") + "," + mc.fix.northing.ToString("0.00") + ","
                                                    + mc.altitude.ToString("0.00") + ","
                                                    + mc.latitude + "," + mc.longitude + "\r\n");
            }

            //Checks the workswitch if required
            mc.CheckWorkAndSteerSwitch();

            //send the byte out to section machines
            BuildMachineByte();

            //update main window
            oglMain.MakeCurrent();
            oglMain.Refresh();

            if (isAutoDayNight && dayNightCounter > 600)//10 minutes
            {
                dayNightCounter = 1;

                if (sunrise.Date != DateTime.Today)
                {
                    IsBetweenSunriseSunset(mc.latitude, mc.longitude);
                }

                isDayTime = (DateTime.Now.Ticks < sunset.Ticks && DateTime.Now.Ticks > sunrise.Ticks);

                if (isDayTime != isDay)
                {
                    isDay = !isDayTime;
                    SwapDayNightMode();
                }
            }

            //if a minute has elapsed save the field in case of crash and to be able to resume            
            if (isJobStarted && secondsCounter > 30 && sentenceCounter < 20)
            {
                secondsCounter = 1;
                tmrWatchdog.Enabled = false;

                //don't save if no gps
                if (isJobStarted)
                {
                    //auto save the field patches, contours accumulated so far
                    FileSaveSections();
                    FileSaveContour();

                    //NMEA log file
                    if (isLogElevation) FileSaveElevation();
                    //FileSaveFieldKML();
                }
                //go see if data ready for draw and position updates
                tmrWatchdog.Enabled = true;
            }

            //stop the timer and calc how long it took to do calcs and draw
            double frameTimeRough = (double)(swHz.ElapsedTicks * 1000.0) / (double)System.Diagnostics.Stopwatch.Frequency;

            if (frameTimeRough > 50) frameTimeRough = 50;
            frameTime = frameTime * 0.9 + frameTimeRough * 0.1;

            mc.headingTrueDual = double.MaxValue;
        }

        //all the hitch, pivot, section, trailing hitch, headings and fixes
        private void CalculatePositionHeading()
        {
            #region pivot hitch trail
            
            pivotAxlePos.easting = mc.fix.easting - (sinH * vehicle.antennaPivot);
            pivotAxlePos.northing = mc.fix.northing - (cosH * vehicle.antennaPivot);

            steerAxlePos.easting = pivotAxlePos.easting + (sinH * vehicle.wheelbase);
            steerAxlePos.northing = pivotAxlePos.northing + (cosH * vehicle.wheelbase);

            vec2 oldhitchPos = new vec2(hitchPos.easting, hitchPos.northing);

            //determine where the rigid vehicle hitch ends
            hitchPos.easting = pivotAxlePos.easting + (sinH * tool.hitchLength);
            hitchPos.northing = pivotAxlePos.northing + (cosH * tool.hitchLength);
            double moveDist = glm.Distance(oldhitchPos, hitchPos);

            tool.SetToolPosition(hitchPos, fixHeading, moveDist);
            #endregion
        }

        //perimeter and boundary point generation
        public void AddBoundaryPoint()
        {
            bnd.bndBeingMadePts.ResetPoints = true;
            //build the boundary line
            bnd.bndBeingMadePts.points.Add(new vec2(
                pivotAxlePos.easting + (cosH * (bnd.isDrawRightSide ? bnd.createBndOffset : -bnd.createBndOffset)),
                pivotAxlePos.northing - (sinH * (bnd.isDrawRightSide ? bnd.createBndOffset : -bnd.createBndOffset))));
        }

        //add the points for section, contour line points, Area Calc feature
        private void AddSectionOrContourPathPoints()
        {
            if (gyd.isOkToAddDesPoints && gyd.EditGuidanceLine != null)
            {
                if (gyd.EditGuidanceLine is CGuidanceRecPath aa)
                    aa.Status.Add(new CRecPathPt(mc.avgSpeed < 1.0 ? 1.0 : mc.avgSpeed, autoBtnState));

                gyd.EditGuidanceLine.points.Add(new vec2(pivotAxlePos.easting, pivotAxlePos.northing));
            }

            //save the north & east as previous
            prevSectionPos.northing = pivotAxlePos.northing;
            prevSectionPos.easting = pivotAxlePos.easting;

            // if non zero, at least one section is on.
            int sectionCounter = 0;

            //send the current and previous GPS fore/aft corrected fix to each section
            for (int j = 0; j < tool.sections.Count; j++)
            {
                if (tool.sections[j].isMappingOn)
                {
                    tool.sections[j].AddMappingPoint();
                    sectionCounter++;
                }
            }
            if (sectionCounter > 0 && (!isAutoSteerBtnOn || gyd.CurrentGMode == Mode.Contour))
                gyd.AddPoint(pivotAxlePos);
            else
                gyd.StopContourLine();
        }

        //the start of first few frames to initialize entire program
        private void InitializeFirstFewGPSPositions()
        {
            if (!isFirstFixPositionSet)
            {
                if (!isJobStarted)
                {
                    if (glm.isSimEnabled)
                    {
                        worldManager.latStart = Properties.Settings.Default.setGPS_SimLatitude;
                        worldManager.lonStart = Properties.Settings.Default.setGPS_SimLongitude;
                        sim.resetSim();
                    }
                    else
                    {
                        worldManager.latStart = mc.latitude;
                        worldManager.lonStart = mc.longitude;
                    }
                    worldManager.SetLocalMetersPerDegree();
                }

                worldManager.ConvertWGS84ToLocal(mc.latitude, mc.longitude, out mc.fix.northing, out mc.fix.easting);

                //Draw a grid once we know where in the world we are.
                isFirstFixPositionSet = true;
            }
            else
            {
                //keep here till valid data
                if (startCounter > (20))
                {
                    isGPSPositionInitialized = true;
                    dayNightCounter = 601;
                }
                else
                    dayNightCounter = 1;

                FixHeading = tool.toolHeading = 0;
            }
        }
    }//end class
}//end namespace