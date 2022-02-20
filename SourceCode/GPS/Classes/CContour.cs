using System;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
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
                totalWidth = ((mf.tool.toolWidth - mf.tool.toolOverlap) * pass) + spacingInt + ((mf.tool.toolWidth - mf.tool.toolOverlap) * 0.5);
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

        //Reset the contour to zip
        public void ResetContour()
        {
            creatingContour = null;
            curList?.Clear();
        }
    }//class
}//namespace