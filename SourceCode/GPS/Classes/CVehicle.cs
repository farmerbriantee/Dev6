//Please, if you use this, share the improvements

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace AgOpenGPS
{
    public class CVehicle
    {
        private readonly FormGPS mf;

        public bool isSteerAxleAhead, isPivotBehindAntenna;
        public double antennaHeight, antennaPivot, antennaOffset;
        public double wheelbase, minTurningRadius;

        //autosteer values
        public double goalPointLookAhead, goalPointLookAheadMult;

        public double stanleyDistanceErrorGain, stanleyHeadingErrorGain;
        public double minLookAheadDistance = 2.0;
        public double maxSteerAngle;
        public double hydLiftLookAheadTime, trackWidth;

        public double hydLiftLookAheadDistanceLeft, hydLiftLookAheadDistanceRight;

        public int vehicleType;
        public bool isHydLiftOn;
        public double stanleyIntegralDistanceAwayTriggerAB, stanleyIntegralGainAB, purePursuitIntegralGain;


        //flag for free drive window to control autosteer
        public bool isInFreeDriveMode, isInFreeToolDriveMode;

        //the trackbar angle for free drive
        public double driveFreeSteerAngle = 0, driveFreeToolDistance = 0;
        private int vehicleVBO;
        public bool updateVBO;

        public CVehicle(FormGPS _f)
        {
            //constructor
            mf = _f;
        }

        public void LoadSettings()
        {
            isPivotBehindAntenna = Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna;
            antennaHeight = Properties.Vehicle.Default.setVehicle_antennaHeight;
            antennaPivot = Properties.Vehicle.Default.setVehicle_antennaPivot;
            antennaOffset = Properties.Vehicle.Default.setVehicle_antennaOffset;

            wheelbase = Properties.Vehicle.Default.setVehicle_wheelbase;
            minTurningRadius = Properties.Vehicle.Default.setVehicle_minTurningRadius;
            isSteerAxleAhead = Properties.Vehicle.Default.setVehicle_isSteerAxleAhead;

            goalPointLookAhead = Properties.Vehicle.Default.setVehicle_goalPointLookAhead;
            goalPointLookAheadMult = Properties.Vehicle.Default.setVehicle_goalPointLookAheadMult;

            stanleyDistanceErrorGain = Properties.Vehicle.Default.stanleyDistanceErrorGain;
            stanleyHeadingErrorGain = Properties.Vehicle.Default.stanleyHeadingErrorGain;

            maxSteerAngle = Properties.Vehicle.Default.setVehicle_maxSteerAngle;

            isHydLiftOn = false;

            trackWidth = Properties.Vehicle.Default.setVehicle_trackWidth;

            stanleyIntegralGainAB = Properties.Vehicle.Default.stanleyIntegralGainAB;
            stanleyIntegralDistanceAwayTriggerAB = Properties.Vehicle.Default.stanleyIntegralDistanceAwayTriggerAB;

            purePursuitIntegralGain = Properties.Vehicle.Default.purePursuitIntegralGainAB;
            vehicleType = Properties.Vehicle.Default.setVehicle_vehicleType;

            hydLiftLookAheadTime = Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead;
        }

        public double UpdateGoalPointDistance()
        {
            //how far should goal point be away  - speed * seconds * kmph -> m/s then limit min value
            double goalPointDistance = mf.mc.avgSpeed * goalPointLookAhead * 0.05 * goalPointLookAheadMult;
            goalPointDistance += goalPointLookAhead;

            if (goalPointDistance < 1) goalPointDistance = 1;

            return goalPointDistance;
        }

        public void DrawVehicle()
        {
            if (updateVBO)
            {
                vec2Short[] tt = new vec2Short[48];

                //Hitch
                tt[0] = new vec2Short((mf.tool.isToolRearFixed ? -0.35 : 0), mf.tool.hitchLength);
                tt[1] = new vec2Short((mf.tool.isToolRearFixed ? -0.35 : 0), 0);
                tt[2] = new vec2Short((mf.tool.isToolRearFixed ? 0.35 : 0), mf.tool.hitchLength);
                tt[3] = new vec2Short((mf.tool.isToolRearFixed ? 0.35 : 0), 0);

                if (vehicleType == 0)
                {
                    //tractor
                    tt[4] = new vec2Short(trackWidth, wheelbase * 1.5);
                    tt[5] = new vec2Short(-trackWidth, wheelbase * 1.5);
                    tt[6] = new vec2Short(trackWidth, wheelbase * -0.5);
                    tt[7] = new vec2Short(-trackWidth, wheelbase * -0.5);

                    //wheels
                    tt[8] = new vec2Short(trackWidth * 0.5, wheelbase * 0.75);
                    tt[9] = new vec2Short(trackWidth * -0.5, wheelbase * 0.75);
                    tt[10] = new vec2Short(trackWidth * 0.5, wheelbase * -0.75);
                    tt[11] = new vec2Short(trackWidth * -0.5, wheelbase * -0.75);
                }
                else if (vehicleType == 1)
                {
                    //tractor
                    tt[4] = new vec2Short(trackWidth, wheelbase * 1.5);
                    tt[5] = new vec2Short(-trackWidth, wheelbase * 1.5);
                    tt[6] = new vec2Short(trackWidth, wheelbase * -1.5);
                    tt[7] = new vec2Short(-trackWidth, wheelbase * -1.5);

                    //wheels
                    tt[8] = new vec2Short(trackWidth * 0.25, wheelbase * 0.5);
                    tt[9] = new vec2Short(trackWidth * -0.25, wheelbase * 0.5);
                    tt[10] = new vec2Short(trackWidth * 0.25, wheelbase * -0.5);
                    tt[11] = new vec2Short(trackWidth * -0.25, wheelbase * -0.5);
                }
                else if (vehicleType == 2)
                {
                    //tractor
                    tt[4] = new vec2Short(trackWidth, wheelbase * 0.65);
                    tt[5] = new vec2Short(-trackWidth, wheelbase * 0.65);
                    tt[6] = new vec2Short(trackWidth, wheelbase * -0.65);
                    tt[7] = new vec2Short(-trackWidth, wheelbase * -0.65);
                }
                //Heading QuestionMark
                tt[12] = new vec2Short(5, 5);
                tt[13] = new vec2Short(1, 5);
                tt[14] = new vec2Short(5, 1);
                tt[15] = new vec2Short(1, 1);

                //TexCoordPointer
                tt[16] = new vec2Short(0.01, 0);
                tt[17] = new vec2Short(0, 0);
                tt[18] = new vec2Short(0.01, 0.01);
                tt[19] = new vec2Short(0, 0.01);

                //Vehicle Triangle
                tt[20] = new vec2Short(trackWidth * 0.5, 0);
                tt[21] = new vec2Short(0, wheelbase);
                tt[22] = new vec2Short(trackWidth * -0.5, 0);


                //Vehicle Triangle
                tt[20] = new vec2Short(100, 0);
                tt[21] = new vec2Short(0, 100);
                tt[22] = new vec2Short(100, 0);

                //Antenna
                tt[23] = new vec2Short(antennaOffset, antennaPivot);
                tt[24] = new vec2Short(-antennaOffset, antennaPivot);

                double Offset = mf.bnd.isDrawRightSide ? mf.bnd.createBndOffset : -mf.bnd.createBndOffset;

                tt[25] = new vec2Short(0, 0);
                tt[26] = new vec2Short(Offset, 0);
                tt[27] = new vec2Short(Offset * 0.75, 0.25);

                double theta = glm.twoPI / 20;
                double c = Math.Cos(theta);//precalculate the sine and cosine
                double s = Math.Sin(theta);

                double x = 1;
                double y = 0;

                for (int ii = 0; ii < 20; ii++)
                {
                    //apply the rotation matrix
                    double t = x;
                    x = (c * x) - (s * y);
                    y = (s * t) + (c * y);

                    //Zoomed out Circle
                    tt[28 + ii] = new vec2Short(x, y);
                }

                updateVBO = false;
                if (vehicleVBO == 0) vehicleVBO = GL.GenBuffer();

                GL.BindBuffer(BufferTarget.ArrayBuffer, vehicleVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, tt.Length * sizeof(short) * 2, tt, BufferUsageHint.StaticDraw);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, vehicleVBO);
            GL.VertexPointer(2, VertexPointerType.Short, 0, 0);
            GL.Scale(0.01, 0.01, 0.01);

            //draw vehicle
            GL.Rotate(glm.toDegrees(mf.fixHeading), 0.0, 0.0, -1.0);

            if (!mf.tool.isToolFrontFixed)
            {
                GL.LineWidth(4);
                //draw the rigid hitch
                GL.Color3(0, 0, 0);
                GL.DrawArrays(PrimitiveType.Lines, 0, 4);

                GL.LineWidth(1);
                GL.Color3(1.237f, 0.037f, 0.0397f);
                GL.DrawArrays(PrimitiveType.Lines, 0, 4);
            }

            if (!mf.isFirstHeadingSet)
            {
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.Enable(EnableCap.Texture2D);
                GL.Color4(1.25f, 1.25f, 1.275f, 0.75);
                GL.BindTexture(TextureTarget.Texture2D, mf.texture[14]);        // Select Our Texture

                GL.TexCoordPointer(2, TexCoordPointerType.Short, 0, 16);
                GL.DrawArrays(PrimitiveType.TriangleStrip, 12, 4);
                GL.Disable(EnableCap.Texture2D);
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            }

            //3 vehicle types  tractor=0 harvestor=1 4wd=2

            if (mf.isVehicleImage)
            {
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.Enable(EnableCap.Texture2D);

                if (vehicleType == 0)
                {
                    double leftAckermam, rightAckerman;

                    if (glm.isSimEnabled)
                    {
                        if (mf.sim.steerAngle < 0)
                        {
                            leftAckermam = 1.25 * -mf.sim.steerAngle;
                            rightAckerman = -mf.sim.steerAngle;
                        }
                        else
                        {
                            leftAckermam = -mf.sim.steerAngle;
                            rightAckerman = 1.25 * -mf.sim.steerAngle;
                        }
                    }
                    else
                    {
                        if (mf.mc.actualSteerAngleDegrees < 0)
                        {
                            leftAckermam = 1.25 * -mf.mc.actualSteerAngleDegrees;
                            rightAckerman = -mf.mc.actualSteerAngleDegrees;
                        }
                        else
                        {
                            leftAckermam = -mf.mc.actualSteerAngleDegrees;
                            rightAckerman = 1.25 * -mf.mc.actualSteerAngleDegrees;
                        }
                    }

                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[13]);        // Select Our Texture
                    GL.TexCoordPointer(2, TexCoordPointerType.Short, 0, 48);
                    GL.Color4(mf.vehicleColor.R, mf.vehicleColor.G, mf.vehicleColor.B, mf.vehicleOpacityByte);

                    //vehicle body
                    GL.DrawArrays(PrimitiveType.TriangleStrip, 4, 4);


                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[15]);        // Select Our Texture
                    GL.TexCoordPointer(2, TexCoordPointerType.Short, 0, 32);

                    GL.PushMatrix();
                    GL.Translate(trackWidth * 50, wheelbase * 100, 0);
                    GL.Rotate(rightAckerman, 0, 0, 1);

                    //right wheel
                    GL.DrawArrays(PrimitiveType.TriangleStrip, 8, 4);

                    GL.PopMatrix();

                    GL.PushMatrix();
                    GL.Translate(-trackWidth * 50, wheelbase * 100, 0);
                    GL.Rotate(leftAckermam, 0, 0, 1);

                    //Left Wheel
                    GL.DrawArrays(PrimitiveType.TriangleStrip, 8, 4);

                    GL.PopMatrix();
                }
                else if (vehicleType == 1) //Harvestor
                {
                    double leftAckermam, rightAckerman;

                    if (glm.isSimEnabled)
                    {
                        if (mf.sim.steerAngle < 0)
                        {
                            leftAckermam = 1.25 * mf.sim.steerAngle;
                            rightAckerman = mf.sim.steerAngle;
                        }
                        else
                        {
                            leftAckermam = mf.sim.steerAngle;
                            rightAckerman = 1.25 * mf.sim.steerAngle;
                        }
                    }
                    else
                    {
                        if (mf.mc.actualSteerAngleDegrees < 0)
                        {
                            leftAckermam = 1.25 * mf.mc.actualSteerAngleDegrees;
                            rightAckerman = mf.mc.actualSteerAngleDegrees;
                        }
                        else
                        {
                            leftAckermam = mf.mc.actualSteerAngleDegrees;
                            rightAckerman = 1.25 * mf.mc.actualSteerAngleDegrees;
                        }
                    }

                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[15]);        // Select Our Texture
                    GL.TexCoordPointer(2, TexCoordPointerType.Short, 0, 32);
                    GL.Color4((byte)20, (byte)20, (byte)20, mf.vehicleOpacityByte);


                    GL.PushMatrix();
                    GL.Translate(trackWidth * 50, -wheelbase * 100, 0);
                    GL.Rotate(rightAckerman, 0, 0, 1);

                    //right wheel
                    GL.DrawArrays(PrimitiveType.TriangleStrip, 8, 4);

                    GL.PopMatrix();

                    GL.PushMatrix();
                    GL.Translate(-trackWidth * 50, -wheelbase * 100, 0);
                    GL.Rotate(leftAckermam, 0, 0, 1);

                    //Left Wheel
                    GL.DrawArrays(PrimitiveType.TriangleStrip, 8, 4);

                    GL.PopMatrix();


                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[18]);        // Select Our Texture
                    GL.TexCoordPointer(2, TexCoordPointerType.Short, 0, 48);
                    GL.Color4(mf.vehicleColor.R, mf.vehicleColor.G, mf.vehicleColor.B, mf.vehicleOpacityByte);

                    //vehicle body
                    GL.DrawArrays(PrimitiveType.TriangleStrip, 4, 4);


                    //disable, straight color
                }
                else if (vehicleType == 2) //4WD - Image Text # Front is 16 Rear is 17
                {
                    double modelSteerAngle;

                    if (glm.isSimEnabled)
                        modelSteerAngle = 0.5 * mf.sim.steerAngle;
                    else
                        modelSteerAngle = 0.5 * mf.mc.actualSteerAngleDegrees;

                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[17]);        // Select Our Texture
                    GL.TexCoordPointer(2, TexCoordPointerType.Short, 0, 48);
                    GL.Color4(mf.vehicleColor.R, mf.vehicleColor.G, mf.vehicleColor.B, mf.vehicleOpacityByte);


                    GL.PushMatrix();

                    GL.Translate(0, -wheelbase * 50, 0);
                    GL.Rotate(modelSteerAngle, 0, 0, 1);

                    //vehicle body
                    GL.DrawArrays(PrimitiveType.TriangleStrip, 4, 4);

                    GL.PopMatrix();
                    GL.PushMatrix();


                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[16]);        // Select Our Texture

                    GL.Translate(0, wheelbase * 50, 0);
                    GL.Rotate(-modelSteerAngle, 0, 0, 1);

                    //vehicle body
                    GL.DrawArrays(PrimitiveType.TriangleStrip, 4, 4);

                    GL.PopMatrix();
                }

                GL.Disable(EnableCap.Texture2D);
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            }
            else
            {
                GL.Color4(0.0, 1.0, 0.0, mf.vehicleOpacity);
                GL.DrawArrays(PrimitiveType.Triangles, 20, 3);

                GL.LineWidth(3);
                GL.Color3(0.12, 0.12, 0.12);
                GL.DrawArrays(PrimitiveType.LineLoop, 20, 3);
            }

            if (mf.worldManager.camSetDistance < 75)
            {
                //draw the bright antenna dot
                GL.PointSize(8.0f);
                GL.Color3(0.0f, 0.0f, 0.0f);
                GL.DrawArrays(PrimitiveType.Points, 23, mf.mc.headingTrueDual != double.MaxValue ? 2 : 1);

                GL.PointSize(4.0f);
                GL.Color3(0.20f, 1.0f, 1.0f);
                GL.DrawArrays(PrimitiveType.Points, 23, mf.mc.headingTrueDual != double.MaxValue ? 2 : 1);
            }

            if (mf.bnd.isBndBeingMade)
            {
                GL.LineWidth(2);
                GL.Color3(1.0, 0.0, 1.0);
                GL.DrawArrays(PrimitiveType.Lines, 25, 2);

                GL.Color3(1.0, 1.0, 0.20);
                GL.DrawArrays(PrimitiveType.Lines, 26, 2);
            }

            if (mf.gyd.CurrentGMode == Mode.Curve || mf.gyd.CurrentGMode == Mode.AB)
            {
                GL.Color4(1.269, 1.25, 1.2510, 0.87);

                GL.Scale(100, 100, 1);
                if (mf.gyd.howManyPathsAway == 0)
                    mf.font.DrawTextVehicle(0, wheelbase + 1, "0", 1);
                else if (mf.gyd.howManyPathsAway > 0) mf.font.DrawTextVehicle(0, wheelbase + 1, mf.gyd.howManyPathsAway.ToString() + "R", 1);
                else mf.font.DrawTextVehicle(0, wheelbase + 1, mf.gyd.howManyPathsAway.ToString() + "L", 1);
                GL.Scale(0.01, 0.01, 1);
            }
            GL.LineWidth(1);

            if (mf.worldManager.camSetDistance > 500)
            {
                //Zoomed out Circle
                GL.PushMatrix();
                GL.Scale(mf.worldManager.camSetDistance * 0.015, mf.worldManager.camSetDistance * 0.015, 1);

                GL.Color4(0.5f, 0.5f, 1.0f, 0.25);
                GL.DrawArrays(PrimitiveType.TriangleFan, 28, 20);

                GL.Color3(0.5f, 1.0f, 0.2f);
                GL.LineWidth(2);
                GL.DrawArrays(PrimitiveType.LineLoop, 28, 20);

                GL.PopMatrix();
            }
        }
    }
}