using OpenTK.Graphics.OpenGL;
using System;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        private int lastLockPt = int.MaxValue, backSpacing = 30;

        public CGuidanceLine creatingContour, currentContour;

        public bool isLocked = false;

        public void BuildCurrentContourLine(vec3 pivot)
        {
            if ((mf.secondsSinceStart - lastSecond) < (curList.Count == 0 ? 0.3 : 2.0)) return;

            lastSecond = mf.secondsSinceStart;
            int ptCount;
            double minDistA = double.MaxValue;
            int start, stop;

            int pt = 0;

            if (!isLocked)
            {
                int stripNum = -1;
                for (int s = 0; s < curveArr.Count; s++)
                {
                    if (curveArr[s].mode.HasFlag(Mode.Contour))
                    {
                        ptCount = curveArr[s].curvePts.Count - (curveArr[s] == creatingContour ? backSpacing : 1);
                        if (ptCount == 0) continue;
                        double dist;
                        for (int p = 0; p < ptCount; p += 6)
                        {
                            dist = glm.Distance(pivot, curveArr[s].curvePts[p]);
                            if (dist < minDistA)
                            {
                                lastLockPt = p;
                                minDistA = dist;
                                stripNum = s;
                            }
                        }

                        //catch the last point
                        dist = glm.Distance(pivot, curveArr[s].curvePts[ptCount]);
                        if (dist < minDistA)
                        {
                            lastLockPt = ptCount;
                            minDistA = dist;
                            stripNum = s;
                        }
                    }
                }

                if (stripNum >= 0)
                    currentContour = curveArr[stripNum];
                else
                    currentContour = null;
            }

            if (currentContour != null)
            {
                ptCount = currentContour.curvePts.Count - (currentContour == creatingContour ? backSpacing : 0);
                if (ptCount < 2)
                {
                    currentContour = null;
                    curList.Clear();
                    isLocked = false;
                    return;
                }

                //determine closest point
                double minDistance = double.MaxValue;

                start = lastLockPt - 10;
                if (start < 0) start = 0;
                stop = lastLockPt + 10;
                if (stop > ptCount) stop = ptCount;

                for (int i = start; i < stop; i++)
                {
                    double dist = glm.Distance(pivot, currentContour.curvePts[i]);

                    if (dist <= minDistance)
                    {
                        minDistance = dist;
                        pt = lastLockPt = i;
                    }
                }

                minDistance = Math.Sqrt(minDistance);

                if (minDistance > 2.5 * mf.tool.toolWidth)
                {
                    currentContour = null;
                    curList.Clear();
                    isLocked = false;
                    return;
                }

                //now we have closest point, the distance squared from it, and which patch and point its from
                double refX = currentContour.curvePts[pt].easting;
                double refZ = currentContour.curvePts[pt].northing;

                double dx, dz, distanceFromRefLine = 0;

                if (pt < currentContour.curvePts.Count - 1)
                {
                    dx = currentContour.curvePts[pt + 1].easting - refX;
                    dz = currentContour.curvePts[pt + 1].northing - refZ;

                    //how far are we away from the reference line at 90 degrees - 2D cross product and distance
                    distanceFromRefLine = ((dz * pivot.easting) - (dx * pivot.northing) + (currentContour.curvePts[pt + 1].easting
                                            * refZ) - (currentContour.curvePts[pt + 1].northing * refX))
                                            / Math.Sqrt((dz * dz) + (dx * dx));
                }
                else if (pt > 0)
                {
                    dx = refX - currentContour.curvePts[pt - 1].easting;
                    dz = refZ - currentContour.curvePts[pt - 1].northing;

                    //how far are we away from the reference line at 90 degrees - 2D cross product and distance
                    distanceFromRefLine = ((dz * pivot.easting) - (dx * pivot.northing) + (refX
                                            * currentContour.curvePts[pt - 1].northing) - (refZ * currentContour.curvePts[pt - 1].easting))
                                            / Math.Sqrt((dz * dz) + (dx * dx));
                }

                //are we going same direction as stripList was created?
                isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(mf.fixHeading - currentContour.curvePts[pt].heading) - Math.PI) < 1.57;

                double RefDist = (distanceFromRefLine + (isHeadingSameWay ? mf.tool.toolOffset : -mf.tool.toolOffset))
                                    / (mf.tool.toolWidth - mf.tool.toolOverlap);

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

                    curList.Clear();

                    //don't guide behind yourself
                    if (currentContour == creatingContour && howManyPathsAway == 0) return;

                    //shorter behind you
                    if (isHeadingSameWay)
                    {
                        start = pt - 6; if (start < 0) start = 0;
                        stop = pt + 45; if (stop > ptCount) stop = ptCount;
                    }
                    else
                    {
                        start = pt - 45; if (start < 0) start = 0;
                        stop = pt + 6; if (stop > ptCount) stop = ptCount;
                    }

                    double distAway = (mf.tool.toolWidth - mf.tool.toolOverlap) * howManyPathsAway + (isHeadingSameWay ? -mf.tool.toolOffset : mf.tool.toolOffset);
                    double distSqAway = (distAway * distAway) * 0.97;

                    for (int i = start; i < stop; i++)
                    {
                        vec3 point = new vec3(
                            currentContour.curvePts[i].easting + (Math.Cos(currentContour.curvePts[i].heading) * distAway),
                            currentContour.curvePts[i].northing - (Math.Sin(currentContour.curvePts[i].heading) * distAway),
                            currentContour.curvePts[i].heading);

                        bool Add = true;
                        //make sure its not closer then 1 eq width
                        for (int j = start; j < stop; j++)
                        {
                            double check = glm.DistanceSquared(point.northing, point.easting, currentContour.curvePts[j].northing, currentContour.curvePts[j].easting);
                            if (check < distSqAway)
                            {
                                Add = false;
                                break;
                            }
                        }
                        if (Add)
                        {
                            if (false && curList.Count > 0)
                            {
                                double dist = ((point.easting - curList[curList.Count - 1].easting) * (point.easting - curList[curList.Count - 1].easting))
                                    + ((point.northing - curList[curList.Count - 1].northing) * (point.northing - curList[curList.Count - 1].northing));
                                if (dist > 0.3)
                                    curList.Add(point);
                            }
                            else curList.Add(point);
                        }
                    }
                    
                    int ptc = curList.Count;
                    if (ptc < 5)
                    {
                        currentContour = null;
                        curList.Clear();
                        isLocked = false;
                        return;
                    }
                }
                else
                {
                    currentContour = null;
                    curList.Clear();
                    isLocked = false;
                    return;
                }
            }
            else
            {
                currentContour = null;
                curList.Clear();
                isLocked = false;
                return;
            }
        }

        public void AddPoint(vec3 pivot)
        {
            if (creatingContour == null)
                creatingContour = new CGuidanceLine(Mode.Contour);

            creatingContour.curvePts.Add(new vec3(pivot.easting + Math.Cos(pivot.heading) * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));
        }

        //End the strip
        public void StopContourLine()
        {
            //make sure its long enough to bother
            if (creatingContour?.curvePts.Count > 5)
            {
                //build tale
                double head = creatingContour.curvePts[0].heading;
                int length = (int)mf.tool.toolWidth + 3;
                vec3 pnt;
                for (int a = 0; a < length; a++)
                {
                    pnt.easting = creatingContour.curvePts[0].easting - (Math.Sin(head));
                    pnt.northing = creatingContour.curvePts[0].northing - (Math.Cos(head));
                    pnt.heading = creatingContour.curvePts[0].heading;
                    creatingContour.curvePts.Insert(0, pnt);
                }

                int ptc = creatingContour.curvePts.Count - 1;
                head = creatingContour.curvePts[ptc].heading;

                for (double i = 1; i < length; i += 1)
                {
                    pnt.easting = creatingContour.curvePts[ptc].easting + (Math.Sin(head) * i);
                    pnt.northing = creatingContour.curvePts[ptc].northing + (Math.Cos(head) * i);
                    pnt.heading = head;
                    creatingContour.curvePts.Add(pnt);
                }

                //add the point list to the save list for appending to contour file
                mf.contourSaveList.Add(creatingContour.curvePts);

                curveArr.Add(creatingContour);
            }
            else creatingContour = null;
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

                var New2 = new CGuidanceLine(Mode.Contour | Mode.Boundary);

                for (int i = New.points.Count - 1; i >= 0; i--)
                {
                    New2.curvePts.Add(new vec3(New.points[i].easting, New.points[i].northing, 0));
                }
                New2.curvePts.CalculateHeadings(true);

                curveArr.Add(New2);
            }

            mf.TimedMessageBox(1500, "Boundary Contour", "Contour Path Created");
        }

        //draw the red follow me line
        public void DrawContourLine()
        {
            ////draw the guidance line
            int ptCount = curList.Count;
            if (ptCount < 2) return;
            GL.LineWidth(lineWidth);
            GL.Color3(0.98f, 0.2f, 0.980f);
            GL.Begin(PrimitiveType.LineStrip);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(curList[h].easting, curList[h].northing, 0);
            GL.End();

            GL.PointSize(lineWidth);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0.87f, 08.7f, 0.25f);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(curList[h].easting, curList[h].northing, 0);

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

            if (currentContour != null)
            {
                //GL.PointSize(6.0f);
                GL.Begin(PrimitiveType.Points);
                for (int h = 0; h < currentContour.curvePts.Count; h++) GL.Vertex3(currentContour.curvePts[h].easting, currentContour.curvePts[h].northing, 0);
                GL.End();
            }

            if (mf.isPureDisplayOn && mf.guidanceLineDistanceOff != 32000 && !mf.isStanleyUsed)
            {
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
            creatingContour = null;
            currentContour = null;
            curList?.Clear();
        }
    }//class
}//namespace