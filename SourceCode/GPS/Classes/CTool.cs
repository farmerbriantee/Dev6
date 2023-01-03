using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace AgOpenGPS
{
    public class CTool
    {
        private readonly FormGPS mf;

        //maximum sections available
        public const int MAXSECTIONS = 50;

        /// <summary>
        /// an array of sections, so far 16 section + 1 fullWidth Section
        /// </summary>
        public List<CSection> sections = new List<CSection>();

        public double toolWidth;
        public double toolFarLeftPosition = 0, toolFarRightPosition = 0;
        public double toolFarLeftSpeed = 0, toolFarRightSpeed = 0;

        //points in world space that start and end of section are in
        public vec2 leftPoint, rightPoint, axlePos, tankAxle;
        public vec2 WorkPos, tankHitch;
        public double tankHeading, toolHeading;

        public double toolOverlap, toolOffset;
        public double TrailingHitchLength, TankHitchLength;
        public double TrailingAxleLength, TankAxleLength;

        public bool isSteering;
        public double AntennaOffset, AntennaHeight;

        public double lookAheadMin, lookAheadMax;

        public double lookAheadOffSetting, lookAheadOnSetting;
        public double turnOffDelay, mappingOnDelay, mappingOffDelay;

        public double lookAheadDistanceOnPixelsLeft, lookAheadDistanceOnPixelsRight;
        public double lookAheadDistanceOffPixelsLeft, lookAheadDistanceOffPixelsRight;
        public double lookAheadBoundaryOnPixelsLeft, lookAheadBoundaryOnPixelsRight;
        public double lookAheadBoundaryOffPixelsLeft, lookAheadBoundaryOffPixelsRight;

        public bool isToolTrailing, isToolTBT;
        public bool isToolRearFixed, isToolFrontFixed;

        public bool isMultiColoredSections, superSectionOnRequest, isFastSections = false;

        public double hitchLength, toolSteerShift;

        public int numOfSections, maxOverlap;
        public double boundOverlap = 0.0, slowSpeedCutoff = 0;

        //storage for the cos and sin of heading
        public double cosSectionHeading = 1.0, sinSectionHeading = 0.0;

        //read pixel values
        public int rpXPosition, rpWidth;

        private int _ToolVBO;
        public bool updateVBO = true;

        public Color[] secColors = new Color[16];

        //Constructor called by FormGPS
        public CTool(FormGPS _f)
        {
            mf = _f;
            sections.Add(new CSection(mf, -1));
        }

        public void LoadSettings()
        {
            secColors[0] = Properties.Settings.Default.setColor_sec01;
            secColors[1] = Properties.Settings.Default.setColor_sec02;
            secColors[2] = Properties.Settings.Default.setColor_sec03;
            secColors[3] = Properties.Settings.Default.setColor_sec04;
            secColors[4] = Properties.Settings.Default.setColor_sec05;
            secColors[5] = Properties.Settings.Default.setColor_sec06;
            secColors[6] = Properties.Settings.Default.setColor_sec07;
            secColors[7] = Properties.Settings.Default.setColor_sec08;
            secColors[8] = Properties.Settings.Default.setColor_sec09;
            secColors[9] = Properties.Settings.Default.setColor_sec10;
            secColors[10] = Properties.Settings.Default.setColor_sec11;
            secColors[11] = Properties.Settings.Default.setColor_sec12;
            secColors[12] = Properties.Settings.Default.setColor_sec13;
            secColors[13] = Properties.Settings.Default.setColor_sec14;
            secColors[14] = Properties.Settings.Default.setColor_sec15;
            secColors[15] = Properties.Settings.Default.setColor_sec16;
            

            isSteering = Properties.Vehicle.Default.Tool_isTooLSteering;
            AntennaHeight = Properties.Vehicle.Default.Tool_antennaHeight;
            AntennaOffset = Properties.Vehicle.Default.Tool_antennaOffset;


            //from settings grab the vehicle specifics
            toolWidth = Properties.Vehicle.Default.Tool_Width;
            toolOverlap = Properties.Vehicle.Default.Tool_Overlap;
            toolOffset = Properties.Vehicle.Default.Tool_Offset;

            hitchLength = Properties.Vehicle.Default.setVehicle_hitchLength;
            TankAxleLength = Properties.Vehicle.Default.Tool_TankTrailingAxleLength;
            TankHitchLength = Properties.Vehicle.Default.Tool_TankTrailingHitchLength;
            TrailingAxleLength = Properties.Vehicle.Default.Tool_TrailingAxleLength;
            TrailingHitchLength = Properties.Vehicle.Default.Tool_TrailingHitchLength;

            isToolRearFixed = Properties.Vehicle.Default.Tool_isToolRearFixed;
            isToolTrailing = Properties.Vehicle.Default.Tool_isTrailing;
            isToolTBT = Properties.Vehicle.Default.Tool_isTBT;

            mappingOnDelay = lookAheadOnSetting = Properties.Vehicle.Default.Tool_LookAheadOn;
            mappingOffDelay = lookAheadOffSetting = Properties.Vehicle.Default.Tool_LookAheadOff;
            turnOffDelay = Properties.Vehicle.Default.Tool_OffDelay;

            numOfSections = Properties.Vehicle.Default.Tool_numSections;

            maxOverlap = Properties.Vehicle.Default.Tool_minCoverage;
            boundOverlap = Properties.Vehicle.Default.setVehicle_bndOverlap;
            isMultiColoredSections = Properties.Settings.Default.setColor_isMultiColorSections;

            slowSpeedCutoff = Properties.Vehicle.Default.Tool_slowSpeedCutoff;

            //fast or slow section update
            isFastSections = Properties.Vehicle.Default.Tool_Section_isFast;

            SectionSetPosition();
        }

        public void ResetTool()
        {
            toolHeading = tankHeading = mf.fixHeading;

            double sinH = mf.sinH;
            double cosH = mf.cosH;

            tankAxle.easting = mf.hitchPos.easting + (sinH * TankAxleLength);
            tankAxle.northing = mf.hitchPos.northing + (cosH * TankAxleLength);
            tankHitch.easting = tankAxle.easting + (sinH * TankHitchLength);
            tankHitch.northing = tankAxle.northing + (cosH * TankHitchLength);

            axlePos.easting = tankHitch.easting + (sinH * TrailingAxleLength);
            axlePos.northing = tankHitch.northing + (cosH * TrailingAxleLength);
            WorkPos.easting = axlePos.easting + (sinH * TrailingHitchLength);
            WorkPos.northing = axlePos.northing + (cosH * TrailingHitchLength);
        }

        public void SetToolPosition(vec2 hitchPos, double fixHeading, double distance)
        {
            int startCounter = 60;
            //tool attached via a trailing hitch
            if (isToolTrailing)
            {
                double over;
                double distancetoolTravel = distance;

                if (isToolTBT)
                {
                    //Torriem rules!!!!! Oh yes, this is all his. Thank-you
                    if (distance != 0)
                    {
                        double t = (TankAxleLength) / distance;
                        tankAxle.easting = hitchPos.easting + t * (hitchPos.easting - tankAxle.easting);
                        tankAxle.northing = hitchPos.northing + t * (hitchPos.northing - tankAxle.northing);
                        tankHeading = Math.Atan2(hitchPos.easting - tankAxle.easting, hitchPos.northing - tankAxle.northing);
                    }

                    ////the tool is seriously jacknifed or just starting out so just spring it back.
                    over = Math.Abs(Math.PI - Math.Abs(Math.Abs(tankHeading - fixHeading) - Math.PI));

                    //criteria for a forced reset to put tool directly behind vehicle
                    if (over > 2.0 || startCounter < 51)
                    {
                        tankHeading = fixHeading;
                    }

                    double sin2 = Math.Sin(tankHeading);
                    double cos2 = Math.Cos(tankHeading);

                    tankAxle.easting = hitchPos.easting + (sin2 * TankAxleLength);
                    tankAxle.northing = hitchPos.northing + (cos2 * TankAxleLength);

                    vec2 oldtankHitch = new vec2(tankHitch.easting, tankHitch.northing);
                    tankHitch.easting = tankAxle.easting + (sin2 * TankHitchLength);
                    tankHitch.northing = tankAxle.northing + (cos2 * TankHitchLength);
                    distancetoolTravel = glm.Distance(tankHitch, oldtankHitch);
                }
                else
                {
                    tankHeading = fixHeading;
                    tankHitch.easting = tankAxle.easting = hitchPos.easting;
                    tankHitch.northing = tankAxle.northing = hitchPos.northing;
                }

                if (mf.toolGPSWatchdog < 11)
                {
                    WorkPos.easting = mf.mc.fixTool.easting;
                    WorkPos.northing = mf.mc.fixTool.northing;

                    double newHeading = Math.Atan2(tankHitch.easting - WorkPos.easting, tankHitch.northing - WorkPos.northing);
                    double hypotenuse = glm.Distance(tankHitch, WorkPos);

                    //offset based on settings
                    if (AntennaOffset != 0)
                    {
                        newHeading += Math.Sinh(AntennaOffset / hypotenuse);
                    }

                    if (mf.mc.imuRollTool != 88888 && AntennaHeight != 0)
                    {
                        //change for roll to the right is positive times -1
                        double rollCorrectionDistance = Math.Sin(glm.toRadians((mf.mc.imuRollTool))) * AntennaHeight;
                        newHeading += Math.Sinh(rollCorrectionDistance / hypotenuse);
                    }

                    toolHeading = newHeading;
                    if (toolHeading < 0) toolHeading += glm.twoPI;
                }
                else
                {
                    if (isSteering)
                    {
                        toolOffset = mf.mc.toolActualDistance;
                        updateVBO = true;
                    }
                    else
                    {
                        if (toolOffset != 0)
                            updateVBO = true;
                        toolOffset = 0;
                    }

                    //Torriem rules!!!!! Oh yes, this is all his. Thank-you
                    if (distancetoolTravel != 0)
                    {
                        double t = (TrailingAxleLength) / distancetoolTravel;
                        axlePos.easting = tankHitch.easting + t * (tankHitch.easting - axlePos.easting);
                        axlePos.northing = tankHitch.northing + t * (tankHitch.northing - axlePos.northing);
                        toolHeading = Math.Atan2(tankHitch.easting - axlePos.easting, tankHitch.northing - axlePos.northing);
                    }

                    ////the tool is seriously jacknifed or just starting out so just spring it back.
                    over = Math.Abs(Math.PI - Math.Abs(Math.Abs(toolHeading - tankHeading) - Math.PI));

                    //criteria for a forced reset to put tool directly behind vehicle
                    if (over > 2.0 || startCounter < 51)
                    {
                        toolHeading = tankHeading;
                    }
                }

                double sin = Math.Sin(toolHeading);
                double cos = Math.Cos(toolHeading);

                axlePos.easting = tankHitch.easting + (sin * TrailingAxleLength);
                axlePos.northing = tankHitch.northing + (cos * TrailingAxleLength);

                WorkPos.easting = axlePos.easting + (sin * TrailingHitchLength);
                WorkPos.northing = axlePos.northing + (cos * TrailingHitchLength);
            }
            else//rigidly connected to vehicle
            {
                double cos = Math.Cos(fixHeading);
                double sin = Math.Sin(fixHeading);

                if (mf.toolGPSWatchdog < 11)
                {
                    vec2 toolPos2 = new vec2(mf.mc.fixTool.easting, mf.mc.fixTool.northing);

                    //offset based on settings
                    if (AntennaOffset != 0)
                    {
                        toolPos2.easting -= cos * AntennaOffset;
                        toolPos2.northing += sin * AntennaOffset;
                    }
                    if (mf.mc.imuRollTool != 88888 && AntennaHeight != 0)
                    {
                        //change for roll to the right is positive times -1
                        double rollCorrectionDistance = Math.Sin(glm.toRadians((mf.mc.imuRollTool))) * AntennaHeight;

                        toolPos2.easting -= cos * rollCorrectionDistance;
                        toolPos2.northing += sin * rollCorrectionDistance;
                    }

                    toolSteerShift = cos * (toolPos2.easting - hitchPos.easting) - sin * (toolPos2.northing - hitchPos.northing);
                }
                else if (isSteering)
                {
                    toolSteerShift = mf.mc.toolActualDistance;
                }
                else
                {
                    toolSteerShift = 0;
                }

                toolHeading = fixHeading;
                WorkPos.easting = hitchPos.easting + (cos * toolSteerShift);
                WorkPos.northing = hitchPos.northing - (sin * toolSteerShift);
            }
        }


        //calculate the extreme tool left, right velocities, each section lookahead, and whether or not its going backwards
        public void CalculateSectionLookAhead()
        {
            //precalc the sin and cos of heading * -1
            sinSectionHeading = Math.Sin(-toolHeading);
            cosSectionHeading = Math.Cos(-toolHeading);

            //calculate left side of section 1
            vec2 left = new vec2();
            vec2 right = left;
            double leftSpeed = 0, rightSpeed = 0;

            //speed max for section kmh*0.277 to m/s * 10 cm per pixel * 1.7 max speed
            double meterPerSecPerPixel = Math.Abs(mf.mc.avgSpeed) * 4.5;

            vec2 lastLeftPoint = leftPoint;
            //only one first left point, the rest are all rights moved over to left
            leftPoint = new vec2(cosSectionHeading * (toolFarLeftPosition + toolOffset) + WorkPos.easting,
                               sinSectionHeading * (toolFarLeftPosition + toolOffset) + WorkPos.northing);

            left = leftPoint - lastLeftPoint;

            //get the speed for left side only once
            leftSpeed = left.GetLength() * mf.HzTime;
            if (leftSpeed > 27.7778) leftSpeed = 27.7778;

            //Is section outer going forward or backward
            if (Math.PI - Math.Abs(Math.Abs(left.HeadingXZ() - toolHeading) - Math.PI) > glm.PIBy2)
            {
                if (leftSpeed > 0) leftSpeed *= -1;
            }
            toolFarLeftSpeed = toolFarLeftSpeed * 0.9 + leftSpeed * 0.1;


            vec2 lastRightPoint = rightPoint;

            rightPoint = new vec2(cosSectionHeading * (toolFarRightPosition + toolOffset) + WorkPos.easting,
                                sinSectionHeading * (toolFarRightPosition + toolOffset) + WorkPos.northing);

            //now we have left and right for this section
            right = rightPoint - lastRightPoint;

            //grab vector length and convert to meters/sec/10 pixels per meter                
            rightSpeed = right.GetLength() * mf.HzTime;
            if (rightSpeed > 27.7778) rightSpeed = 27.7778;

            if (Math.PI - Math.Abs(Math.Abs(right.HeadingXZ() - toolHeading) - Math.PI) > glm.PIBy2)
                if (rightSpeed > 0) rightSpeed *= -1;

            //save the far left and right speed in m/sec averaged over 20%

            toolFarRightSpeed = toolFarRightSpeed * 0.9 + rightSpeed * 0.1;


            vec2 tt = (rightPoint - leftPoint) / toolWidth;


            //now loop all the section rights and the one extreme left
            for (int j = 0; j < sections.Count; j++)
            {
                sections[j].leftPoint = leftPoint + tt * (sections[j].positionLeft - toolFarLeftPosition);
                sections[j].rightPoint = leftPoint + tt * (sections[j].positionRight - toolFarLeftPosition);
            }

            double oneFrameLeft = toolFarLeftSpeed / mf.HzTime * 10;
            double oneFrameRight = toolFarRightSpeed / mf.HzTime * 10;

            if (!isFastSections)
            {
                oneFrameLeft *= 2;
                oneFrameRight *= 2;
            }

            //set the look ahead for hyd Lift in pixels per second
            mf.vehicle.hydLiftLookAheadDistanceLeft = oneFrameLeft + toolFarLeftSpeed * mf.vehicle.hydLiftLookAheadTime * 10;
            mf.vehicle.hydLiftLookAheadDistanceRight = oneFrameRight + toolFarRightSpeed * mf.vehicle.hydLiftLookAheadTime * 10;

            if (mf.vehicle.hydLiftLookAheadDistanceLeft > 250)
                mf.vehicle.hydLiftLookAheadDistanceLeft = 250;
            else if (mf.vehicle.hydLiftLookAheadDistanceLeft < -250)
                mf.vehicle.hydLiftLookAheadDistanceLeft = -250;
            if (mf.vehicle.hydLiftLookAheadDistanceRight > 250)
                mf.vehicle.hydLiftLookAheadDistanceRight = 250;
            else if (mf.vehicle.hydLiftLookAheadDistanceRight < -250)
                mf.vehicle.hydLiftLookAheadDistanceRight = -250;

            double oneFrameLeftBoundary = oneFrameLeft * ((boundOverlap - 50.0) * 0.02);
            double oneFrameRightBoundary = oneFrameRight * ((boundOverlap - 50.0) * 0.02);

            oneFrameLeft *= (maxOverlap - 50.0) * 0.02;
            oneFrameRight *= (maxOverlap - 50.0) * 0.02;

            lookAheadBoundaryOnPixelsLeft = Math.Max(oneFrameLeftBoundary, 0) + toolFarLeftSpeed * lookAheadOnSetting * 10;
            lookAheadBoundaryOnPixelsRight = Math.Max(oneFrameRightBoundary, 0) + toolFarRightSpeed * lookAheadOnSetting * 10;

            lookAheadBoundaryOffPixelsLeft = Math.Max(-oneFrameLeftBoundary, 0) + toolFarLeftSpeed * lookAheadOffSetting * 10;
            lookAheadBoundaryOffPixelsRight = Math.Max(-oneFrameRightBoundary, 0) + toolFarRightSpeed * lookAheadOffSetting * 10;

            lookAheadDistanceOnPixelsLeft = Math.Max(oneFrameLeft, 0) + toolFarLeftSpeed * lookAheadOnSetting * 10;
            lookAheadDistanceOnPixelsRight = Math.Max(oneFrameRight, 0) + toolFarRightSpeed * lookAheadOnSetting * 10;

            lookAheadDistanceOffPixelsLeft = Math.Max(-oneFrameLeft, 0) + toolFarLeftSpeed * lookAheadOffSetting * 10;
            lookAheadDistanceOffPixelsRight = Math.Max(-oneFrameRight, 0) + toolFarRightSpeed * lookAheadOffSetting * 10;

            MinMaxValue(ref lookAheadBoundaryOnPixelsLeft);
            MinMaxValue(ref lookAheadBoundaryOnPixelsRight);

            MinMaxValue(ref lookAheadBoundaryOffPixelsLeft);
            MinMaxValue(ref lookAheadBoundaryOffPixelsRight);

            MinMaxValue(ref lookAheadDistanceOnPixelsLeft);
            MinMaxValue(ref lookAheadDistanceOnPixelsRight);

            MinMaxValue(ref lookAheadDistanceOffPixelsLeft);
            MinMaxValue(ref lookAheadDistanceOffPixelsRight);

            double lookAheadMin1 = Math.Min(lookAheadBoundaryOnPixelsLeft, lookAheadBoundaryOnPixelsRight);
            double lookAheadMax1 = Math.Max(lookAheadBoundaryOnPixelsLeft, lookAheadBoundaryOnPixelsRight);
            double lookAheadMin2 = Math.Min(lookAheadBoundaryOffPixelsLeft, lookAheadBoundaryOffPixelsRight);
            double lookAheadMax2 = Math.Max(lookAheadBoundaryOffPixelsLeft, lookAheadBoundaryOffPixelsRight);
            double lookAheadMin3 = Math.Min(lookAheadDistanceOnPixelsLeft, lookAheadDistanceOnPixelsRight);
            double lookAheadMax3 = Math.Max(lookAheadDistanceOnPixelsLeft, lookAheadDistanceOnPixelsRight);
            double lookAheadMin4 = Math.Min(lookAheadDistanceOffPixelsLeft, lookAheadDistanceOffPixelsRight);
            double lookAheadMax4 = Math.Max(lookAheadDistanceOffPixelsLeft, lookAheadDistanceOffPixelsRight);

            lookAheadMin = Math.Min(Math.Min(lookAheadMin1, lookAheadMin2), Math.Min(lookAheadMin3, lookAheadMin4));
            lookAheadMax = Math.Max(Math.Max(lookAheadMax1, lookAheadMax2), Math.Max(lookAheadMax3, lookAheadMax4));
            if (mf.vehicle.isHydLiftOn)
            {
                double lookAheadMin5 = Math.Min(mf.vehicle.hydLiftLookAheadDistanceLeft, mf.vehicle.hydLiftLookAheadDistanceRight);
                double lookAheadMax5 = Math.Max(mf.vehicle.hydLiftLookAheadDistanceLeft, mf.vehicle.hydLiftLookAheadDistanceRight);

                lookAheadMin = Math.Min(lookAheadMin, lookAheadMin5);
                lookAheadMax = Math.Max(lookAheadMax, lookAheadMax5);
            }

            //used to increase triangle count when going around corners, less on straight
            //pick the slow moving side edge of tool
            double distance = toolWidth * 0.5;
            if (distance > 3) distance = 3;

            //whichever is less
            if (toolFarLeftSpeed < toolFarRightSpeed)
            {
                double twist = toolFarLeftSpeed / toolFarRightSpeed;
                //twist *= twist;
                if (twist < 0.2) twist = 0.2;
                mf.sectionTriggerStepDistance = distance * twist * twist;
            }
            else
            {
                double twist = toolFarRightSpeed / toolFarLeftSpeed;
                //twist *= twist;
                if (twist < 0.2) twist = 0.2;

                mf.sectionTriggerStepDistance = distance * twist * twist;
            }

            //finally fixed distance for making a curve line
            if (!mf.gyd.isOkToAddDesPoints) mf.sectionTriggerStepDistance = mf.sectionTriggerStepDistance + 0.2;
            if (mf.gyd.CurrentGMode == Mode.Contour) mf.sectionTriggerStepDistance *= 0.5;
        }

        private static double maxLookAhead = 250;
        public void MinMaxValue(ref double dd)
        {
            if (dd > maxLookAhead)
                dd = maxLookAhead;
            else if (dd < -maxLookAhead)
                dd = -maxLookAhead;
        }

        public void SectionSetPosition()
        {
            for (int j = sections.Count - 2; j >= numOfSections; j--)
            {
                mf.Controls.Remove(sections[j].button);
                sections.RemoveAt(j);//save triangle list first?
            }

            double defaultWidthsetting = Properties.Vehicle.Default.Tool_defaultSectionWidth;

            string[] words = Properties.Vehicle.Default.Tool_SectionWidths.Split(',');
            double leftOffset = toolWidth * -0.5;


            for (int j = 0; j < numOfSections; j++)
            {
                if (j + 1 >= sections.Count)
                {
                    sections.Insert(sections.Count - 1, new CSection(mf, j));
                    // using this to name the controls we add - makes finding them for Update with rate later a lot easier.
                    sections[j].button.Name = "section" + (j+1);
                    mf.Controls.Add(sections[j].button);
                    sections[j].button.BringToFront();
                    if (glm.isSimEnabled)
                        sections[j].UpdateButton(mf.autoBtnState);
                }
                sections[j].positionLeft = leftOffset;
                sections[j].rpSectionPosition = 250 + (int)Math.Round(sections[j].positionLeft * 10, 0, MidpointRounding.AwayFromZero);

                leftOffset += j < words.Length ? double.Parse(words[j], CultureInfo.InvariantCulture) : defaultWidthsetting;

                sections[j].positionRight = leftOffset;
                sections[j].rpSectionWidth = (int)Math.Round((sections[j].positionRight - sections[j].positionLeft) * 10, 0, MidpointRounding.AwayFromZero);
                //leftOffset += space between sections;

            }

            //left and right tool position
            toolFarLeftPosition = sections[0].positionLeft;
            toolFarRightPosition = sections[numOfSections - 1].positionRight;

            //now do the full width section
            sections[numOfSections].positionLeft = toolFarLeftPosition;
            sections[numOfSections].positionRight = toolFarRightPosition;

            //find the right side pixel position
            rpXPosition = 250 + (int)Math.Round(toolFarLeftPosition * 10, 0, MidpointRounding.AwayFromZero);
            rpWidth = (int)Math.Round(toolWidth * 10, 0, MidpointRounding.AwayFromZero);

            updateVBO = true;
        }

        //Does the logic to process section on off requests
        public void ProcessSectionOnOffRequests()
        {
            if (sections.Count > 0)
            {
                double timer = mf.HzTime / (isFastSections ? 1 : 2);
                for (int j = 0; j < numOfSections; j++)
                {
                    //SECTIONS - 
                    if (sections[j].sectionOnRequest > 1)
                    {
                        sections[j].isSectionOn = true;

                        sections[j].sectionOverlapTimer = (int)Math.Max(timer * turnOffDelay, 1);

                        if (!sections[j].isMappingOn && sections[j].mappingOnTimer == 0)
                            sections[j].mappingOnTimer = (int)Math.Max(timer * mappingOnDelay + 1, 1);

                        sections[j].mappingOffTimer = (int)(timer * mappingOffDelay + 2);
                    }
                    else if (sections[j].sectionOverlapTimer > 0)
                    {
                        sections[j].sectionOverlapTimer--;
                        if (sections[j].isSectionOn && sections[j].sectionOverlapTimer == 0)
                            sections[j].isSectionOn = false;
                        else
                            sections[j].mappingOffTimer = (int)(timer * mappingOffDelay + 2);
                    }
                    if (sections[j].sectionOnRequest > 0)
                        sections[j].sectionOnRequest--;

                    //MAPPING -
                    if (sections[numOfSections].sectionOnRequest > 0)
                    {
                        sections[j].mappingOnTimer = 2;
                        if (sections[j].isMappingOn)
                            sections[j].TurnMappingOff();
                    }
                    if (sections[j].mappingOnTimer > 0 && sections[j].mappingOffTimer > 1)
                    {
                        sections[j].mappingOnTimer--;
                        if (!sections[j].isMappingOn && sections[j].mappingOnTimer == 0)
                            sections[j].TurnMappingOn();
                    }
                    if (sections[j].mappingOffTimer > 0)
                    {
                        sections[j].mappingOffTimer--;
                        if (sections[j].mappingOffTimer == 0)
                        {
                            sections[j].mappingOnTimer = 0;
                            if (sections[j].isMappingOn)
                                sections[j].TurnMappingOff();
                        }
                    }
                }

                if (sections[numOfSections].sectionOnRequest > 0)
                {
                    if (!sections[numOfSections].isMappingOn)
                        sections[numOfSections].TurnMappingOn();
                }
                else if (sections[numOfSections].isMappingOn)
                    sections[numOfSections].TurnMappingOff();
            }
        }

        public void DrawTool()
        {
            //translate and rotate at pivot axle
            GL.Translate(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
            GL.PushMatrix();

            if (mf.vehicleGPSWatchdog < 11)
                //translate down to the hitch pin
                GL.Translate(mf.sinH * hitchLength, mf.cosH * hitchLength, 0);
            
            if (updateVBO)
            {
                updateVBO = false;
                if (_ToolVBO == 0)
                    _ToolVBO = GL.GenBuffer();

                vec2Short[] _vertices = new vec2Short[12 + numOfSections * 5];
                //Tank Hitch
                _vertices[0] = new vec2Short(0, TankAxleLength + TankHitchLength);
                _vertices[1] = new vec2Short(0, TankAxleLength);
                //Tank Triangle
                _vertices[2] = new vec2Short(-0.5, TankAxleLength);
                _vertices[3] = new vec2Short(0, 0);
                _vertices[4] = new vec2Short(0.5, TankAxleLength);
                //Trailing Hitch
                _vertices[5] = new vec2Short(toolOffset, TrailingAxleLength);
                _vertices[6] = new vec2Short(toolOffset, TrailingAxleLength + TrailingHitchLength);
                //Trailing Triangle
                _vertices[7] = new vec2Short(-0.4 + toolOffset, TrailingAxleLength);
                _vertices[8] = new vec2Short(0, 0);
                _vertices[9] = new vec2Short(0.4 + toolOffset, TrailingAxleLength);

                //Tram Dots
                _vertices[10] = new vec2Short((mf.tram.isOuter ? (toolFarRightPosition - mf.tram.halfWheelTrack) : mf.tram.halfWheelTrack), 0.21);
                _vertices[11] = new vec2Short((mf.tram.isOuter ? (toolFarLeftPosition + mf.tram.halfWheelTrack) : -mf.tram.halfWheelTrack), 0.21);

                double hiteA = -0.5;

                for (int j = 0; j < numOfSections; j++)
                {
                    _vertices[12 + j * 5] = new vec2Short(sections[j].positionLeft, 0);
                    _vertices[13 + j * 5] = new vec2Short(sections[j].positionLeft, hiteA);
                    _vertices[14 + j * 5] = new vec2Short(((sections[j].positionRight - sections[j].positionLeft) * 0.5 + sections[j].positionLeft), hiteA * 1.5);
                    _vertices[15 + j * 5] = new vec2Short(sections[j].positionRight, hiteA);
                    _vertices[16 + j * 5] = new vec2Short(sections[j].positionRight, 0);
                }

                GL.BindBuffer(BufferTarget.ArrayBuffer, _ToolVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(short) * 2, _vertices, BufferUsageHint.StaticDraw);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, _ToolVBO);
            GL.VertexPointer(2, VertexPointerType.Short, 0, 0);
            GL.Scale(0.01, 0.01, 0.01);

            if (isToolTrailing && mf.vehicleGPSWatchdog < 11)
            {
                if (isToolTBT)
                {
                    //rotate to tank heading
                    GL.Rotate(glm.toDegrees(-tankHeading), 0.0, 0.0, 1.0);

                    //draw the tank hitch
                    GL.LineWidth(4);
                    GL.Color3(0, 0, 0);
                    GL.DrawArrays(PrimitiveType.Lines, 0, 2);

                    GL.LineWidth(2);
                    GL.Color3(1.237f, 0.037f, 0.0397f);
                    GL.DrawArrays(PrimitiveType.Lines, 0, 2);

                    //draw the tank axle
                    GL.LineWidth(4);
                    GL.Color3(0, 0, 0);
                    GL.DrawArrays(PrimitiveType.LineLoop, 2, 3);

                    GL.LineWidth(2);
                    GL.Color3(0.765f, 0.76f, 0.32f);
                    GL.DrawArrays(PrimitiveType.LineLoop, 2, 3);

                    //move down the tank hitch, unwind, rotate to section heading
                    GL.Translate(0.0, (TankAxleLength + TankHitchLength) * 100, 0.0);
                    GL.Rotate(glm.toDegrees(tankHeading), 0.0, 0.0, 1.0);
                }

                GL.Rotate(glm.toDegrees(-toolHeading), 0.0, 0.0, 1.0);

                //draw the tool hitch
                GL.LineWidth(4);
                GL.Color3(0, 0, 0);
                GL.DrawArrays(PrimitiveType.Lines, 5, 2);

                GL.LineWidth(2);
                GL.Color3(1.237f, 0.037f, 0.0397f);
                GL.DrawArrays(PrimitiveType.Lines, 5, 2);

                //draw the tool axle
                GL.LineWidth(4);
                GL.Color3(0, 0, 0);
                GL.DrawArrays(PrimitiveType.LineLoop, 7, 3);

                GL.LineWidth(2);
                GL.Color3(0.7f, 0.4f, 0.2f);
                GL.DrawArrays(PrimitiveType.LineLoop, 7, 3);

                GL.Translate((toolOffset + toolSteerShift) * 100, (TrailingAxleLength + TrailingHitchLength) * 100, 0.0);
            }
            else//no tow between hitch
            {
                GL.Rotate(glm.toDegrees(-toolHeading), 0.0, 0.0, 1.0);
                GL.Translate((toolOffset + toolSteerShift) * 100, 0.0, 0.0);
            }

            //look ahead lines
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);

            //lookahead section off
            GL.Color3(0.70f, 0.2f, 0.2f);
            DrawLookAheadLine(lookAheadDistanceOffPixelsLeft * 10, lookAheadDistanceOffPixelsRight * 10);

            //lookahead section on
            GL.Color3(0.20f, 0.7f, 0.2f);
            DrawLookAheadLine(lookAheadDistanceOnPixelsLeft * 10, lookAheadDistanceOnPixelsRight * 10);

            if (mf.vehicle.isHydLiftOn)
            {
                GL.Color3(0.70f, 0.2f, 0.72f);
                DrawLookAheadLine(mf.vehicle.hydLiftLookAheadDistanceLeft * 10, mf.vehicle.hydLiftLookAheadDistanceRight * 10);
            }

            GL.End();

            //draw the sections
            GL.LineWidth(2);

            for (int j = 0; j < numOfSections; j++)
            {
                if (sections[j].sectionOnRequest > 0)
                {
                    if (sections[j].isMappingOn || sections[numOfSections].isMappingOn)
                    {
                        //Both On
                        if (sections[j].sectionState == btnStates.Auto)
                            GL.Color3(0.0f, 0.9f, 0.0f);
                        else
                            GL.Color3(0.97f, 0.97f, 0f);
                    }
                    else //Section wants to turn mapping on
                        GL.Color3(0.5f, 0.0f, 1.0f);//violet
                }
                else if (sections[j].isSectionOn || sections[numOfSections].isSectionOn)
                    //Section wants to turn off
                    GL.Color3(1.0f, 0.647f, 0.0f);//orange
                else if (sections[j].isMappingOn || sections[numOfSections].isMappingOn)
                    //Section wants to turn mapping off
                    GL.Color3(0.5f, 0.5f, 0.5f);//gray
                else if (sections[j].mappingOnTimer > 0 && sections[j].mappingOffTimer > sections[j].mappingOnTimer)
                    GL.Color3(0.5f, 0.0f, 1.0f);//violet
                else
                    //Both Off
                    GL.Color3(0.97f, 0.0f, 0.0f);//red

                GL.DrawArrays(PrimitiveType.TriangleFan, 12 + j * 5, 5);
                GL.Color3(0.0, 0.0, 0.0);
                GL.DrawArrays(PrimitiveType.LineLoop, 12 + j * 5, 5);
            }

            //tram Dots
            if (mf.tram.displayMode != 0 && mf.worldManager.camSetDistance < 200)
            {
                GL.PointSize(8);

                //right side
                if (((mf.tram.controlByte) & 1) == 1) GL.Color3(0.0f, 0.900f, 0.39630f);
                else GL.Color3(0.90f, 0.00f, 0.0f);
                GL.DrawArrays(PrimitiveType.Points, 10, 1);

                //left side
                if ((mf.tram.controlByte & 2) == 2) GL.Color3(0.0f, 0.900f, 0.3930f);
                else GL.Color3(0.90f, 0.00f, 0.0f);
                GL.DrawArrays(PrimitiveType.Points, 11, 1);
            }
            GL.PopMatrix();
        }

        public void DrawLookAheadLine(double leftLookAhead, double rightLookAhead)
        {
            if (leftLookAhead > 0 && rightLookAhead > 0)
            {
                GL.Vertex2(toolFarLeftPosition * 100, leftLookAhead);
                GL.Vertex2(toolFarRightPosition * 100, rightLookAhead);
            }
            else
            {
                double mOn = (rightLookAhead - leftLookAhead) / toolWidth;

                if (toolFarLeftSpeed > 0)
                {
                    GL.Vertex2(toolFarLeftPosition * 100, leftLookAhead);
                    GL.Vertex2((toolFarLeftPosition - leftLookAhead / mOn) * 100, 0);
                }
                else if (rightLookAhead > 0)
                {
                    GL.Vertex2(toolFarRightPosition * 100, rightLookAhead);
                    GL.Vertex2((toolFarRightPosition - rightLookAhead / mOn) * 100, 0);
                }
            }
        }
    }
}
