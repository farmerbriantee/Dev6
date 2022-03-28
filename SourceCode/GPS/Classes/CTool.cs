
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;


namespace AgOpenGPS
{
    public class CTool
    {
        private readonly FormGPS mf;

        public double toolWidth;
        public double toolFarLeftPosition = 0;
        public double toolFarLeftSpeed = 0;
        public double toolFarRightPosition = 0;
        public double toolFarRightSpeed = 0;

        public double toolOverlap;
        public double toolTrailingHitchLength, toolTankTrailingHitchLength;
        public double toolOffset;

        public double lookAheadOffSetting, lookAheadOnSetting;
        public double turnOffDelay;

        public double lookAheadDistanceOnPixelsLeft, lookAheadDistanceOnPixelsRight;
        public double lookAheadDistanceOffPixelsLeft, lookAheadDistanceOffPixelsRight;
        public double lookAheadBoundaryOnPixelsLeft, lookAheadBoundaryOnPixelsRight;
        public double lookAheadBoundaryOffPixelsLeft, lookAheadBoundaryOffPixelsRight;

        public bool isToolTrailing, isToolTBT;
        public bool isToolRearFixed, isToolFrontFixed;

        public bool isMultiColoredSections;
        public string toolAttachType;

        public double hitchLength;

        //how many individual sections
        public int numOfSections;
        public int numSuperSection;

        //used for super section off on
        public int minOverlap;
        public double boundOverlap = 0.0;

        //read pixel values
        public int rpXPosition;
        public int rpWidth;

        public Color[] secColors = new Color[16];

        //Constructor called by FormGPS
        public CTool(FormGPS _f)
        {

            mf = _f;

            //from settings grab the vehicle specifics
            toolWidth = Properties.Vehicle.Default.setVehicle_toolWidth;
            toolOverlap = Properties.Vehicle.Default.setVehicle_toolOverlap;

            toolOffset = Properties.Vehicle.Default.setVehicle_toolOffset;

            toolTrailingHitchLength = Properties.Vehicle.Default.setTool_toolTrailingHitchLength;
            toolTankTrailingHitchLength = Properties.Vehicle.Default.setVehicle_tankTrailingHitchLength;
            hitchLength = Properties.Vehicle.Default.setVehicle_hitchLength;

            isToolRearFixed = Properties.Vehicle.Default.setTool_isToolRearFixed;
            isToolTrailing = Properties.Vehicle.Default.setTool_isToolTrailing;
            isToolTBT = Properties.Vehicle.Default.setTool_isToolTBT;
            isToolFrontFixed = Properties.Vehicle.Default.setTool_isToolFront;

            lookAheadOnSetting = Properties.Vehicle.Default.setVehicle_toolLookAheadOn;
            lookAheadOffSetting = Properties.Vehicle.Default.setVehicle_toolLookAheadOff;
            turnOffDelay = Properties.Vehicle.Default.setVehicle_toolOffDelay;

            numOfSections = Properties.Vehicle.Default.setVehicle_numSections;
            numSuperSection = numOfSections + 1;

            minOverlap = Properties.Vehicle.Default.setVehicle_minCoverage;
            isMultiColoredSections = Properties.Settings.Default.setColor_isMultiColorSections;

            secColors[0] =  Properties.Settings.Default.setColor_sec01;
            secColors[1] =  Properties.Settings.Default.setColor_sec02;
            secColors[2] =  Properties.Settings.Default.setColor_sec03;
            secColors[3] =  Properties.Settings.Default.setColor_sec04;
            secColors[4] =  Properties.Settings.Default.setColor_sec05;
            secColors[5] =  Properties.Settings.Default.setColor_sec06;
            secColors[6] =  Properties.Settings.Default.setColor_sec07;
            secColors[7] =  Properties.Settings.Default.setColor_sec08;
            secColors[8] =  Properties.Settings.Default.setColor_sec09;
            secColors[9] =  Properties.Settings.Default.setColor_sec10;
            secColors[10] = Properties.Settings.Default.setColor_sec11;
            secColors[11] = Properties.Settings.Default.setColor_sec12;
            secColors[12] = Properties.Settings.Default.setColor_sec13;
            secColors[13] = Properties.Settings.Default.setColor_sec14;
            secColors[14] = Properties.Settings.Default.setColor_sec15;
            secColors[15] = Properties.Settings.Default.setColor_sec16;
        }

