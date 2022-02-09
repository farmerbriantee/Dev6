using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        public bool isContourOn, isContourBtnOn;

        private int stripNum, lastLockPt = int.MaxValue, backSpacing = 30;

        //list of strip data individual points
        public List<vec3> ptList = new List<vec3>();

        //list of the list of individual Lines for entire field
        public List<List<vec3>> stripList = new List<List<vec3>>();

        //list of points for the new contour line
        public List<vec3> ctList = new List<vec3>();

        public bool isLocked = false;

        //hitting the cycle lines buttons lock to current line
        public void SetLockToLine()
        {
            if (ctList.Count > 5) isLocked = !isLocked;
        }

        public void BuildContourGuidanceLine(vec3 pivot, vec3 steer)
        {
            if ((mf.secondsSinceStart - lastSecond) < (ctList.Count == 0 ? 0.3 : 2.0)) return;

            lastSecond = mf.secondsSinceStart;
            int ptCount;
            double minDistA = double.MaxValue;
            int start, stop;

            int pt = 0;

            //check if no strips yet, return
            int stripCount = stripList.Count;

            //if making a new strip ignore it or it will win always
            stripCount--;
            if (stripCount < 0) return;

            if (!isLocked)
            {
                stripNum = 0;
                for (int s = 0; s < stripCount; s++)
                {
                    ptCount = stripList[s].Count;
                    if (ptCount == 0) continue;
                    double dist;
                    for (int p = 0; p < ptCount; p += 6)
                    {
                        dist = ((pivot.easting - stripList[s][p].easting) * (pivot.easting - stripList[s][p].easting))
                            + ((pivot.northing - stripList[s][p].northing) * (pivot.northing - stripList[s][p].northing));
                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            stripNum = s;
                        }
                    }

                    //catch the last point
                    dist = ((pivot.easting - stripList[s][ptCount - 1].easting) * (pivot.easting - stripList[s][ptCount - 1].easting))
                        + ((pivot.northing - stripList[s][ptCount - 1].northing) * (pivot.northing - stripList[s][ptCount - 1].northing));
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        stripNum = s;
                    }
                }

                for (int p = 0; p < stripList[stripCount].Count - backSpacing; p += 4)
                {
                    double dist = ((pivot.easting - stripList[stripCount][p].easting) * (pivot.easting - stripList[stripCount][p].easting))
                        + ((pivot.northing - stripList[stripCount][p].northing) * (pivot.northing - stripList[stripCount][p].northing));                    
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        stripNum = stripCount;
                    }
                }

                //no points in the box, exit
                ptCount = stripList[stripNum].Count;
                if (ptCount < 2)
                {
                    ctList.Clear();
                    isLocked = false;
                    return;
                }

                //determine closest point
                double minDistance = double.MaxValue;

                //if being built, start high, keep from guiding latest points made
                int currentStripBox = 0;
                if (stripNum == stripCount) currentStripBox = backSpacing;
                for (int i = 0; i < ptCount - currentStripBox; i++)
                {
                    double dist = ((pivot.easting - stripList[stripNum][i].easting) * (pivot.easting - stripList[stripNum][i].easting))
                        + ((pivot.northing - stripList[stripNum][i].northing) * (pivot.northing - stripList[stripNum][i].northing));

                    if (dist <= minDistance)
                    {
                        minDistance = dist;
                        pt = lastLockPt = i;
                    }
                }

                minDistance = Math.Sqrt(minDistance);

                if (minDistance > 2.6 * mf.tool.toolWidth)
                {
                    ctList.Clear();
                    isLocked = false;
                    return;
                }
            }

            //locked to this stripNum so find closest within a range
            else
            {
                //no points in the box, exit
                ptCount = stripList[stripNum].Count;

                start = lastLockPt - 10; if (start < 0) start = 0;
                stop = lastLockPt + 10; if (stop > ptCount) stop = ptCount;

                if (ptCount < 2 )
                {
                    ctList.Clear();
                    isLocked = false;
                    return;
                }

                //determine closest point
                double minDistance = double.MaxValue;

                //if being built, start high, keep from guiding latest points made
                //int currentStripBox = 0;
                //if (stripNum == stripCount) currentStripBox = 10;
                for (int i = start; i < stop; i++)
                {
                    double dist = ((pivot.easting - stripList[stripNum][i].easting) * (pivot.easting - stripList[stripNum][i].easting))
                        + ((pivot.northing - stripList[stripNum][i].northing) * (pivot.northing - stripList[stripNum][i].northing));

                    if (minDistance >= dist)
                    {
                        minDistance = dist;
                        pt = lastLockPt = i;
                    }
                }

                minDistance = Math.Sqrt(minDistance);

                if (minDistance > 2 * mf.tool.toolWidth)
                {
                    ctList.Clear();
                    isLocked = false;
                    return;
                }
            }

            //now we have closest point, the distance squared from it, and which patch and point its from
            double refX = stripList[stripNum][pt].easting;
            double refZ = stripList[stripNum][pt].northing;

            double dx, dz, distanceFromRefLine;

            if (pt < stripList[stripNum].Count - 1)
            {
                dx = stripList[stripNum][pt + 1].easting - refX;
                dz = stripList[stripNum][pt + 1].northing - refZ;

                //how far are we away from the reference line at 90 degrees - 2D cross product and distance
                distanceFromRefLine = ((dz * pivot.easting) - (dx * pivot.northing) + (stripList[stripNum][pt + 1].easting
                                        * refZ) - (stripList[stripNum][pt + 1].northing * refX))
                                        / Math.Sqrt((dz * dz) + (dx * dx));
            }
            else if (pt > 0)
            {
                dx = refX - stripList[stripNum][pt - 1].easting;
                dz = refZ - stripList[stripNum][pt - 1].northing;

                //how far are we away from the reference line at 90 degrees - 2D cross product and distance
                distanceFromRefLine = ((dz * pivot.easting) - (dx * pivot.northing) + (refX
                                        * stripList[stripNum][pt - 1].northing) - (refZ * stripList[stripNum][pt - 1].easting))
                                        / Math.Sqrt((dz * dz) + (dx * dx));
            }
            else return;


            //are we going same direction as stripList was created?
            bool isSameWay = Math.PI - Math.Abs(Math.Abs(mf.fixHeading - stripList[stripNum][pt].heading) - Math.PI) < 1.57;

            double RefDist = (distanceFromRefLine + (isSameWay ? mf.tool.toolOffset : -mf.tool.toolOffset)) 
                                / (mf.tool.toolWidth - mf.tool.toolOverlap);

            double howManyPathsAway;

            if (Math.Abs(distanceFromRefLine) > mf.tool.halfToolWidth)
            {
                //beside what is done
                if (RefDist < 0) howManyPathsAway = -1;
                else howManyPathsAway = 1;
            }
            else
            {
                //driving on what is done
                howManyPathsAway = 0;
            }

            if (howManyPathsAway >= -1 && howManyPathsAway <= 1)
            {
                //Is our angle of attack too high? Stops setting the wrong mapped path sometimes
                //double refToPivotDelta = Math.PI - Math.Abs(Math.Abs(pivot.heading - stripList[stripNum][pt].heading) - Math.PI);
                //if (refToPivotDelta > glm.PIBy2) refToPivotDelta = Math.Abs(refToPivotDelta - Math.PI);

                //if (refToPivotDelta > 0.8)
                //{
                //    ctList.Clear();
                //    isLocked = false;
                //    return;
                //}

                ctList.Clear();

                //don't guide behind yourself
                if (stripNum == stripList.Count-1 && howManyPathsAway == 0) return;

                //make the new guidance line list called guideList
                ptCount = stripList[stripNum].Count;

                //shorter behind you
                if (isSameWay)
                {
                    start = pt - 6; if (start < 0) start = 0;
                    stop = pt + 45; if (stop > ptCount) stop = ptCount;
                }
                else
                {
                    start = pt - 45; if (start < 0) start = 0;
                    stop = pt + 6; if (stop > ptCount) stop = ptCount;
                }

                //if (howManyPathsAway != 0 && (mf.tool.halfToolWidth < (0.5*mf.tool.toolOffset)))
                {
                    double distAway = (mf.tool.toolWidth - mf.tool.toolOverlap) * howManyPathsAway + (isSameWay ? -mf.tool.toolOffset : mf.tool.toolOffset);
                    double distSqAway = (distAway * distAway) * 0.97;


                    for (int i = start; i < stop; i++)
                    {
                        vec3 point = new vec3(
                            stripList[stripNum][i].easting + (Math.Cos(stripList[stripNum][i].heading) * distAway),
                            stripList[stripNum][i].northing - (Math.Sin(stripList[stripNum][i].heading) * distAway),
                            stripList[stripNum][i].heading);

                        bool Add = true;
                        //make sure its not closer then 1 eq width
                        for (int j = start; j < stop; j++)
                        {
                            double check = glm.DistanceSquared(point.northing, point.easting, stripList[stripNum][j].northing, stripList[stripNum][j].easting);
                            if (check < distSqAway)
                            {
                                Add = false;
                                break;
                            }
                        }
                        if (Add)
                        {
                            if (false && ctList.Count > 0)
                            {
                                double dist = ((point.easting - ctList[ctList.Count - 1].easting) * (point.easting - ctList[ctList.Count - 1].easting))
                                    + ((point.northing - ctList[ctList.Count - 1].northing) * (point.northing - ctList[ctList.Count - 1].northing));
                                if (dist > 0.3)
                                    ctList.Add(point);
                            }
                            else ctList.Add(point);
                        }
                    }
                }

                int ptc = ctList.Count;
                if (ptc < 5)
                {
                    ctList.Clear();
                    isLocked = false;
                    return;
                }
            }
            else
            {
                ctList.Clear();
                isLocked = false;
                return;
            }
        }

        //determine distance from contour guidance line
        public void DistanceFromContourLine(vec3 pivot, vec3 steer)
        {
            if (ctList.Count > 8)
            {
                if (mf.isStanleyUsed)
                    StanleyGuidanceContour(pivot, steer, ctList);
                else
                    PurePursuitContour(pivot, steer, ctList);
            }
            else
            {
                //invalid distance so tell AS module
                mf.guidanceLineDistanceOff = 32000;
            }
        }

        //start stop and add points to list
        public void StartContourLine(vec3 pivot)
        {
            if (stripList.Count == 0)
            {
                //make new ptList
                ptList = new List<vec3>(16);
                //ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading) 
                //    * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));
                stripList.Add(ptList);
                isContourOn = true;
                return;
            }
            else
            {
                //reuse ptList
                if (ptList.Count > 0) ptList.Clear();
                //ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading) 
                //    * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));
                isContourOn = true;
            }
        }

        //Add current position to stripList
        public void AddPoint(vec3 pivot)
        {
            ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading) * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));
        }

        //End the strip
        public void StopContourLine(vec3 pivot)
        {
            //make sure its long enough to bother
            if (ptList.Count > 5)
            {
                //ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading) 
                //    * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));

                //build tale
                double head = ptList[0].heading;
                int length = (int)mf.tool.toolWidth+3;
                vec3 pnt;
                for (int a = 0; a < length; a ++)
                {
                    pnt.easting = ptList[0].easting - (Math.Sin(head));
                    pnt.northing = ptList[0].northing - (Math.Cos(head));
                    pnt.heading = ptList[0].heading;
                    ptList.Insert(0, pnt);
                }

                int ptc = ptList.Count - 1;
                head = ptList[ptc].heading;

                for (double i = 1; i < length; i += 1)
                {
                    pnt.easting = ptList[ptc].easting + (Math.Sin(head) * i);
                    pnt.northing = ptList[ptc].northing + (Math.Cos(head) * i);
                    pnt.heading = head;
                    ptList.Add(pnt);
                }

                //add the point list to the save list for appending to contour file
                mf.contourSaveList.Add(ptList);

                ptList = new List<vec3>(32);
                stripList.Add(ptList);

            }

            //delete ptList
            else
            {
                ptList.Clear();
            }

            //turn it off
            isContourOn = false;
        }

        //build contours for boundaries
        public void BuildFenceContours(int pass, int spacingInt)
        {
            if (mf.bnd.bndList.Count == 0)
            {
                mf.TimedMessageBox(1500, "Boundary Contour Error", "No Boundaries Made");
                return;
            }

            double totalWidth;

            if (pass == 1)
            {
                //determine how wide a headland space
                totalWidth = (((mf.tool.toolWidth - mf.tool.toolOverlap) * 0.5) - spacingInt) * -1;
            }
            else
            {
                totalWidth = ((mf.tool.toolWidth - mf.tool.toolOverlap) * pass) + spacingInt +
                    ((mf.tool.toolWidth - mf.tool.toolOverlap) * 0.5);
            }

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {

                Polyline New = mf.bnd.bndList[j].fenceLine.OffsetAndDissolvePolyline(j == 0 ? totalWidth : -totalWidth, true, -1,-1, true);

                ptList = new List<vec3>(New.points.Count);
                for (int i = New.points.Count - 1; i >= 0; i--)
                {
                    ptList.Add(new vec3(New.points[i].easting, New.points[i].northing, 0));
                }
                ptList.CalculateHeadings();
                stripList.Add(ptList);
            }

            mf.TimedMessageBox(1500, "Boundary Contour", "Contour Path Created");
        }

        //draw the red follow me line
        public void DrawContourLine()
        {
            ////draw the guidance line
            int ptCount = ctList.Count;
            if (ptCount < 2) return;
            GL.LineWidth(lineWidth);
            GL.Color3(0.98f, 0.2f, 0.980f);
            GL.Begin(PrimitiveType.LineStrip);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(ctList[h].easting, ctList[h].northing, 0);
            GL.End();

            GL.PointSize(lineWidth);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0.87f, 08.7f, 0.25f);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(ctList[h].easting, ctList[h].northing, 0);

            GL.End();

            //Draw the captured ref strip, red if locked
            if (isLocked)
            {
                GL.Color3(0.983f, 0.2f, 0.20f);
                GL.LineWidth(4);
            }
            else
            {
                GL.Color3(0.3f, 0.982f, 0.0f);
                GL.LineWidth(lineWidth);
            }

            //GL.PointSize(6.0f);
            GL.Begin(PrimitiveType.Points);
            for (int h = 0; h < stripList[stripNum].Count; h++) GL.Vertex3(stripList[stripNum][h].easting, stripList[stripNum][h].northing, 0);
            GL.End();

            //GL.Begin(PrimitiveType.Points);
            //GL.Color3(1.0f, 0.95f, 0.095f);
            //GL.Vertex3(rEastCT, rNorthCT, 0.0);
            //GL.End();
            //GL.PointSize(1.0f);

            //GL.Color3(0.98f, 0.98f, 0.50f);
            //GL.Begin(PrimitiveType.LineStrip);
            //GL.Vertex3(boxE.easting, boxE.northing, 0);
            //GL.Vertex3(boxA.easting, boxA.northing, 0);
            //GL.Vertex3(boxD.easting, boxD.northing, 0);
            //GL.Vertex3(boxG.easting, boxG.northing, 0);
            //GL.Vertex3(boxE.easting, boxE.northing, 0);
            //GL.End();

            //GL.Begin(PrimitiveType.LineStrip);
            //GL.Vertex3(boxF.easting, boxF.northing, 0);
            //GL.Vertex3(boxH.easting, boxH.northing, 0);
            //GL.Vertex3(boxC.easting, boxC.northing, 0);
            //GL.Vertex3(boxB.easting, boxB.northing, 0);
            //GL.Vertex3(boxF.easting, boxF.northing, 0);
            //GL.End();

            ////draw the reference line
            //GL.PointSize(3.0f);
            ////if (isContourBtnOn)
            //{
            //    ptCount = stripList.Count;
            //    if (ptCount > 0)
            //    {
            //        ptCount = stripList[closestRefPatch].Count;
            //        GL.Begin(PrimitiveType.Points);
            //        for (int i = 0; i < ptCount; i++)
            //        {
            //            GL.Vertex2(stripList[closestRefPatch][i].easting, stripList[closestRefPatch][i].northing);
            //        }
            //        GL.End();
            //    }
            //}

            //ptCount = conList.Count;
            //if (ptCount > 0)
            //{
            //    //draw closest point and side of line points
            //    GL.Color3(0.5f, 0.900f, 0.90f);
            //    GL.PointSize(4.0f);
            //    GL.Begin(PrimitiveType.Points);
            //    for (int i = 0; i < ptCount; i++) GL.Vertex3(conList[i].x, conList[i].z, 0);
            //    GL.End();

            //    GL.Color3(0.35f, 0.30f, 0.90f);
            //    GL.PointSize(6.0f);
            //    GL.Begin(PrimitiveType.Points);
            //    GL.Vertex3(conList[closestRefPoint].x, conList[closestRefPoint].z, 0);
            //    GL.End();
            //}

            if (mf.isPureDisplayOn && mf.guidanceLineDistanceOff != 32000 && !mf.isStanleyUsed)
            {
                //if (ppRadiusCT < 50 && ppRadiusCT > -50)
                //{
                //    const int numSegments = 100;
                //    double theta = glm.twoPI / numSegments;
                //    double c = Math.Cos(theta);//precalculate the sine and cosine
                //    double s = Math.Sin(theta);
                //    double x = ppRadiusCT;//we start at angle = 0
                //    double y = 0;

                //    GL.LineWidth(1);
                //    GL.Color3(0.795f, 0.230f, 0.7950f);
                //    GL.Begin(PrimitiveType.LineLoop);
                //    for (int ii = 0; ii < numSegments; ii++)
                //    {
                //        //glVertex2f(x + cx, y + cy);//output vertex
                //        GL.Vertex3(x + radiusPointCT.easting, y + radiusPointCT.northing, 0);//output vertex

                //        //apply the rotation matrix
                //        double t = x;
                //        x = (c * x) - (s * y);
                //        y = (s * t) + (c * y);
                //    }
                //    GL.End();
                //}

                //Draw lookahead Point
                GL.PointSize(6.0f);
                GL.Begin(PrimitiveType.Points);

                GL.Color3(1.0f, 0.95f, 0.095f);
                GL.Vertex3(goalPoint.easting, goalPoint.northing, 0.0);
                GL.End();
                GL.PointSize(1.0f);
            }
        }

        //Reset the contour to zip
        public void ResetContour()
        {
            stripList.Clear();
            ptList?.Clear();
            ctList?.Clear();
        }
    }//class
}//namespace