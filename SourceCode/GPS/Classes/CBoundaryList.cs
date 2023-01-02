using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CBoundaryList
    {
        //list of coordinates of boundary line
        public Polyline2 fenceLine = new Polyline2();
        public List<Polyline2> hdLine = new List<Polyline2>();
        public Polyline2 turnLine = new Polyline2();

        //area variable
        public double area = 0;

        //boundary variables
        public bool isDriveThru = false;

        public void FixFenceLine()
        {
            double spacing;
            //boundary point spacing based on eq width
            //close if less then 30 ha, 60ha, more then 60
            if (area < 200000) spacing = 1.7424;
            else if (area < 400000) spacing = 6.9696;
            else spacing = 15.6816;

            double distance;

            //make sure distance isn't too small between points on headland
            int j = fenceLine.points.Count - 1;
            for (int i = 0; i < fenceLine.points.Count; j = i++)
            {
                if (j < 0) j = fenceLine.points.Count - 1;
                distance = glm.DistanceSquared(fenceLine.points[i], fenceLine.points[j]);
                if (distance < spacing)
                {
                    fenceLine.points.RemoveAt(i);
                    i--;
                }
            }
        }

        public void CalculateFenceArea()
        {
            FixFenceLine();

            //make sure boundary is clockwise
            fenceLine.points.IsClockwise(true, out area);

            fenceLine.RemoveHandle();
            fenceLine.loop = true;
            //no idea why this fixes some strange bugs
            fenceLine = fenceLine.OffsetAndDissolvePolyline<Polyline2>(0.0)[0];
        }
    }
}