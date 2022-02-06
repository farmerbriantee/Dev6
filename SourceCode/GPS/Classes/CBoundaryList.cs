using System;

namespace AgOpenGPS
{
    public class CBoundaryList
    {
        //list of coordinates of boundary line
        public Polyline fenceLine = new Polyline();
        public Polyline hdLine = new Polyline();
        public Polyline turnLine = new Polyline();

        //area variable
        public double area;

        //boundary variables
        public bool isDriveThru;

        //constructor
        public CBoundaryList()
        {
            area = 0;
            isDriveThru = false;
        }

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
            int bndCount = fenceLine.points.Count;
            for (int i = 0; i < bndCount - 1; i++)
            {
                distance = glm.DistanceSquared(fenceLine.points[i], fenceLine.points[i + 1]);
                if (distance < spacing)
                {
                    fenceLine.points.RemoveAt(i + 1);
                    bndCount = fenceLine.points.Count;
                    i--;
                }
            }
        }

        //obvious
        public bool CalculateFenceArea()
        {
            FixFenceLine();

            int ptCount = fenceLine.points.Count;
            if (ptCount < 1) return false;

            area = 0;         // Accumulates area in the loop
            int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

            for (int i = 0; i < ptCount; j = i++)
            {
                area += (fenceLine.points[j].easting + fenceLine.points[i].easting) * (fenceLine.points[j].northing - fenceLine.points[i].northing);
            }

            bool isClockwise = area >= 0;

            area = Math.Abs(area / 2);

            //make sure boundary is clockwise
            if (!isClockwise)
                fenceLine.points.Reverse();

            //no idea why this fixes some strange bugs
            fenceLine = fenceLine.OffsetAndDissolvePolyline(0.001, true, -1, -1, true);

            return isClockwise;
        }
    }
}