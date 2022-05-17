using System;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        public void AddPoint(vec2 pivot, double heading)
        {
            if (creatingContour == null)
            {
                creatingContour = new CGuidanceLine(Mode.Contour);
                curveArr.Add(creatingContour);
            }

            creatingContour.points.Add(new vec2(pivot.easting + Math.Cos(heading) * mf.tool.toolOffset, pivot.northing - Math.Sin(heading) * mf.tool.toolOffset));
        }

        //End the strip
        public void StopContourLine()
        {
            //make sure its long enough to bother
            if (creatingContour?.points.Count > 5)
            {
                creatingContour.points.AddFirstLastPoints(mf.tool.toolWidth + 3);

                //add the point list to the save list for appending to contour file
                mf.contourSaveList.Add(creatingContour.points);
            }
            else
                curveArr.Remove(creatingContour);

            creatingContour = null;
        }

        //build contours for boundaries
        public void BuildFenceContours(double pass, double spacing)
        {
            if (mf.bnd.bndList.Count == 0)
            {
                mf.TimedMessageBox(1500, "Boundary Contour Error", "No Boundaries Made");
                return;
            }

            double totalWidth = ((mf.tool.toolWidth - mf.tool.toolOverlap) * pass) + spacing;
            
            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                Polyline New = mf.bnd.bndList[j].fenceLine.OffsetAndDissolvePolyline(j == 0 ? totalWidth : -totalWidth, 0, -1, -1, true);
                New.loop = true;
                curveArr.Add(new CGuidanceLine(Mode.Contour, New));
            }

            mf.TimedMessageBox(1500, "Boundary Contour", "Contour Path Created");
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
    }//class
}//namespace