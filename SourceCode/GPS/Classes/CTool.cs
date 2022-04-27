
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
        public vec2 leftPoint, rightPoint;

        public double toolOverlap, toolOffset;
        public double toolTrailingHitchLength, toolTankTrailingHitchLength;

        public bool isSteering;
        public double AntennaOffset, AntennaHeight;

        public double lookAheadMin, lookAheadMax;

        public double lookAheadOffSetting, lookAheadOnSetting;
        public double turnOffDelay;

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

        //read pixel values
        public int rpXPosition, rpWidth;

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

            toolTrailingHitchLength = Properties.Vehicle.Default.Tool_TrailingHitchLength;
            toolTankTrailingHitchLength = Properties.Vehicle.Default.Tool_TankTrailingHitchLength;
            hitchLength = Properties.Vehicle.Default.setVehicle_hitchLength;

            isToolRearFixed = Properties.Vehicle.Default.Tool_isToolRearFixed;
            isToolTrailing = Properties.Vehicle.Default.Tool_isTrailing;
            isToolTBT = Properties.Vehicle.Default.Tool_isTBT;

            lookAheadOnSetting = Properties.Vehicle.Default.Tool_LookAheadOn;
            lookAheadOffSetting = Properties.Vehicle.Default.Tool_LookAheadOff;
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
                    mf.Controls.Add(sections[j].button);
                    sections[j].button.BringToFront();
                    if (mf.timerSim.Enabled)
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
            sections[sections.Count - 1].positionLeft = toolFarLeftPosition;
            sections[sections.Count - 1].positionRight = toolFarRightPosition;

            //find the right side pixel position
            rpXPosition = 250 + (int)Math.Round(toolFarLeftPosition * 10, 0, MidpointRounding.AwayFromZero);
            rpWidth = (int)Math.Round(toolWidth * 10, 0, MidpointRounding.AwayFromZero);
        }

        //Does the logic to process section on off requests
        public void ProcessSectionOnOffRequests()
        {
            if (sections.Count > 0)
            {
                double timer = mf.HzTime / (isFastSections ? 1 : 2);
                for (int j = 0; j < sections.Count - 1; j++)
                {
                    //SECTIONS - 
                    if (sections[j].sectionOnRequest > 1)
                    {
                        sections[j].isSectionOn = true;

                        sections[j].sectionOverlapTimer = (int)Math.Max(timer * turnOffDelay, 1);

                        if (!sections[j].isMappingOn && sections[j].mappingOnTimer == 0)
                            sections[j].mappingOnTimer = (int)Math.Max(timer * lookAheadOnSetting + 1, 1);//mappingOnDelay

                        sections[j].mappingOffTimer = (int)(timer * lookAheadOffSetting + 2);//mappingOffDelay
                    }
                    else if (sections[j].sectionOverlapTimer > 0)
                    {
                        sections[j].sectionOverlapTimer--;
                        if (sections[j].isSectionOn && sections[j].sectionOverlapTimer == 0)
                            sections[j].isSectionOn = false;
                        else
                            sections[j].mappingOffTimer = (int)(timer * lookAheadOffSetting + 2);//mappingOffDelay
                    }
                    if (sections[j].sectionOnRequest > 0)
                        sections[j].sectionOnRequest--;

                    //MAPPING -
                    if (sections[sections.Count - 1].sectionOnRequest > 0)
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

                if (sections[sections.Count - 1].sectionOnRequest > 0)
                {
                    if (!sections[sections.Count - 1].isMappingOn)
                        sections[sections.Count - 1].TurnMappingOn();
                }
                else if (sections[sections.Count - 1].isMappingOn)
                    sections[sections.Count - 1].TurnMappingOff();
            }
        }

        public void DrawTool()
        {
            //translate and rotate at pivot axle
            GL.Translate(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
            GL.PushMatrix();

            if (mf.vehicleGPSWatchdog < 11)
                //translate down to the hitch pin
                GL.Translate(Math.Sin(mf.fixHeading) * hitchLength,
                            Math.Cos(mf.fixHeading) * hitchLength, 0);

            if (mf.tool.isSteering && !isToolTrailing)
                GL.Translate(Math.Cos(mf.fixHeading) * toolSteerShift,
                                Math.Sin(mf.fixHeading) * -toolSteerShift, 0);

            if (isToolTrailing && mf.vehicleGPSWatchdog < 11)
            {
                if (isToolTBT)
                {
                    //rotate to tank heading
                    GL.Rotate(glm.toDegrees(-mf.tankPos.heading), 0.0, 0.0, 1.0);

                    //draw the tank hitch
                    GL.LineWidth(6);
                    //draw the rigid hitch
                    GL.Color3(0, 0, 0);
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(-0.57, toolTankTrailingHitchLength, 0);
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(0.57, toolTankTrailingHitchLength, 0);

                    GL.End();

                    GL.LineWidth(1);
                    //draw the rigid hitch
                    GL.Color3(0.765f, 0.76f, 0.32f);
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(-0.57, toolTankTrailingHitchLength, 0);
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(0.57, toolTankTrailingHitchLength, 0);

                    GL.End();

                    //move down the tank hitch, unwind, rotate to section heading
                    GL.Translate(0.0, toolTankTrailingHitchLength, 0.0);
                    GL.Rotate(glm.toDegrees(mf.tankPos.heading), 0.0, 0.0, 1.0);
                }

                GL.Rotate(glm.toDegrees(-mf.toolPos.heading), 0.0, 0.0, 1.0);

                GL.LineWidth(6);
                GL.Color3(0, 0, 0);
                GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex3(-0.4 + mf.tool.toolOffset, toolTrailingHitchLength, 0);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0.4 + mf.tool.toolOffset, toolTrailingHitchLength, 0);

                GL.End();

                GL.LineWidth(1);
                //draw the rigid hitch
                GL.Color3(0.7f, 0.4f, 0.2f);
                GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex3(-0.4 + mf.tool.toolOffset, toolTrailingHitchLength, 0);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0.4 + mf.tool.toolOffset, toolTrailingHitchLength, 0);

                GL.End();
                GL.Translate(toolOffset, toolTrailingHitchLength, 0.0);
            }
            else//no tow between hitch
            {
                GL.Rotate(glm.toDegrees(-mf.toolPos.heading), 0.0, 0.0, 1.0);
                GL.Translate(toolOffset, 0.0, 0.0);
            }

            if (mf.isJobStarted)
            {
                //look ahead lines
                GL.LineWidth(1);
                GL.Begin(PrimitiveType.Lines);

                //lookahead section off
                GL.Color3(0.70f, 0.2f, 0.2f);
                DrawLookAheadLine(lookAheadDistanceOffPixelsLeft * 0.1, lookAheadDistanceOffPixelsRight * 0.1);

                //lookahead section on
                GL.Color3(0.20f, 0.7f, 0.2f);
                DrawLookAheadLine(lookAheadDistanceOnPixelsLeft * 0.1, lookAheadDistanceOnPixelsRight * 0.1);

                if (mf.vehicle.isHydLiftOn)
                {
                    GL.Color3(0.70f, 0.2f, 0.72f);
                    DrawLookAheadLine(mf.vehicle.hydLiftLookAheadDistanceLeft * 0.1, mf.vehicle.hydLiftLookAheadDistanceRight * 0.1);
                }

                GL.End();
            }
            //draw the sections
            GL.LineWidth(2);

            double hite = mf.worldManager.camSetDistance / -150;
            if (hite > 0.7) hite = 0.7;
            if (hite < 0.5) hite = 0.5;

            for (int j = 0; j < sections.Count - 1; j++)
            {
                if (sections[j].sectionOnRequest > 0)
                {
                    if (sections[j].isMappingOn || sections[sections.Count - 1].isMappingOn)
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
                else if (sections[j].isSectionOn || sections[sections.Count - 1].isSectionOn)
                    //Section wants to turn off
                    GL.Color3(1.0f, 0.647f, 0.0f);//orange
                else if (sections[j].isMappingOn || sections[sections.Count - 1].isMappingOn)
                    //Section wants to turn mapping off
                    GL.Color3(0.5f, 0.5f, 0.5f);//gray
                else if (sections[j].mappingOnTimer > 0 && sections[j].mappingOffTimer > sections[j].mappingOnTimer)
                    GL.Color3(0.5f, 0.0f, 1.0f);//violet
                else
                    //Both Off
                    GL.Color3(0.97f, 0.0f, 0.0f);//red

                double mid = (sections[j].positionRight - sections[j].positionLeft) * 0.5 + sections[j].positionLeft;

                GL.Begin(PrimitiveType.TriangleFan);
                {
                    GL.Vertex3(sections[j].positionLeft, 0, 0);
                    GL.Vertex3(sections[j].positionLeft, -hite, 0);

                    GL.Vertex3(mid, -hite * 1.5, 0);

                    GL.Vertex3(sections[j].positionRight, -hite, 0);
                    GL.Vertex3(sections[j].positionRight, 0, 0);
                }
                GL.End();

                GL.Color3(0.0, 0.0, 0.0);
                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Vertex3(sections[j].positionLeft, 0, 0);
                    GL.Vertex3(sections[j].positionLeft, -hite, 0);

                    GL.Vertex3(mid, -hite * 1.5, 0);

                    GL.Vertex3(sections[j].positionRight, -hite, 0);
                    GL.Vertex3(sections[j].positionRight, 0, 0);
                }
                GL.End();
            }

            //tram Dots
            if (mf.tram.displayMode != 0)
            {
                if (mf.worldManager.camSetDistance > -200)
                {
                    GL.PointSize(8);

                    GL.Begin(PrimitiveType.Points);
                    //section markers
                    //right side
                    if (((mf.tram.controlByte) & 1) == 1) GL.Color3(0.0f, 0.900f, 0.39630f);
                    else GL.Color3(0.90f, 0.00f, 0.0f);

                    GL.Vertex3(mf.tram.isOuter ? (toolFarRightPosition - mf.tram.halfWheelTrack) : mf.tram.halfWheelTrack, 0.21, 0);

                    //left side
                    if ((mf.tram.controlByte & 2) == 2) GL.Color3(0.0f, 0.900f, 0.3930f);
                    else GL.Color3(0.90f, 0.00f, 0.0f);

                    GL.Vertex3(mf.tram.isOuter ? (toolFarLeftPosition + mf.tram.halfWheelTrack) : (-mf.tram.halfWheelTrack), 0.21, 0);
                    GL.End();
                }
            }
            GL.PopMatrix();
        }

        public void DrawLookAheadLine(double leftLookAhead, double rightLookAhead)
        {
            if (leftLookAhead > 0 && rightLookAhead > 0)
            {
                GL.Vertex3(toolFarLeftPosition, leftLookAhead, 0);
                GL.Vertex3(toolFarRightPosition, rightLookAhead, 0);
            }
            else
            {
                double mOn = (rightLookAhead - leftLookAhead) / toolWidth;

                if (toolFarLeftSpeed > 0)
                {
                    GL.Vertex3(toolFarLeftPosition, leftLookAhead, 0);
                    GL.Vertex3(toolFarLeftPosition - leftLookAhead / mOn, 0, 0);
                }
                else if (rightLookAhead > 0)
                {
                    GL.Vertex3(toolFarRightPosition, rightLookAhead, 0);
                    GL.Vertex3(toolFarRightPosition - rightLookAhead / mOn, 0, 0);
                }
            }
        }
    }
}
