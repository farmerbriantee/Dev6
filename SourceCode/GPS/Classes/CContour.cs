using System;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        public void AddPoint(vec2 pivot)
        {
            if (creatingContour == null)
            {
                creatingContour = new CGuidanceLine(Mode.Contour);
                curveArr.Add(creatingContour);
            }

            creatingContour.points.Add(new vec2(pivot.easting + mf.cosH * mf.tool.toolOffset, pivot.northing - mf.sinH * mf.tool.toolOffset));
        }

        //End the strip
        public void StopContourLine()
        {
            //make sure its long enough to bother
            if (creatingContour?.points.Count > 5)
            {
                creatingContour.points.AddFirstLastPoints(mf.tool.toolWidth + 3, 5, false);

                //add the point list to the save list for appending to contour file
                mf.contourSaveList.Add(creatingContour.points);
            }
            else
                curveArr.Remove(creatingContour);
            
            creatingContour = null;
        }

        //build contours for boundaries
        public bool BuildFenceContours(double pass, double spacing)
        {
            if (mf.bnd.bndList.Count == 0)
            {
                return false;
            }

            double offsetDist = ((mf.tool.toolWidth - mf.tool.toolOverlap) * pass) + spacing;

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                Polyline New = mf.bnd.bndList[j].fenceLine.OffsetAndDissolvePolyline<Polyline>(j == 0 ? offsetDist : -offsetDist)[0];
                New.loop = true;
                curveArr.Add(new CGuidanceLine(Mode.Contour, New) { Name = "Boundary Contour" });
            }

            return true;
        }

        //Reset the contour to zip
        public void ResetContour()
        {
            creatingContour = null;
            //curList.Clear();

            for (int i = curveArr.Count - 1; i >= 0; i--)
            {
                if (curveArr[i].mode.HasFlag(Mode.Contour))
                    curveArr.RemoveAt(i);
            }
        }

        public bool StartDrivingRecordedPath()
        {
            //create the dubins path based on start and goal to start of recorded path
            if (currentRecPath?.points.Count > 4)
            {
                if (resumeState == 0) //start at the start
                    currentPositonIndex = 0;
                else if (resumeState == 1) //resume from where stopped mid path
                {
                    if (currentPositonIndex + 5 > currentRecPath.points.Count)
                        currentPositonIndex = 0;
                }
                else //find closest point
                {
                    // Try to find the nearest point of the recordet path in relation to the current position:
                    mf.pivotAxlePos.GetCurrentSegment(currentRecPath, out int AA, out int BB);

                    //scootch down the line a bit
                    if (BB + 5 < currentRecPath.points.Count) BB += 5;
                    else BB = currentRecPath.points.Count - 1;

                    currentPositonIndex = BB;
                }

                if (currentPositonIndex + 1 < currentRecPath.points.Count)
                {
                    //the goal is the first point of path, the start is the current position
                    //get the dubins for approach to recorded path

                    vec2 diff = currentRecPath.points[currentPositonIndex + 1] - currentRecPath.points[currentPositonIndex];
                    double Heading = Math.Atan2(diff.easting, diff.northing);

                    CDubins dubPath = new CDubins(mf.vehicle.minTurningRadius * 1.2);

                    // current psition
                    vec2 pivotAxlePosRP = mf.pivotAxlePos;

                    //bump it forward
                    vec2 pt2 = new vec2(pivotAxlePosRP.easting + (mf.sinH * 3),
                        pivotAxlePosRP.northing + (mf.cosH * 3));

                    //get the dubins path vec2 point coordinates of turn
                    ytList.points = dubPath.GenerateDubins(pt2, mf.fixHeading, currentRecPath.points[currentPositonIndex], Heading);
                    ytList.loop = false;
                    ytList.points.Insert(0, new vec2(pivotAxlePosRP.easting, pivotAxlePosRP.northing));
                    isYouTurnTriggered = true;

                    //has a valid dubins path been created?
                    if (curList.points.Count < 2) return false;

                    return true;
                }
            }
            return false;
        }
    }//class
}//namespace