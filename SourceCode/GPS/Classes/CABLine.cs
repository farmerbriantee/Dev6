using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        public double abHeading, abLength;
        public double angVel;

        public bool isABValid;

        //the current AB guidance line
        public vec3 currentABLineP1 = new vec3(0.0, 0.0, 0.0);
        public vec3 currentABLineP2 = new vec3(0.0, 1.0, 0.0);

        //List of all available ABLines
        public List<CABLines> lineArr = new List<CABLines>();

        public int numABLines, numABLineSelected;

        public bool isABLineBeingSet;
        public bool isABLineSet, isABLineLoaded;
        public bool isBtnABLineOn;

        //the reference line endpoints
        public vec2 refABLineP1 = new vec2(0.0, 0.0);
        public vec2 refABLineP2 = new vec2(0.0, 1.0);

        public double snapDistance;
        public int lineWidth;

        //design
        public vec2 desPoint1 = new vec2(0.2, 0.15);
        public vec2 desPoint2 = new vec2(0.3, 0.3);
        public double desHeading = 0;

        public int tramPassEvery;

        public void BuildCurrentABLine(vec3 pivot)
        {
            if (!isABValid || ((mf.secondsSinceStart - lastSecond) > 0.66 && (!mf.isAutoSteerBtnOn || mf.mc.steerSwitchHigh)))
            {
                double dx, dy;

                lastSecond = mf.secondsSinceStart;

                //move the ABLine over based on the overlap amount set in
                double widthMinusOverlap = mf.tool.toolWidth - mf.tool.toolOverlap;

                //x2-x1
                dx = refABLineP2.easting - refABLineP1.easting;
                //z2-z1
                dy = refABLineP2.northing - refABLineP1.northing;

                distanceFromRefLine = ((dy * mf.guidanceLookPos.easting) - (dx * mf.guidanceLookPos.northing) + (refABLineP2.easting
                                        * refABLineP1.northing) - (refABLineP2.northing * refABLineP1.easting))
                                            / Math.Sqrt((dy * dy) + (dx * dx));

                isLateralTriggered = false;

                isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(pivot.heading - abHeading) - Math.PI) < glm.PIBy2;

                if (isYouTurnTriggered) isHeadingSameWay = !isHeadingSameWay;

                //Which ABLine is the vehicle on, negative is left and positive is right side
                double RefDist = (distanceFromRefLine + (isHeadingSameWay ? mf.tool.toolOffset : -mf.tool.toolOffset)) / widthMinusOverlap;
                if (RefDist < 0) howManyPathsAway = (int)(RefDist - 0.5);
                else howManyPathsAway = (int)(RefDist + 0.5);

                //depending which way you are going, the offset can be either side
                vec2 point1 = new vec2((Math.Cos(-abHeading) * (widthMinusOverlap * howManyPathsAway + (isHeadingSameWay ? -mf.tool.toolOffset : mf.tool.toolOffset))) + refPoint1.easting,
                (Math.Sin(-abHeading) * ((widthMinusOverlap * howManyPathsAway) + (isHeadingSameWay ? -mf.tool.toolOffset : mf.tool.toolOffset))) + refPoint1.northing);

                //create the new line extent points for current ABLine based on original heading of AB line
                currentABLineP1.easting = point1.easting - (Math.Sin(abHeading) * abLength);
                currentABLineP1.northing = point1.northing - (Math.Cos(abHeading) * abLength);

                currentABLineP2.easting = point1.easting + (Math.Sin(abHeading) * abLength);
                currentABLineP2.northing = point1.northing + (Math.Cos(abHeading) * abLength);

                currentABLineP1.heading = abHeading;
                currentABLineP2.heading = abHeading;

                isABValid = true;
            }
        }

        public void GetCurrentABLine(vec3 pivot, vec3 steer)
        {
            //build new current ref line if required
            BuildCurrentABLine(pivot);

            List<vec3> New = new List<vec3> { currentABLineP1, currentABLineP2 };//will be changed to List<> later
            CalculateSteerAngle(pivot, steer, isYouTurnTriggered ? ytList : New);
        }

        public void DrawABLines()
        {
            //Draw AB Points
            GL.PointSize(8.0f);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0.95f, 0.0f, 0.0f);
            GL.Vertex3(refPoint1.easting, refPoint1.northing, 0.0);
            GL.Color3(0.0f, 0.90f, 0.95f);
            GL.Vertex3(refPoint2.easting, refPoint2.northing, 0.0);
            GL.End();

            if (mf.font.isFontOn && !isABLineBeingSet)
            {
                mf.font.DrawText3D(refPoint1.easting, refPoint1.northing, "&A");
                mf.font.DrawText3D(refPoint2.easting, refPoint2.northing, "&B");
            }

            GL.PointSize(1.0f);

            //Draw reference AB line
            GL.LineWidth(lineWidth);
            GL.Enable(EnableCap.LineStipple);
            GL.LineStipple(1, 0x0F00);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(0.930f, 0.2f, 0.2f);
            GL.Vertex3(refABLineP1.easting, refABLineP1.northing, 0);
            GL.Vertex3(refABLineP2.easting, refABLineP2.northing, 0);
            GL.End();
            GL.Disable(EnableCap.LineStipple);

            //draw current AB Line
            GL.LineWidth(lineWidth);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(0.95f, 0.20f, 0.950f);
            GL.Vertex3(currentABLineP1.easting, currentABLineP1.northing, 0.0);
            GL.Vertex3(currentABLineP2.easting, currentABLineP2.northing, 0.0);
            GL.End();

            //ABLine currently being designed
            if (isABLineBeingSet)
            {
                GL.LineWidth(lineWidth);
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(0.95f, 0.20f, 0.950f);
                GL.Vertex3(desPoint1.easting - (Math.Sin(desHeading) * abLength), desPoint1.northing - (Math.Cos(desHeading) * abLength), 0.0);
                GL.Vertex3(desPoint1.easting + (Math.Sin(desHeading) * abLength), desPoint1.northing + (Math.Cos(desHeading) * abLength), 0.0);
                GL.End();

                GL.Color3(0.2f, 0.950f, 0.20f);
                mf.font.DrawText3D(desPoint1.easting, desPoint1.northing, "&A");
                mf.font.DrawText3D(desPoint2.easting, desPoint2.northing, "&B");
            }

            if (mf.isSideGuideLines && mf.camera.camSetDistance > mf.tool.toolWidth * -120)
            {
                //get the tool offset and width
                double toolOffset = mf.tool.toolOffset * 2;
                double toolWidth = mf.tool.toolWidth - mf.tool.toolOverlap;
                double cosHeading = Math.Cos(-abHeading);
                double sinHeading = Math.Sin(-abHeading);

                GL.Color3(0.756f, 0.7650f, 0.7650f);
                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x0303);

                GL.LineWidth(lineWidth);
                GL.Begin(PrimitiveType.Lines);

                /*
                for (double i = -2.5; i < 3; i++)
                {
                    GL.Vertex3((cosHeading * ((mf.tool.toolWidth - mf.tool.toolOverlap) * (howManyPathsAway + i))) + refPoint1.easting, (sinHeading * ((mf.tool.toolWidth - mf.tool.toolOverlap) * (howManyPathsAway + i))) + refPoint1.northing, 0);
                    GL.Vertex3((cosHeading * ((mf.tool.toolWidth - mf.tool.toolOverlap) * (howManyPathsAway + i))) + refPoint2.easting, (sinHeading * ((mf.tool.toolWidth - mf.tool.toolOverlap) * (howManyPathsAway + i))) + refPoint2.northing, 0);
                }
                */

                if (isHeadingSameWay)
                {
                    GL.Vertex3((cosHeading * (toolWidth + toolOffset)) + currentABLineP1.easting, (sinHeading * (toolWidth + toolOffset)) + currentABLineP1.northing, 0);
                    GL.Vertex3((cosHeading * (toolWidth + toolOffset)) + currentABLineP2.easting, (sinHeading * (toolWidth + toolOffset)) + currentABLineP2.northing, 0);
                    GL.Vertex3((cosHeading * (-toolWidth + toolOffset)) + currentABLineP1.easting, (sinHeading * (-toolWidth + toolOffset)) + currentABLineP1.northing, 0);
                    GL.Vertex3((cosHeading * (-toolWidth + toolOffset)) + currentABLineP2.easting, (sinHeading * (-toolWidth + toolOffset)) + currentABLineP2.northing, 0);

                    toolWidth *= 2;
                    GL.Vertex3((cosHeading * toolWidth) + currentABLineP1.easting, (sinHeading * toolWidth) + currentABLineP1.northing, 0);
                    GL.Vertex3((cosHeading * toolWidth) + currentABLineP2.easting, (sinHeading * toolWidth) + currentABLineP2.northing, 0);
                    GL.Vertex3((cosHeading * (-toolWidth)) + currentABLineP1.easting, (sinHeading * (-toolWidth)) + currentABLineP1.northing, 0);
                    GL.Vertex3((cosHeading * (-toolWidth)) + currentABLineP2.easting, (sinHeading * (-toolWidth)) + currentABLineP2.northing, 0);
                }
                else
                {
                    GL.Vertex3((cosHeading * (toolWidth - toolOffset)) + currentABLineP1.easting, (sinHeading * (toolWidth - toolOffset)) + currentABLineP1.northing, 0);
                    GL.Vertex3((cosHeading * (toolWidth - toolOffset)) + currentABLineP2.easting, (sinHeading * (toolWidth - toolOffset)) + currentABLineP2.northing, 0);
                    GL.Vertex3((cosHeading * (-toolWidth - toolOffset)) + currentABLineP1.easting, (sinHeading * (-toolWidth - toolOffset)) + currentABLineP1.northing, 0);
                    GL.Vertex3((cosHeading * (-toolWidth - toolOffset)) + currentABLineP2.easting, (sinHeading * (-toolWidth - toolOffset)) + currentABLineP2.northing, 0);

                    toolWidth *= 2;
                    GL.Vertex3((cosHeading * toolWidth) + currentABLineP1.easting, (sinHeading * toolWidth) + currentABLineP1.northing, 0);
                    GL.Vertex3((cosHeading * toolWidth) + currentABLineP2.easting, (sinHeading * toolWidth) + currentABLineP2.northing, 0);
                    GL.Vertex3((cosHeading * (-toolWidth)) + currentABLineP1.easting, (sinHeading * (-toolWidth)) + currentABLineP1.northing, 0);
                    GL.Vertex3((cosHeading * (-toolWidth)) + currentABLineP2.easting, (sinHeading * (-toolWidth)) + currentABLineP2.northing, 0);
                }

                GL.End();
                GL.Disable(EnableCap.LineStipple);
            }

            if (!mf.isStanleyUsed && mf.camera.camSetDistance > -200)
            {
                //Draw lookahead Point
                GL.PointSize(8.0f);
                GL.Begin(PrimitiveType.Points);
                GL.Color3(1.0f, 1.0f, 0.0f);
                GL.Vertex3(goalPoint.easting, goalPoint.northing, 0.0);
                //GL.Vertex3(rEastSteer, rNorthSteer, 0.0);
                //GL.Vertex3(rEastPivot, rNorthPivot, 0.0);
                GL.End();
                GL.PointSize(1.0f);

                if (ppRadius < 50 && ppRadius > -50)
                {
                    const int numSegments = 100;
                    double theta = glm.twoPI / numSegments;
                    double c = Math.Cos(theta);//precalculate the sine and cosine
                    double s = Math.Sin(theta);
                    double x = ppRadius;//we start at angle = 0
                    double y = 0;

                    GL.LineWidth(1);
                    GL.Color3(0.53f, 0.530f, 0.950f);
                    GL.Begin(PrimitiveType.LineLoop);
                    for (int ii = 0; ii < numSegments; ii++)
                    {
                        //glVertex2f(x + cx, y + cy);//output vertex
                        GL.Vertex3(x + radiusPoint.easting, y + radiusPoint.northing, 0);//output vertex
                        double t = x;//apply the rotation matrix
                        x = (c * x) - (s * y);
                        y = (s * t) + (c * y);
                    }
                    GL.End();
                }
            }

            DrawYouTurn();

            GL.PointSize(1.0f);
            GL.LineWidth(1);
        }

        public void BuildTram()
        {
            mf.tram.BuildTramBnd();

            mf.tram.tramList?.Clear();
            mf.tram.tramArr?.Clear();
            List<vec2> tramRef = new List<vec2>();

            bool isBndExist = mf.bnd.bndList.Count != 0;

            double pass = 0.5;
            double hsin = Math.Sin(abHeading);
            double hcos = Math.Cos(abHeading);

            //divide up the AB line into segments
            vec2 P1 = new vec2();
            for (int i = 0; i < 3200; i += 4)
            {
                P1.easting = (hsin * i) + refABLineP1.easting;
                P1.northing = (hcos * i) + refABLineP1.northing;
                tramRef.Add(P1);
            }

            //create list of list of points of triangle strip of AB Highlight
            double headingCalc = abHeading + glm.PIBy2;
            hsin = Math.Sin(headingCalc);
            hcos = Math.Cos(headingCalc);

            mf.tram.tramList?.Clear();
            mf.tram.tramArr?.Clear();

            //no boundary starts on first pass
            int cntr = 0;
            if (isBndExist) cntr = 1;

            for (int i = cntr; i < mf.tram.passes; i++)
            {
                mf.tram.tramArr = new List<vec2>
                {
                    Capacity = 128
                };

                mf.tram.tramList.Add(mf.tram.tramArr);

                for (int j = 0; j < tramRef.Count; j++)
                {
                    P1.easting = (hsin * ((mf.tram.tramWidth * (pass + i)) - mf.tram.halfWheelTrack + mf.tool.halfToolWidth)) + tramRef[j].easting;
                    P1.northing = (hcos * ((mf.tram.tramWidth * (pass + i)) - mf.tram.halfWheelTrack + mf.tool.halfToolWidth)) + tramRef[j].northing;

                    if (!isBndExist || mf.bnd.bndList[0].fenceLine.points.IsPointInPolygon(P1))
                    {
                        mf.tram.tramArr.Add(P1);
                    }
                }
            }

            for (int i = cntr; i < mf.tram.passes; i++)
            {
                mf.tram.tramArr = new List<vec2>
                {
                    Capacity = 128
                };

                mf.tram.tramList.Add(mf.tram.tramArr);

                for (int j = 0; j < tramRef.Count; j++)
                {
                    P1.easting = (hsin * ((mf.tram.tramWidth * (pass + i)) + mf.tram.halfWheelTrack + mf.tool.halfToolWidth)) + tramRef[j].easting;
                    P1.northing = (hcos * ((mf.tram.tramWidth * (pass + i)) + mf.tram.halfWheelTrack + mf.tool.halfToolWidth)) + tramRef[j].northing;

                    if (!isBndExist || mf.bnd.bndList[0].fenceLine.points.IsPointInPolygon(P1))
                    {
                        mf.tram.tramArr.Add(P1);
                    }
                }
            }

            tramRef?.Clear();
            //outside tram

            if (mf.bnd.bndList.Count == 0 || mf.tram.passes != 0)
            {
                //return;
            }
        }

        public void DeleteAB()
        {
            refPoint1 = new vec2(0.0, 0.0);
            refPoint2 = new vec2(0.0, 1.0);

            refABLineP1 = new vec2(0.0, 0.0);
            refABLineP2 = new vec2(0.0, 1.0);

            currentABLineP1 = new vec3(0.0, 0.0, 0.0);
            currentABLineP2 = new vec3(0.0, 1.0, 0.0);

            abHeading = 0.0;
            howManyPathsAway = 0.0;
            isABLineSet = false;
            isABLineLoaded = false;
        }

        public void SetABLineByHeading()
        {
            //heading is set in the AB Form
            refABLineP1.easting = refPoint1.easting - (Math.Sin(abHeading) * abLength);
            refABLineP1.northing = refPoint1.northing - (Math.Cos(abHeading) * abLength);

            refABLineP2.easting = refPoint1.easting + (Math.Sin(abHeading) * abLength);
            refABLineP2.northing = refPoint1.northing + (Math.Cos(abHeading) * abLength);

            refPoint2.easting = refABLineP2.easting;
            refPoint2.northing = refABLineP2.northing;

            isABLineSet = true;
            isABLineLoaded = true;
        }

        public void MoveABLine(double dist)
        {
            moveDistance += isHeadingSameWay ? dist : -dist;

            //calculate the new points for the reference line and points
            refPoint1.easting += Math.Cos(abHeading) * (isHeadingSameWay ? dist : -dist);
            refPoint1.northing -= Math.Sin(abHeading) * (isHeadingSameWay ? dist : -dist);

            refABLineP1.easting = refPoint1.easting - (Math.Sin(abHeading) * abLength);
            refABLineP1.northing = refPoint1.northing - (Math.Cos(abHeading) * abLength);

            refABLineP2.easting = refPoint1.easting + (Math.Sin(abHeading) * abLength);
            refABLineP2.northing = refPoint1.northing + (Math.Cos(abHeading) * abLength);

            refPoint2.easting = refABLineP2.easting;
            refPoint2.northing = refABLineP2.northing;

            isABValid = false;
        }
    }

    public class CABLines
    {
        public vec2 origin = new vec2();
        public double heading = 0;
        public string Name = "aa";
    }
}