using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

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

                double distAway = widthMinusOverlap * howManyPathsAway + (isHeadingSameWay ? -mf.tool.toolOffset : mf.tool.toolOffset);

                curList = BuildOffsetList(refList, distAway);
                isValid = true;
            }
        }

        public List<vec3> BuildOffsetList(CGuidanceLine refList, double distAway)
        {
            //move the ABLine over based on the overlap amount set in vehicle
            double distSqAway = (distAway * distAway) - 0.01;

            List<vec3> buildList = new List<vec3>();
            if (refList.mode.HasFlag(Mode.AB))
            {
                double east = Math.Sin(refList.curvePts[0].heading) * abLength;
                double east2 = Math.Cos(refList.curvePts[0].heading) * distAway;
                double north = Math.Cos(refList.curvePts[0].heading) * abLength;
                double north2 = Math.Sin(refList.curvePts[0].heading) * distAway;

                buildList.Add(new vec3(refList.curvePts[0].easting - east + east2, refList.curvePts[0].northing - north + north2, refList.curvePts[0].heading));
                buildList.Add(new vec3(refList.curvePts[0].easting + east + east2, refList.curvePts[0].northing + north + north2, refList.curvePts[0].heading));
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
                        if (buildList.Count > 0)
                        {
                            double dist = ((point.easting - buildList[buildList.Count - 1].easting) * (point.easting - buildList[buildList.Count - 1].easting))
                                + ((point.northing - buildList[buildList.Count - 1].northing) * (point.northing - buildList[buildList.Count - 1].northing));
                            if (dist > 1)
                                buildList.Add(point);
                        }
                        else buildList.Add(point);
                    }
                }

                int cnt = buildList.Count;
                if (cnt > 6)
                {
                    vec3[] arr = new vec3[cnt];
                    buildList.CopyTo(arr);

                    for (int i = 1; i < (buildList.Count - 1); i++)
                    {
                        arr[i].easting = (buildList[i - 1].easting + buildList[i].easting + buildList[i + 1].easting) / 3;
                        arr[i].northing = (buildList[i - 1].northing + buildList[i].northing + buildList[i + 1].northing) / 3;
                    }
                    buildList.Clear();

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
                    buildList.Add(arr[0]);
                    buildList.Add(arr[1]);

                    for (int i = 0; i < cnt - 3; i++)
                    {
                        // add p1
                        buildList.Add(arr[i + 1]);

                        distance = glm.Distance(arr[i + 1], arr[i + 2]);

                        if (distance > spacing)
                        {
                            int loopTimes = (int)(distance / spacing + 1);
                            for (int j = 1; j < loopTimes; j++)
                            {
                                vec3 pos = new vec3(glm.Catmull(j / (double)(loopTimes), arr[i], arr[i + 1], arr[i + 2], arr[i + 3]));
                                buildList.Add(pos);
                            }
                        }
                    }

                    buildList.Add(arr[cnt - 2]);
                    buildList.Add(arr[cnt - 1]);

                    buildList.CalculateHeadings(false);
                }
            }
            return buildList;
        }

        public void DrawGuidanceLines()
        {
            GL.LineWidth(lineWidth);

            if (mf.panelDrag.Visible && recList.Count > 0)
            {
                GL.Color3(0.98f, 0.92f, 0.460f);
                GL.Begin(PrimitiveType.LineStrip);
                for (int h = 0; h < recList.Count; h++) GL.Vertex3(recList[h].easting, recList[h].northing, 0);
                GL.End();

                if (!isRecordOn)
                {
                    //Draw lookahead Point
                    GL.PointSize(16.0f);
                    GL.Begin(PrimitiveType.Points);

                    GL.Color3(1.0f, 0.5f, 0.95f);
                    GL.Vertex3(recList[currentPositonIndex].easting, recList[currentPositonIndex].northing, 0);
                    GL.End();
                    GL.PointSize(1.0f);
                }
            }
            else
            {
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

                if (currentGuidanceLine != null && currentGuidanceLine.curvePts.Count > 1)
                {
                    GL.Color3(0.96, 0.2f, 0.2f);
                    GL.Enable(EnableCap.LineStipple);
                    GL.LineStipple(1, 0x0F00);
                    bool Extend = false;
                    if (currentGuidanceLine.mode.HasFlag(Mode.Contour))
                    {
                        if (isLocked)
                            GL.Color3(0.983f, 0.2f, 0.20f);
                        else
                            GL.Color3(0.3f, 0.982f, 0.0f);
                        GL.Begin(PrimitiveType.Points);
                    }
                    else if (currentGuidanceLine.mode.HasFlag(Mode.Boundary))
                        GL.Begin(PrimitiveType.LineLoop);
                    else
                    {
                        Extend = true;
                        GL.Begin(PrimitiveType.LineStrip);
                    }

                    for (int h = 0; h < currentGuidanceLine.curvePts.Count; h++)
                    {
                        if (Extend && h == 0)
                            GL.Vertex3(currentGuidanceLine.curvePts[h].easting - (Math.Sin(currentGuidanceLine.curvePts[h].heading) * abLength), currentGuidanceLine.curvePts[h].northing - (Math.Cos(currentGuidanceLine.curvePts[h].heading) * abLength), 0.0);
                        
                        GL.Vertex3(currentGuidanceLine.curvePts[h].easting, currentGuidanceLine.curvePts[h].northing, 0);

                        if (Extend && h == currentGuidanceLine.curvePts.Count -1)
                            GL.Vertex3(currentGuidanceLine.curvePts[h].easting + (Math.Sin(currentGuidanceLine.curvePts[h].heading) * abLength), currentGuidanceLine.curvePts[h].northing + (Math.Cos(currentGuidanceLine.curvePts[h].heading) * abLength), 0.0);
                    }
                    GL.End();
                    GL.Disable(EnableCap.LineStipple);
                }

                if (curList.Count > 1)
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

                    GL.Color3(0.95f, 0.2f, 0.95f);
                    if (currentGuidanceLine?.mode.HasFlag(Mode.Boundary) == true)
                        GL.Begin(PrimitiveType.LineLoop);
                    else
                        GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < curList.Count; h++)
                        GL.Vertex3(curList[h].easting, curList[h].northing, 0);
                    GL.End();

                    if (currentGuidanceLine?.mode.HasFlag(Mode.Contour) == true)
                    {
                        GL.Begin(PrimitiveType.Points);
                        GL.Color3(0.87f, 08.7f, 0.25f);
                        for (int h = 0; h < curList.Count; h++)
                            GL.Vertex3(curList[h].easting, curList[h].northing, 0);
                        GL.End();
                    }

                    if (mf.isPureDisplayOn && !mf.isStanleyUsed)
                    {
                        //Draw lookahead Point
                        GL.PointSize(8.0f);
                        GL.Begin(PrimitiveType.Points);
                        GL.Color3(1.0f, 0.95f, 0.195f);
                        GL.Vertex3(goalPoint.easting, goalPoint.northing, 0.0);
                        GL.End();
                    }

                    if (OffsetList.Count > 1)
                    {

                        GL.Begin(PrimitiveType.LineStrip);
                        for (int i = 0; i < OffsetList.Count; i++)
                        {
                            GL.Vertex3(OffsetList[i].easting, OffsetList[i].northing, 0);
                        }
                        GL.End();
                    }

                    if (ytList.Count > 1)
                    {
                        GL.PointSize(lineWidth*2);

                        if (isYouTurnTriggered)
                            GL.Color3(0.95f, 0.5f, 0.95f);
                        else if (isOutOfBounds)
                            GL.Color3(0.9495f, 0.395f, 0.325f);
                        else
                            GL.Color3(0.395f, 0.925f, 0.30f);

                        GL.Begin(PrimitiveType.Points);
                        for (int i = 0; i < ytList.Count; i++)
                        {
                            GL.Vertex3(ytList[i].easting, ytList[i].northing, 0);
                        }
                        GL.End();
                    }
                }
            }
            GL.PointSize(lineWidth);
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