        public void DrawTool()
        {

            //translate and rotate at pivot axle
            GL.Translate(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
            GL.PushMatrix();

            //translate down to the hitch pin
            GL.Translate(Math.Sin(mf.fixHeading) * hitchLength,
                            Math.Cos(mf.fixHeading) * hitchLength, 0);

            //settings doesn't change trailing hitch length if set to rigid, so do it here
            double trailingTank, trailingTool;
            if (isToolTrailing)
            {
                trailingTank = toolTankTrailingHitchLength;
                trailingTool = toolTrailingHitchLength;
            }
            else { trailingTank = 0; trailingTool = 0; }

            //there is a trailing tow between hitch
            if (isToolTBT && isToolTrailing)
            {
                //rotate to tank heading
                GL.Rotate(glm.toDegrees(-mf.tankPos.heading), 0.0, 0.0, 1.0);

                //draw the tank hitch
                GL.LineWidth(6);
                //draw the rigid hitch
                GL.Color3(0, 0, 0);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(-0.57, trailingTank, 0);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0.57, trailingTank, 0);

                GL.End();

                GL.LineWidth(1);
                //draw the rigid hitch
                GL.Color3(0.765f, 0.76f, 0.32f);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(-0.57, trailingTank, 0);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0.57, trailingTank, 0);

                GL.End();

                //move down the tank hitch, unwind, rotate to section heading
                GL.Translate(0.0, trailingTank, 0.0);
                GL.Rotate(glm.toDegrees(mf.tankPos.heading), 0.0, 0.0, 1.0);
                GL.Rotate(glm.toDegrees(-mf.toolPos.heading), 0.0, 0.0, 1.0);
            }
            else//no tow between hitch
            {
                GL.Rotate(glm.toDegrees(-mf.toolPos.heading), 0.0, 0.0, 1.0);
            }

            //draw the hitch if trailing
            if (isToolTrailing)
            {
                GL.LineWidth(6);
                GL.Color3(0, 0, 0);
                GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex3(-0.4 + mf.tool.toolOffset, trailingTool, 0);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0.4 + mf.tool.toolOffset, trailingTool, 0);

                GL.End();

                GL.LineWidth(1);
                //draw the rigid hitch
                GL.Color3(0.7f, 0.4f, 0.2f);
                GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex3(-0.4 + mf.tool.toolOffset, trailingTool, 0);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0.4 + mf.tool.toolOffset, trailingTool, 0);

