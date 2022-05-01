using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        public void GetCurrentGuidanceLine(vec2 pivot, vec2 steer, double heading)
        {
            if (CurrentGMode == Mode.RecPath)
                UpdatePosition(pivot, steer, heading);
            else
            {
                if (CurrentGMode == Mode.Contour)
                    FindCurrentContourLine(pivot);

                if (currentGuidanceLine != null)
                    BuildCurrentCurveLine(pivot, heading, currentGuidanceLine);
                else
                {
                    curList.Clear();
                    isLocked = false;
                    return;
                }
                
                CalculateSteerAngle(pivot, steer, heading, isYouTurnTriggered ? ytList : curList, currentGuidanceLine.mode.HasFlag(Mode.Boundary) && !isYouTurnTriggered);
            }
        }

        public void FindCurrentContourLine(vec2 pivot)
        {
            if ((mf.secondsSinceStart - lastSecondSearch) < (curList.Count == 0 ? 0.3 : 2.0)) return;

            lastSecondSearch = mf.secondsSinceStart;
            int ptCount;
            double minDistA = double.MaxValue;

            if (!isLocked)
            {
                int stripNum = -1;
                for (int s = 0; s < curveArr.Count; s++)
                {
                    if (curveArr[s].mode.HasFlag(Mode.Contour))
                    {
                        ptCount = curveArr[s].curvePts.Count - (curveArr[s] == creatingContour ? backSpacing : 1);
                        if (ptCount < 1) continue;
                        double dist;
                        for (int p = 0; p < ptCount; p += 6)
                        {
                            dist = glm.Distance(pivot, curveArr[s].curvePts[p]);
                            if (dist < minDistA)
                            {
                                minDistA = dist;
                                stripNum = s;
                            }
                        }

                        //catch the last point
                        dist = glm.Distance(pivot, curveArr[s].curvePts[ptCount]);
                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            stripNum = s;
                        }
                    }
                }

                if (stripNum < 0)
                {
                    isValid = false;
                    moveDistance = 0;
                    currentGuidanceLine = null;
                }
                else if (currentGuidanceLine != curveArr[stripNum])
                {
                    isValid = false;
                    moveDistance = 0;
                    currentGuidanceLine = curveArr[stripNum];
                }
            }
        }

        public void BuildCurrentCurveLine(vec2 pivot, double heading, CGuidanceLine refList)
        {
            //move the ABLine over based on the overlap amount set in vehicle
            double widthMinusOverlap = mf.tool.toolWidth - mf.tool.toolOverlap;
            
            if (!isValid || ((mf.secondsSinceStart - lastSecond) > 0.66 && (!mf.isAutoSteerBtnOn || mf.mc.steerSwitchHigh || refList == creatingContour)))
            {
                int refCount = refList.curvePts.Count - (refList == creatingContour ? backSpacing : 0);
                if (refList.curvePts.Count < 2)
                {
                    curList.Clear();
                    isLocked = false;
                    return;
                }

                if (!refList.mode.HasFlag(Mode.Contour))
                {
                    //guidance look ahead distance based on time or tool width at least 
                    double guidanceLookDist = Math.Max(mf.tool.toolWidth * 0.5, mf.mc.avgSpeed * 0.277777 * mf.guidanceLookAheadTime);
                    pivot = new vec2(pivot.easting + (Math.Sin(heading) * guidanceLookDist),
                                                    pivot.northing + (Math.Cos(heading) * guidanceLookDist));
                }

                pivot.GetCurrentSegment(refList.curvePts, 0, refList.mode.HasFlag(Mode.Boundary), out rA, out rB, refCount);
                if (rA < 0 || rB < 0)
                {
                    curList.Clear();
                    isLocked = false;
                    return;
                }

                //same way as line creation or not
                isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(heading - refList.curvePts[rA].heading) - Math.PI) < glm.PIBy2;

                //x2-x1
                double dx = refList.curvePts[rB].easting - refList.curvePts[rA].easting;
                //z2-z1
                double dz = refList.curvePts[rB].northing - refList.curvePts[rA].northing;

                //how far are we away from the reference line at 90 degrees - 2D cross product and distance
                distanceFromRefLine = ((dz * pivot.easting) - (dx * pivot.northing) + (refList.curvePts[rB].easting
                                    * refList.curvePts[rA].northing) - (refList.curvePts[rB].northing * refList.curvePts[rA].easting))
                                    / Math.Sqrt((dz * dz) + (dx * dx));

                double RefDist = (distanceFromRefLine + (isHeadingSameWay ? mf.tool.toolOffset : -mf.tool.toolOffset)) / widthMinusOverlap;
                if (double.IsInfinity(RefDist))
                    howManyPathsAway = 0;
                else if (RefDist < 0) howManyPathsAway = (int)(RefDist - 0.5);
                else howManyPathsAway = (int)(RefDist + 0.5);

                lastSecond = mf.secondsSinceStart;
            }

            if (refList?.mode.HasFlag(Mode.Contour) == true)
            {
                if (howManyPathsAway > 1)
                    howManyPathsAway = 1;
                if (howManyPathsAway < -1)
                    howManyPathsAway = -1;
            }

            if (!isValid || howManyPathsAway != oldHowManyPathsAway || (oldIsHeadingSameWay != isHeadingSameWay && mf.tool.toolOffset != 0))
            {
                curList.Clear();

                if (refList == creatingContour && howManyPathsAway == 0) return;

                double distAway = widthMinusOverlap * howManyPathsAway + (isHeadingSameWay ? -mf.tool.toolOffset : mf.tool.toolOffset);

                curList = BuildOffsetList(refList, distAway);
                isValid = true;

                if (mf.isSideGuideLines && (refList.mode.HasFlag(Mode.AB) || refList.mode.HasFlag(Mode.Boundary)) && howManyPathsAway != oldHowManyPathsAway)
                {
                    int Gcnt;

                    int Move = howManyPathsAway - oldHowManyPathsAway;

                    if (!isValid || Move < -5 || Move > 5 || Move == 0)
                    {
                        if (sideGuideLines.Count > 0) sideGuideLines.Clear();
                        for (double i = -2.5; i < 3; i++)
                        {
                            sideGuideLines.Add(new List<vec3>());
                            Gcnt = sideGuideLines.Count - 1;

                            sideGuideLines[Gcnt] = BuildOffsetList(refList, widthMinusOverlap * (howManyPathsAway + i));

                        }
                    }
                    else if (Move < 0)
                    {
                        for (int i = -1; i >= Move; i--)
                        {
                            sideGuideLines.RemoveAt(5);
                            sideGuideLines.Insert(0, new List<vec3>());
                            Gcnt = 0;
                            sideGuideLines[Gcnt] = BuildOffsetList(refList, widthMinusOverlap * (oldHowManyPathsAway - 2.5 + i));
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= Move; i++)
                        {
                            sideGuideLines.RemoveAt(0);
                            sideGuideLines.Add(new List<vec3>());
                            Gcnt = 5;
                            sideGuideLines[Gcnt] = BuildOffsetList(refList, widthMinusOverlap * (oldHowManyPathsAway + 2.5 + i));
                        }
                    }
                }
                else if (!mf.isSideGuideLines) sideGuideLines.Clear();
                
                oldHowManyPathsAway = howManyPathsAway;
                oldIsHeadingSameWay = isHeadingSameWay;
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
            else if (refList.mode.HasFlag(Mode.Boundary))
            {
                Polyline Poly = new Polyline();

                for (int i = 0; i < refList.curvePts.Count; i++)
                {
                    Poly.points.Add(new vec2(refList.curvePts[i].easting, refList.curvePts[i].northing));
                }

                Polyline BB = Poly.OffsetAndDissolvePolyline(distAway, true, -1, -1, true, mf.vehicle.minTurningRadius);

                //BB.points.CalculateRoundedCorner(mf.vehicle.minTurningRadius, mf.vehicle.minTurningRadius, BB.loop, 0.4);

                for (int i = 0; i < BB.points.Count; i++)
                {
                    buildList.Add(new vec3(BB.points[i].easting, BB.points[i].northing, 0));
                }

                buildList.CalculateHeadings(true);
            }
            else
            {
                int ptCount = refList.curvePts.Count - (refList == creatingContour ? backSpacing : 0);

                int start = (refList.mode.HasFlag(Mode.Contour) && refList == creatingContour) ? (rA - (isHeadingSameWay ? 10 : 50)) : 0;
                if (start < 0) start = 0;

                int end = (refList.mode.HasFlag(Mode.Contour) && refList == creatingContour) ? (rA + (isHeadingSameWay ? 50 : 10)) : ptCount;
                if (end > ptCount) end = ptCount;

                for (int i = start; i < end; i++)
                {
                    vec3 point = new vec3(
                    refList.curvePts[i].easting + Math.Cos(refList.curvePts[i].heading) * distAway,
                    refList.curvePts[i].northing - Math.Sin(refList.curvePts[i].heading) * distAway,
                    refList.curvePts[i].heading);
                    bool Add = true;
                    for (int t = start; t < end; t++)
                    {
                        double dist = glm.DistanceSquared(point, refList.curvePts[t]);
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
                            double dist = glm.DistanceSquared(point, buildList[buildList.Count - 1]);
                            if (dist > 1)
                                buildList.Add(point);
                        }
                        else buildList.Add(point);
                    }
                }

                if (refList.mode.HasFlag(Mode.Curve))
                {
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
                    }
                }
                buildList.CalculateHeadings(false);
            }
            return buildList;
        }

        public void DrawGuidanceLines()
        {
            GL.LineWidth(lineWidth);

            if (CurrentGMode == Mode.RecPath && recList.Count > 0)
            {
                GL.Color3(0.98f, 0.92f, 0.460f);
                GL.Begin(PrimitiveType.LineStrip);
                for (int h = 0; h < recList.Count; h++) GL.Vertex3(recList[h].easting, recList[h].northing, 0);
                GL.End();

                if (!isRecordOn && currentPositonIndex < recList.Count)
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

            if (EditGuidanceLine != null)
            {
                GL.Color3(0.95f, 0.42f, 0.750f);
                GL.Begin(PrimitiveType.LineStrip);

                for (int h = 0; h < EditGuidanceLine.curvePts.Count; h++)
                {
                    if (h == 0 && !EditGuidanceLine.mode.HasFlag(Mode.Boundary))
                        GL.Vertex3(EditGuidanceLine.curvePts[h].easting - (Math.Sin(EditGuidanceLine.curvePts[h].heading) * abLength), EditGuidanceLine.curvePts[h].northing - (Math.Cos(EditGuidanceLine.curvePts[h].heading) * abLength), 0.0);

                    GL.Vertex3(EditGuidanceLine.curvePts[h].easting, EditGuidanceLine.curvePts[h].northing, 0);

                    if (h == EditGuidanceLine.curvePts.Count - 1 && EditGuidanceLine.mode.HasFlag(Mode.AB))
                        GL.Vertex3(EditGuidanceLine.curvePts[h].easting + (Math.Sin(EditGuidanceLine.curvePts[h].heading) * abLength), EditGuidanceLine.curvePts[h].northing + (Math.Cos(EditGuidanceLine.curvePts[h].heading) * abLength), 0.0);
                }

                if (!EditGuidanceLine.mode.HasFlag(Mode.AB))
                {
                    GL.Color3(0.930f, 0.0692f, 0.260f);
                    GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
                }
                GL.End();
            }
            else
            {
                if (currentGuidanceLine != null && currentGuidanceLine.curvePts.Count > 1)
                {
                    if (isSmoothWindowOpen)
                        GL.Color3(0.930f, 0.92f, 0.260f);
                    else
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

                        if (Extend && h == currentGuidanceLine.curvePts.Count - 1)
                            GL.Vertex3(currentGuidanceLine.curvePts[h].easting + (Math.Sin(currentGuidanceLine.curvePts[h].heading) * abLength), currentGuidanceLine.curvePts[h].northing + (Math.Cos(currentGuidanceLine.curvePts[h].heading) * abLength), 0.0);
                    }
                    GL.End();
                    GL.Disable(EnableCap.LineStipple);
                }

                if (curList.Count > 1)
                {
                    if (mf.isSideGuideLines && mf.worldManager.camSetDistance > mf.tool.toolWidth * -120)
                    {
                        GL.Color3(0.756f, 0.7650f, 0.7650f);
                        GL.Enable(EnableCap.LineStipple);
                        GL.LineStipple(1, 0x0303);

                        for (int i = 0; i < sideGuideLines.Count; i++)
                        {
                            GL.Begin(currentGuidanceLine.mode.HasFlag(Mode.AB) ? PrimitiveType.LineStrip : PrimitiveType.LineLoop);
                            for (int j = 0; j < sideGuideLines[i].Count; j++)
                            {
                                GL.Vertex3(sideGuideLines[i][j].easting, sideGuideLines[i][j].northing, 0);
                            }
                            GL.End();
                        }

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

                    if (!mf.isStanleyUsed && mf.worldManager.camSetDistance > -200)
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
                        GL.Enable(EnableCap.LineStipple);
                        GL.LineStipple(1, 0xFC00);
                        GL.Color3(0.95f, 0.5f, 0.95f);
                        GL.Begin(PrimitiveType.LineStrip);
                        for (int i = 0; i < OffsetList.Count; i++)
                        {
                            GL.Vertex3(OffsetList[i].easting, OffsetList[i].northing, 0);
                        }
                        GL.End();
                        GL.Disable(EnableCap.LineStipple);
                    }

                    if (ytList.Count > 1)
                    {
                        GL.PointSize(lineWidth * 2);

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
            isValid = false;

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