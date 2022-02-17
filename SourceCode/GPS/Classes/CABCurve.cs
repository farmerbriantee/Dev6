using OpenTK.Graphics.OpenGL;
using System;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        public void GetCurrentGuidanceLine(vec3 pivot, vec3 steer)
        {
            if (isDrivingRecordedPath)
                UpdatePosition();
            else
            {
                if (isContourBtnOn)
                    BuildCurrentContourLine(pivot);
                else if (currentGuidanceLine != null)
                    BuildCurrentCurveLine(pivot, currentGuidanceLine);
                else return;

                CalculateSteerAngle(pivot, steer, isYouTurnTriggered ? ytList : curList);
            }
        }

        public void BuildCurrentCurveLine(vec3 pivot, CGuidanceLine refList)
        {
            //move the ABLine over based on the overlap amount set in vehicle
            double widthMinusOverlap = mf.tool.toolWidth - mf.tool.toolOverlap;

            if (!isValid || ((mf.secondsSinceStart - lastSecond) > 0.66 && (!mf.isAutoSteerBtnOn || mf.mc.steerSwitchHigh)))
            {
                double minDistA = double.MaxValue, minDistB;

                int refCount = refList.curvePts.Count;
                if (refList.curvePts.Count < 2)
                {
                    curList?.Clear();
                    return;
                }

                if (!isContourBtnOn)
                {
                    //guidance look ahead distance based on time or tool width at least 
                    double guidanceLookDist = Math.Max(mf.tool.toolWidth * 0.5, mf.avgSpeed * 0.277777 * mf.guidanceLookAheadTime);
                    pivot = new vec3(mf.pivotAxlePos.easting + (Math.Sin(mf.pivotAxlePos.heading) * guidanceLookDist),
                                                    mf.pivotAxlePos.northing + (Math.Cos(mf.pivotAxlePos.heading) * guidanceLookDist), pivot.heading);
                }

                //close call hit
                int cc = 0, dd;

                for (int j = 0; j < refCount; j += 10)
                {
                    double dist = ((pivot.easting - refList.curvePts[j].easting) * (pivot.easting - refList.curvePts[j].easting))
                                    + ((pivot.northing - refList.curvePts[j].northing) * (pivot.northing - refList.curvePts[j].northing));
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        cc = j;
                    }
                }

                minDistA = minDistB = double.MaxValue;

                dd = cc + 7; if (dd > refCount - 1) dd = refCount;
                cc -= 7; if (cc < 0) cc = 0;

                //find the closest 2 points to current close call
                for (int j = cc; j < dd; j++)
                {
                    double dist = ((pivot.easting - refList.curvePts[j].easting) * (pivot.easting - refList.curvePts[j].easting))
                                    + ((pivot.northing - refList.curvePts[j].northing) * (pivot.northing - refList.curvePts[j].northing));
                    if (dist < minDistA)
                    {
                        minDistB = minDistA;
                        rB = rA;
                        minDistA = dist;
                        rA = j;
                    }
                    else if (dist < minDistB)
                    {
                        minDistB = dist;
                        rB = j;
                    }
                }

                if (rA > rB) { int C = rA; rA = rB; rB = C; }

                //same way as line creation or not
                isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(pivot.heading - refList.curvePts[rA].heading) - Math.PI) < glm.PIBy2;

                if (isYouTurnTriggered) isHeadingSameWay = !isHeadingSameWay;

                //x2-x1
                double dx = refList.curvePts[rB].easting - refList.curvePts[rA].easting;
                //z2-z1
                double dz = refList.curvePts[rB].northing - refList.curvePts[rA].northing;

                //how far are we away from the reference line at 90 degrees - 2D cross product and distance
                distanceFromRefLine = ((dz * pivot.easting) - (dx * pivot.northing) + (refList.curvePts[rB].easting
                                    * refList.curvePts[rA].northing) - (refList.curvePts[rB].northing * refList.curvePts[rA].easting))
                                    / Math.Sqrt((dz * dz) + (dx * dx));

                double RefDist = (distanceFromRefLine + (isHeadingSameWay ? mf.tool.toolOffset : -mf.tool.toolOffset)) / widthMinusOverlap;
                if (RefDist < 0) howManyPathsAway = (int)(RefDist - 0.5);
                else howManyPathsAway = (int)(RefDist + 0.5);

                lastSecond = mf.secondsSinceStart;
                if (!isValid) oldIsHeadingSameWay = !isHeadingSameWay;
            }

            if (howManyPathsAway != oldHowManyPathsAway || oldIsHeadingSameWay != isHeadingSameWay)
            {
                oldHowManyPathsAway = howManyPathsAway;
                oldIsHeadingSameWay = isHeadingSameWay;


                //build current list
                isValid = true;

                //build the current line
                curList?.Clear();

                double distAway = widthMinusOverlap * howManyPathsAway + (isHeadingSameWay ? -mf.tool.toolOffset : mf.tool.toolOffset);

                double distSqAway = (distAway * distAway) - 0.01;

                if (refList.mode.HasFlag(Mode.AB))
                {
                    //depending which way you are going, the offset can be either side
                    vec2 point1 = new vec2((Math.Cos(-refList.curvePts[0].heading) * (widthMinusOverlap * howManyPathsAway + (isHeadingSameWay ? -mf.tool.toolOffset : mf.tool.toolOffset))) + refList.curvePts[0].easting,
                    (Math.Sin(-refList.curvePts[0].heading) * ((widthMinusOverlap * howManyPathsAway) + (isHeadingSameWay ? -mf.tool.toolOffset : mf.tool.toolOffset))) + refList.curvePts[0].northing);

                    curList.Add(new vec3(point1.easting - (Math.Sin(refList.curvePts[0].heading) * abLength), point1.northing - (Math.Cos(refList.curvePts[0].heading) * abLength), refList.curvePts[0].heading));
                    curList.Add(new vec3(point1.easting + (Math.Sin(refList.curvePts[0].heading) * abLength), point1.northing + (Math.Cos(refList.curvePts[0].heading) * abLength), refList.curvePts[0].heading));
                }
                else
                {
                    for (int i = 0; i < refList.curvePts.Count; i++)
                    {
                        vec3 point = new vec3(
                        refList.curvePts[i].easting + (Math.Sin(glm.PIBy2 + refList.curvePts[i].heading) * distAway),
                        refList.curvePts[i].northing + (Math.Cos(glm.PIBy2 + refList.curvePts[i].heading) * distAway),
                        refList.curvePts[i].heading);
                        bool Add = true;
                        for (int t = 0; t < refList.curvePts.Count; t++)
                        {
                            double dist = ((point.easting - refList.curvePts[t].easting) * (point.easting - refList.curvePts[t].easting))
                                + ((point.northing - refList.curvePts[t].northing) * (point.northing - refList.curvePts[t].northing));
                            if (dist < distSqAway)
                            {
                                Add = false;
                                break;
                            }
                        }
                        if (Add)
                        {
                            if (curList.Count > 0)
                            {
                                double dist = ((point.easting - curList[curList.Count - 1].easting) * (point.easting - curList[curList.Count - 1].easting))
                                    + ((point.northing - curList[curList.Count - 1].northing) * (point.northing - curList[curList.Count - 1].northing));
                                if (dist > 1)
                                    curList.Add(point);
                            }
                            else curList.Add(point);
                        }
                    }

                    int cnt = curList.Count;
                    if (cnt > 6)
                    {
                        vec3[] arr = new vec3[cnt];
                        curList.CopyTo(arr);

                        for (int i = 1; i < (curList.Count - 1); i++)
                        {
                            arr[i].easting = (curList[i - 1].easting + curList[i].easting + curList[i + 1].easting) / 3;
                            arr[i].northing = (curList[i - 1].northing + curList[i].northing + curList[i + 1].northing) / 3;
                        }
                        curList.Clear();

                        for (int i = 0; i < (arr.Length - 1); i++)
                        {
                            arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                            if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                            if (arr[i].heading >= glm.twoPI) arr[i].heading -= glm.twoPI;
                        }

                        arr[arr.Length - 1].heading = arr[arr.Length - 2].heading;


                        if (mf.tool.isToolTrailing)
                        {
                            //depending on hitch is different profile of draft
                            double hitch;
                            if (mf.tool.isToolTBT && mf.tool.toolTankTrailingHitchLength < 0)
                            {
                                hitch = mf.tool.toolTankTrailingHitchLength * 0.85;
                                hitch += mf.tool.toolTrailingHitchLength * 0.65;
                            }
                            else hitch = mf.tool.toolTrailingHitchLength * 1.0;// - mf.vehicle.wheelbase;

                            //move the line forward based on hitch length ratio
                            for (int i = 0; i < arr.Length; i++)
                            {
                                arr[i].easting -= Math.Sin(arr[i].heading) * (hitch);
                                arr[i].northing -= Math.Cos(arr[i].heading) * (hitch);
                            }

                            ////average the points over 3, center weighted
                            //for (int i = 1; i < arr.Length - 2; i++)
                            //{
                            //    arr2[i].easting = (arr[i - 1].easting + arr[i].easting + arr[i + 1].easting) / 3;
                            //    arr2[i].northing = (arr[i - 1].northing + arr[i].northing + arr[i + 1].northing) / 3;
                            //}

                            //recalculate the heading
                            for (int i = 0; i < (arr.Length - 1); i++)
                            {
                                arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                                if (arr[i].heading >= glm.twoPI) arr[i].heading -= glm.twoPI;
                            }

                            arr[arr.Length - 1].heading = arr[arr.Length - 2].heading;
                        }

                        //replace the array 
                        //curList.AddRange(arr);
                        cnt = arr.Length;
                        double distance;
                        double spacing = 0.5;

                        //add the first point of loop - it will be p1
                        curList.Add(arr[0]);
                        curList.Add(arr[1]);

                        for (int i = 0; i < cnt - 3; i++)
                        {
                            // add p1
                            curList.Add(arr[i + 1]);

                            distance = glm.Distance(arr[i + 1], arr[i + 2]);

                            if (distance > spacing)
                            {
                                int loopTimes = (int)(distance / spacing + 1);
                                for (int j = 1; j < loopTimes; j++)
                                {
                                    vec3 pos = new vec3(glm.Catmull(j / (double)(loopTimes), arr[i], arr[i + 1], arr[i + 2], arr[i + 3]));
                                    curList.Add(pos);
                                }
                            }
                        }

                        curList.Add(arr[cnt - 2]);
                        curList.Add(arr[cnt - 1]);

                        curList.CalculateHeadings(false);
                    }
                }
            }
        }

        public void DrawCurve()
        {
            GL.LineWidth(lineWidth);

            if (EditGuidanceLine != null)
            {
                if (isSmoothWindowOpen)
                {
                    GL.Color3(0.930f, 0.92f, 0.260f);
                    GL.Begin(PrimitiveType.Lines);
                }
                else
                {
                    GL.Color3(0.95f, 0.42f, 0.750f);
                    GL.Begin(PrimitiveType.LineStrip);
                }

                for (int h = 0; h < EditGuidanceLine.curvePts.Count; h++)
                {
                    if (h == 0 && !EditGuidanceLine.mode.HasFlag(Mode.Boundary))
                        GL.Vertex3(EditGuidanceLine.curvePts[h].easting - (Math.Sin(EditGuidanceLine.curvePts[h].heading) * abLength), EditGuidanceLine.curvePts[h].northing - (Math.Cos(EditGuidanceLine.curvePts[h].heading) * abLength), 0.0);

                    GL.Vertex3(EditGuidanceLine.curvePts[h].easting, EditGuidanceLine.curvePts[h].northing, 0);

                    if (h == EditGuidanceLine.curvePts.Count - 1 && !EditGuidanceLine.mode.HasFlag(Mode.Boundary))
                        GL.Vertex3(EditGuidanceLine.curvePts[h].easting + (Math.Sin(EditGuidanceLine.curvePts[h].heading) * abLength), EditGuidanceLine.curvePts[h].northing + (Math.Cos(EditGuidanceLine.curvePts[h].heading) * abLength), 0.0);
                }

                if (!EditGuidanceLine.mode.HasFlag(Mode.AB) || EditGuidanceLine.curvePts.Count == 1)
                {
                    GL.Color3(0.930f, 0.0692f, 0.260f);
                    GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
                }
                GL.End();
            }

            if (currentGuidanceLine != null)
            {
                GL.Color3(0.96, 0.2f, 0.2f);
                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x0F00);

                if (currentGuidanceLine.mode.HasFlag(Mode.Boundary))
                    GL.Begin(PrimitiveType.LineLoop);
                else
                    GL.Begin(PrimitiveType.LineStrip);

                for (int h = 0; h < currentGuidanceLine.curvePts.Count; h++)
                    GL.Vertex3(currentGuidanceLine.curvePts[h].easting, currentGuidanceLine.curvePts[h].northing, 0);
                GL.End();

                GL.Disable(EnableCap.LineStipple);
            }

            if (curList.Count > 0)
            {
                if (mf.isSideGuideLines && mf.camera.camSetDistance > mf.tool.toolWidth * -120)
                {
                    //get the tool offset and width
                    double toolOffset = mf.tool.toolOffset * 2;
                    double toolWidth = mf.tool.toolWidth - mf.tool.toolOverlap;
                    //double cosHeading = Math.Cos(-abHeading);
                    //double sinHeading = Math.Sin(-abHeading);

                    GL.Color3(0.756f, 0.7650f, 0.7650f);
                    GL.Enable(EnableCap.LineStipple);
                    GL.LineStipple(1, 0x0303);

                    GL.LineWidth(lineWidth);
                    GL.Begin(PrimitiveType.Lines);
                    toolOffset = toolOffset;

                    /*
                    for (double i = -2.5; i < 3; i++)
                    {
                        GL.Vertex3((cosHeading * ((mf.tool.toolWidth - mf.tool.toolOverlap) * (howManyPathsAway + i))) + refPoint1.easting, (sinHeading * ((mf.tool.toolWidth - mf.tool.toolOverlap) * (howManyPathsAway + i))) + refPoint1.northing, 0);
                        GL.Vertex3((cosHeading * ((mf.tool.toolWidth - mf.tool.toolOverlap) * (howManyPathsAway + i))) + refPoint2.easting, (sinHeading * ((mf.tool.toolWidth - mf.tool.toolOverlap) * (howManyPathsAway + i))) + refPoint2.northing, 0);
                    }

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
                    */

                    GL.End();
                    GL.Disable(EnableCap.LineStipple);
                }

                GL.PointSize(2);

                GL.Color3(0.95f, 0.2f, 0.95f);
                GL.Begin(PrimitiveType.LineStrip);
                for (int h = 0; h < curList.Count; h++)
                    GL.Vertex3(curList[h].easting, curList[h].northing, 0);
                GL.End();

                if (mf.isPureDisplayOn && !mf.isStanleyUsed)
                {
                    if (currentGuidanceLine.mode.HasFlag(Mode.AB) && ppRadius < 150 && ppRadius > -150)
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

                    //Draw lookahead Point
                    GL.PointSize(8.0f);
                    GL.Begin(PrimitiveType.Points);
                    GL.Color3(1.0f, 0.95f, 0.195f);
                    GL.Vertex3(goalPoint.easting, goalPoint.northing, 0.0);
                    GL.End();
                }

                int ptCount = ytList.Count;
                if (ptCount < 3) return;
                GL.PointSize(lineWidth);

                if (isYouTurnTriggered)
                    GL.Color3(0.95f, 0.5f, 0.95f);
                else if (isOutOfBounds)
                    GL.Color3(0.9495f, 0.395f, 0.325f);
                else
                    GL.Color3(0.395f, 0.925f, 0.30f);

                GL.Begin(PrimitiveType.Points);
                for (int i = 0; i < ptCount; i++)
                {
                    GL.Vertex3(ytList[i].easting, ytList[i].northing, 0);
                }
                GL.End();
            }
            GL.PointSize(1.0f);
        }

        public void MoveGuidanceLine(CGuidanceLine GuidanceLine, double dist)
        {
            isValid = false;

            lastSecond = 0;
            if (GuidanceLine != null)
            {
                int cnt = GuidanceLine.curvePts.Count;
                vec3[] arr = new vec3[cnt];
                GuidanceLine.curvePts.CopyTo(arr);
                GuidanceLine.curvePts.Clear();

                moveDistance += isHeadingSameWay ? dist : -dist;

                for (int i = 0; i < cnt; i++)
                {
                    arr[i].easting += Math.Cos(arr[i].heading) * (isHeadingSameWay ? dist : -dist);
                    arr[i].northing -= Math.Sin(arr[i].heading) * (isHeadingSameWay ? dist : -dist);
                    GuidanceLine.curvePts.Add(arr[i]);
                }
            }
        }

        public void ReverseGuidanceLine(CGuidanceLine guidanceLine)
        {
            if (guidanceLine != null)
            {
                int cnt = guidanceLine.curvePts.Count;
                if (cnt > 1)
                {
                    if (guidanceLine.mode.HasFlag(Mode.AB))
                    {
                        double heading = Math.Atan2(guidanceLine.curvePts[0].easting - guidanceLine.curvePts[1].easting,
                           guidanceLine.curvePts[0].northing - guidanceLine.curvePts[1].northing);

                        guidanceLine.curvePts[0] = new vec3(guidanceLine.curvePts[0].easting, guidanceLine.curvePts[0].northing, heading);
                        guidanceLine.curvePts[1] = new vec3(guidanceLine.curvePts[0].easting + Math.Sin(heading), guidanceLine.curvePts[0].northing + Math.Cos(heading), heading);
                    }
                    else
                    {
                        guidanceLine.curvePts.Reverse();
                        guidanceLine.curvePts.CalculateHeadings(guidanceLine.mode.HasFlag(Mode.Boundary));
                    }
                }
            }
        }
    }
}