                GL.End();
            }

            if (mf.isJobStarted)
            {
                //look ahead lines
                GL.LineWidth(1);
                GL.Begin(PrimitiveType.Lines);

                //lookahead section on
                GL.Color3(0.20f, 0.7f, 0.2f);
                DrawLookAheadLine(mf.tool.lookAheadOnSetting, mf.tool.lookAheadOnSetting, trailingTool);

                //lookahead section off
                GL.Color3(0.70f, 0.2f, 0.2f);
                DrawLookAheadLine(mf.tool.lookAheadOffSetting, mf.tool.lookAheadOffSetting, trailingTool);

                if (mf.vehicle.isHydLiftOn)
                {
                    GL.Color3(0.70f, 0.2f, 0.72f);
                    DrawLookAheadLine(mf.vehicle.hydLiftLookAheadTime, mf.vehicle.hydLiftLookAheadTime, trailingTool);
                }

                GL.End();
            }
            //draw the sections
            GL.LineWidth(2);

            double hite = mf.worldManager.camSetDistance / -150;
            if (hite > 0.7) hite = 0.7;
            if (hite < 0.5) hite = 0.5;

            for (int j = 0; j < numOfSections; j++)
            {
                if (mf.section[j].sectionOnRequest)
                {
                    if (mf.section[j].isMappingOn || mf.section[numOfSections].isMappingOn)
                    {
                        //Both On
                        if (mf.section[j].sectionState == btnStates.Auto)
                            GL.Color3(0.0f, 0.9f, 0.0f);
                        else
                            GL.Color3(0.97f, 0.97f, 0f);
                    }
                    else //Section wants to turn mapping on
                        GL.Color3(0.5f, 0.0f, 1.0f);//violet
                }
                else if (mf.section[j].isSectionOn || mf.section[numOfSections].isSectionOn)
                    //Section wants to turn off
                    GL.Color3(1.0f, 0.647f, 0.0f);//orange
                else if (mf.section[j].isMappingOn || mf.section[numOfSections].isMappingOn)
                    //Section wants to turn mapping off
                    GL.Color3(0.5f, 0.5f, 0.5f);//gray
                else if (mf.section[j].mappingOnTimer > 0 && mf.section[j].mappingOffTimer > mf.section[j].mappingOnTimer)
                    GL.Color3(0.5f, 0.0f, 1.0f);//violet
                else
                    //Both Off
                    GL.Color3(0.97f, 0.0f, 0.0f);//red

                double mid = (mf.section[j].positionRight - mf.section[j].positionLeft) / 2 + mf.section[j].positionLeft;

                GL.Begin(PrimitiveType.TriangleFan);
                {
                    GL.Vertex3(mf.section[j].positionLeft, trailingTool, 0);
                    GL.Vertex3(mf.section[j].positionLeft, trailingTool - hite, 0);

                    GL.Vertex3(mid, trailingTool - hite * 1.5, 0);

                    GL.Vertex3(mf.section[j].positionRight, trailingTool - hite, 0);
                    GL.Vertex3(mf.section[j].positionRight, trailingTool, 0);
                }
                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Color3(0.0, 0.0, 0.0);
                    GL.Vertex3(mf.section[j].positionLeft, trailingTool, 0);
                    GL.Vertex3(mf.section[j].positionLeft, trailingTool - hite, 0);

                    GL.Vertex3(mid, trailingTool - hite * 1.5, 0);

                    GL.Vertex3(mf.section[j].positionRight, trailingTool - hite, 0);
                    GL.Vertex3(mf.section[j].positionRight, trailingTool, 0);
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

                    GL.Vertex3(mf.tram.isOuter ? (toolFarRightPosition - mf.tram.halfWheelTrack) : mf.tram.halfWheelTrack, trailingTool + 0.21, 0);

                    //left side
                    if ((mf.tram.controlByte & 2) == 2) GL.Color3(0.0f, 0.900f, 0.3930f);
                    else GL.Color3(0.90f, 0.00f, 0.0f);

                    GL.Vertex3(mf.tram.isOuter ? (toolFarLeftPosition + mf.tram.halfWheelTrack) : (-mf.tram.halfWheelTrack), trailingTool + 0.21, 0);
                    GL.End();
                    GL.End();
                }
            }
            GL.PopMatrix();
        }

        public void DrawLookAheadLine(double leftLookAhead, double rightLookAhead, double trailingTool)
        {
            leftLookAhead *= mf.tool.toolFarLeftSpeed;
            rightLookAhead *= mf.tool.toolFarRightSpeed;

            if (leftLookAhead > 0 && rightLookAhead > 0)
            {
                GL.Vertex3(mf.tool.toolFarLeftPosition, leftLookAhead + trailingTool, 0);
                GL.Vertex3(mf.tool.toolFarRightPosition, rightLookAhead + trailingTool, 0);
            }
            else
            {
                double mOn = (rightLookAhead - leftLookAhead) / toolWidth;

                if (mf.tool.toolFarLeftSpeed > 0)
                {
                    GL.Vertex3(toolFarLeftPosition, leftLookAhead + trailingTool, 0);
                    GL.Vertex3(toolFarLeftPosition - leftLookAhead / mOn, trailingTool, 0);
                }
                else if (rightLookAhead > 0)
                {
                    GL.Vertex3(toolFarRightPosition, rightLookAhead + trailingTool, 0);
                    GL.Vertex3(toolFarRightPosition - rightLookAhead / mOn, trailingTool, 0);
                }
            }
        }
    }
}
